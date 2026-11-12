using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BulkyBookWeb.TagHelpers;

[HtmlTargetElement("input-group")]
public class InputGroupTagHelper(IHtmlGenerator generator) : TagHelper
{
    private readonly IHtmlGenerator _generator = generator;

    [HtmlAttributeName("asp-for")]
    public ModelExpression For { get; set; } = null!;

    [HtmlAttributeName("layout")]
    public string Layout { get; set; } = "row";

    [HtmlAttributeName("input-type")]
    public string InputType { get; set; } = "Text";

    [HtmlAttributeName("disabled")]
    public bool Disabled { get; set; } = false;

    [HtmlAttributeName("validation")]
    public bool Validation { get; set; } = true;

    [HtmlAttributeName("textarea")]
    public bool TextArea { get; set; } = false;

    [HtmlAttributeName("rows")]
    public int Rows { get; set; } = 3;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var metadata = For.ModelExplorer.Metadata;

        bool isTextArea = TextArea
        || string.Equals(InputType, "textarea", StringComparison.OrdinalIgnoreCase)
        || string.Equals(metadata.DataTypeName, "MultilineText", StringComparison.OrdinalIgnoreCase);

        TagBuilder input;

        if (isTextArea)
        {
            input = new TagBuilder("textarea");
            input.Attributes.Add("name", For.Name);
            input.Attributes.Add("id", For.Name);
            input.Attributes.Add("rows", Rows.ToString());

            if (For.Model != null)
                input.InnerHtml.Append(For.Model.ToString() ?? string.Empty);

            if (Layout == "floating")
            {
                input.Attributes["placeholder"] = "";
                input.Attributes["style"] = "height: 100px;";
            }
        }
        else
        {
            input = new TagBuilder("input");
            input.Attributes.Add("type", GetInputType());
            input.Attributes.Add("name", For.Name);
            input.Attributes.Add("id", For.Name);

            if (For.Model != null)
                input.Attributes.Add("value", For.Model.ToString());
        }

        string baseClass = Layout == "floating"
        ? "form-control border-0 shadow"
        : "form-control";

        input.AddCssClass(baseClass);

        if (Disabled) input.Attributes.Add("disabled", "disabled");

        // 2. Copy client-side validation attributes from the framework's generated control
        TagBuilder tempInput = isTextArea
        ? _generator.GenerateTextArea(ViewContext, For.ModelExplorer, For.Name, Rows, 0, null)
        : _generator.GenerateTextBox(ViewContext, For.ModelExplorer, For.Name, For.Model, null, null);

        foreach (var attr in tempInput.Attributes)
        {
            if (attr.Key.StartsWith("data-val"))
                input.Attributes[attr.Key] = attr.Value;
        }

        var modelState = ViewContext.ModelState[For.Name];
        bool hasError = modelState != null && modelState.Errors.Count > 0;

        if (hasError)
        {
            var existingClass = input.Attributes["class"] ?? "";
            input.Attributes["class"] = existingClass + "input-validation-error";
            input.Attributes["aria-invalid"] = "true";
            input.Attributes["aria-describedby"] = For.Name + "-error";
        }

        var label = _generator.GenerateLabel(
            ViewContext,
            For.ModelExplorer,
            For.Name,
            null,
            new { @class = Layout == "floating" ? "ms-2" : "p-0" }
        );

        // 4. Build the validation span manually (to ensure correct classes and message)
        TagBuilder validation = new("span");
        validation.Attributes.Add("data-valmsg-for", For.Name);
        validation.Attributes.Add("data-valmsg-replace", "true");
        validation.Attributes["id"] = For.Name + "-error"; // for aria-describedby

        if (hasError)
        {
            validation.AddCssClass("field-validation-error text-danger");
            var errorMessage = modelState?.Errors[0].ErrorMessage;
            if (errorMessage != null)
                validation.InnerHtml.Append(errorMessage);
        }
        else
        {
            validation.AddCssClass("field-validation-valid");
            // Empty content when valid
        }

        var wrapper = new TagBuilder("div");
        if (Layout == "floating")
            wrapper.AddCssClass("form-floating py-2 col-12");
        else
            wrapper.AddCssClass("mb-3 row p-1");

        // Assemble
        wrapper.InnerHtml.AppendHtml(input);
        if (!isTextArea)
            wrapper.InnerHtml.AppendHtml(label);
        if (Validation)
            wrapper.InnerHtml.AppendHtml(validation);

        output.TagName = null;
        output.Content.SetHtmlContent(wrapper);

    }

    private string GetInputType()
    {
        var metadata = For.ModelExplorer.Metadata;
        // Check DataType attribute
        if (metadata.DataTypeName != null)
        {
            var dt = metadata.DataTypeName.ToLower();
            if (dt == "upload" || dt == "file" || dt == "imageurl")
                return "file";
            if (dt == "integer" || dt == "number" || dt == "currency")
                return "number";
            if (dt == "emailaddress")
                return "email";
            if (dt == "date")
                return "date";
            // ... add others as needed
        }
        var type = metadata.UnderlyingOrModelType;

        // File types
        if (typeof(IFormFile).IsAssignableFrom(type)
            || type == typeof(IFormFile)
            || type == typeof(IFormFileCollection)
            || typeof(IEnumerable<IFormFile>).IsAssignableFrom(type)
            || type == typeof(byte[]))          // often used for file/image storage
        {
            return "file";
        }

        if (InputType == "file")
            return "file";
        // Fallback to .NET type
        if (type == typeof(int) || type == typeof(decimal) || type == typeof(double) || type == typeof(float))
            return "number";


        return "text";
    }
};

