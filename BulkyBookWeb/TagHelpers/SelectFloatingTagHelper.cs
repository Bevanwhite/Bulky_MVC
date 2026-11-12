using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BulkyBookWeb.TagHelpers;

// Renders a Bootstrap 5 "form-floating" wrapper around a model-bound
// <select> element, together with its <label> and
// validation-message span.
// Usage: <select-floating asp-for="..." asp-items="..."></select-floating>
[HtmlTargetElement("select-floating")]
public class SelectFloatingTagHelper(IHtmlGenerator generator) : TagHelper
{
    // MVC's HTML generator. Produces the same name, id, data-val-*
    // and selected-option markup that <select asp-for>, <label asp-for>
    // and <span asp-validation-for> would generate.
    private readonly IHtmlGenerator _generator = generator;

    // The current MVC view context (ModelState, ViewData, model metadata).
    // Supplied automatically by MVC and not bindable from markup.
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    // Bound model property (asp-for). Provides the expression name,
    // metadata and current value used for binding, labels and validation.
    [HtmlAttributeName("asp-for")]
    public ModelExpression For { get; set; } = null!;

    // The list of options to render inside the <select> (asp-items).
    [HtmlAttributeName("asp-items")]
    public IEnumerable<SelectListItem> Items { get; set; } = [];


    // Text shown for the "empty" placeholder <option>.
    // Defaults to "--Select Items--".
    [HtmlAttributeName("placeholder")]
    public string Placeholder { get; set; } = "--Select Items--";

    // The value posted when the placeholder is chosen.
    // Defaults to empty so the model binder receives "no value".
    [HtmlAttributeName("placeholder-value")]
    public string PlaceholderValue { get; set; } = null!;

    // When true, the placeholder option is rendered with disabled="disabled".
    // Defaults to false so the user can always return to "no selection".
    [HtmlAttributeName("placeholder-disabled")]
    public bool PlaceholderDisabled { get; set; } = true;

    // Builds the final markup:
    // 1. Wraps everything in a <div class="form-floating ...">.
    // 2. Generates the base <select> and its options via IHtmlGenerator.
    // 3. Prepends a selectable placeholder <option> that is
    //    auto-selected when the model value is empty.
    // 4. Generates the matching <label> and validation <span>.
    // 5. Appends select -> label -> validation (required order for form-floating).
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Outer Bootstrap wrapper instead of a raw <select>.
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", "form-floating py-2 col-12");

        // Real <select> with name/id/data-val-* and the supplied <option>s.
        // optionLabel: null => we render our own placeholder below.
        var select = _generator.GenerateSelect(
            ViewContext,
            For.ModelExplorer,
            optionLabel: null,
            expression: For.Name,
            selectList: Items,
            allowMultiple: false,
            htmlAttributes: new { @class = "form-control border-0 shadow" }
        );

        // Custom placeholder <option> with an explicit empty value.
        var placeholderOption = new TagBuilder("option");
        placeholderOption.Attributes["value"] = PlaceholderValue ?? string.Empty;

        if (PlaceholderDisabled)
            placeholderOption.Attributes["disabled"] = "disabled";

        // Select the placeholder when the model currently holds no value.
        if (string.IsNullOrEmpty(For.Model?.ToString()))
            placeholderOption.Attributes["selected"] = "selected";

        placeholderOption.InnerHtml.Append(Placeholder);

        // Reassemble: copy the generated attributes, then placeholder, then options.
        var finalSelect = new TagBuilder("select");
        finalSelect.MergeAttributes(select.Attributes);
        finalSelect.InnerHtml.AppendHtml(placeholderOption);
        finalSelect.InnerHtml.AppendHtml(select.InnerHtml);

        // Matching <label for="...">, text from [Display(Name=...)]
        var label = _generator.GenerateLabel(
            ViewContext,
            For.ModelExplorer,
            For.Name,
            labelText: null,
            htmlAttributes: new { @class = "ms-2" });

        // Validation <span> that unobtrusive validation fills in.
        var validation = _generator.GenerateValidationMessage(
            ViewContext,
            For.ModelExplorer,
            For.Name,
            message: null,
            tag: null,
            htmlAttributes: new { @class = "text-danger" });

        // Emit in form-floating order: input -> label -> validation.
        output.Content.AppendHtml(finalSelect);
        output.Content.AppendHtml(label);
        output.Content.AppendHtml(validation);
    }

}