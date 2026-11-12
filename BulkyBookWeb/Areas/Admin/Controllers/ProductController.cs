using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{

    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: ProductController
    public ActionResult Index()
    {
        List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
        return View(objProductList);
    }

    public IActionResult Upsert(int? id)
    {

        ProductVM productVm = new()
        {
            CategoryList = _unitOfWork.Category.GetAll()
            .Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            Product = new Product()
        };

        if (id is not null or 0)
        {
            productVm.Product = _unitOfWork.Product.Get(u => u.Id == id)
            ?? throw new ArgumentException("Id is not found");
        }

        return View(productVm);
    }

    [HttpPost]
    public IActionResult Upsert(ProductVM productVm, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Add(productVm.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";
            return RedirectToAction("Index");
        }
        else
        {
            productVm.CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            return View(productVm);
        }
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Product? productFormDb = _unitOfWork.Product.Get(u => u.Id == id);

        if (productFormDb == null)
        {
            NotFound("Id is not found");
        }

        return View(productFormDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Product? obj = _unitOfWork.Product.Get(u => u.Id == id);

        if (obj == null)
        {
            return NotFound("Id is not found");
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Product Deleted successfully";
        return RedirectToAction("Index");
    }

}

