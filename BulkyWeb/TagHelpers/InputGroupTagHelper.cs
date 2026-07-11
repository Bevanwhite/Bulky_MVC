using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BulkyWeb.TagHelpers;

[HtmlTargetElement("input-group")]
public class InputGroupTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;

    public InputGroupTagHelper(IHtmlGenerator generator) => _generator = generator;

    [HtmlAttributeName("asp-for")]
    public ModelExpression For { get; set; }

    [HtmlAttributeName("layout")]
    public string Layout { get; set; } = "row";

    [HtmlAttributeName("disabled")]
    public bool Disabled { get; set; } = false;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {

        var input = new TagBuilder("input");

        string inputType = GetInputType();
        input.Attributes.Add("type", inputType);
        input.Attributes.Add("name", For.Name);
        input.Attributes.Add("id", For.Name);

        if (For.Model != null)
            input.Attributes.Add("value", For.Model.ToString());

        string baseClass = Layout == "floating" ? "form-control border-0 shadow" : "form-control";
        input.AddCssClass(baseClass);


        // var input = _generator.GenerateTextBox(
        //     ViewContext,
        //     For.ModelExplorer,
        //     For.Name,
        //     For.Model,
        //     null,
        //     new
        //     {
        //         @class = Layout == "floating" ? "form-control border-0 shadow" : "form-control"
        //     }
        // );

        if (Disabled) input.Attributes.Add("disabled", "disabled");

        var tempInput = _generator.GenerateTextBox(ViewContext, For.ModelExplorer, For.Name, For.Model, null, null);
        foreach (var attr in tempInput.Attributes)
        {
            if (attr.Key.StartsWith("data-val"))
                input.Attributes[attr.Key] = attr.Value;
        }

        var modelState = ViewContext.ModelState[For.Name];
        bool hasError = modelState != null && modelState.Errors.Count > 0;

        if (hasError)
        {
            var existingClass = input.Attributes["class"]?.ToString() ?? "";
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
        TagBuilder validation = new TagBuilder("span");
        validation.Attributes.Add("data-valmsg-for", For.Name);
        validation.Attributes.Add("data-valmsg-replace", "true");
        validation.Attributes["id"] = For.Name + "-error"; // for aria-describedby

        if (hasError)
        {
            validation.AddCssClass("field-validation-error text-danger");
            validation.InnerHtml.Append(modelState.Errors[0].ErrorMessage);
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
        wrapper.InnerHtml.AppendHtml(label);
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
            if (dt == "integer" || dt == "number" || dt == "currency")
                return "number";
            if (dt == "emailaddress")
                return "email";
            if (dt == "date")
                return "date";
            // ... add others as needed
        }
        // Fallback to .NET type
        var type = metadata.UnderlyingOrModelType;
        if (type == typeof(int) || type == typeof(decimal) || type == typeof(double) || type == typeof(float))
            return "number";
        return "text";
    }
};

