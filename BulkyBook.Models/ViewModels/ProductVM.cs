using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Models.ViewModels;

public class ProductVM
{
    public Product Product { get; set; } = new();
    public IEnumerable<SelectListItem> CategoryList { get; set; } = [];
}
