using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BulkyBookWeb.TagHelpers;

[HtmlTargetElement("form-actions")]
public class FormActionsTagHelper : TagHelper
{
    public string SubmitText { get; set; } = "Submit";
    public string SubmitButtonClass { get; set; } = "btn-primary";
    public string Area { get; set; } = "Admin";
    public string Controller { get; set; } = "Category";
    public string BackAction { get; set; } = "Index";
    public string BackText { get; set; } = "Back to List";
    public string BackButtonClass { get; set; } = "btn-outline-primary";
    public string RowCss { get; set; } = "";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var html = $@"
            <div class=""row {RowCss}"">
                <div class= ""col-6 col-md-3"">
                    <button type=""submit"" 
                        class=""btn {SubmitButtonClass} form-control"">{SubmitText}
                    </button>
                </div>
                <div class=""col-6 col-md-3"">
                    <a asp-area=""{Area}"" asp-controller=""{Controller}"" 
                    asp-action=""{BackAction}"" href=""/{Area}/{Controller}""
                    class=""btn {BackButtonClass} form-control"">{BackText}</a>
                </div>
            </div>";

        output.TagName = null;
        output.Content.SetHtmlContent(html);
    }
}
