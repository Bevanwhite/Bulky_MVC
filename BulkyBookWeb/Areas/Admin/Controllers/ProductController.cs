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

    public IActionResult Create()
    {

        ProductVM productVM = new()
        {
            CategoryList = _unitOfWork.Category.GetAll()
            .Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            Product = new Product()
        };

        return View(productVM);
    }

    [HttpPost]
    public IActionResult Create(ProductVM productVM)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Add(productVM.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";
            return RedirectToAction("Index");
        }
        else
        {
            productVM.CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            return View(productVM);
        }
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Product productFormDb = _unitOfWork.Product.Get(u => u.Id == id)
            ?? throw new ArgumentException("Id is not found");

        if (productFormDb == null)
        {
            NotFound();
        }

        return View(productFormDb);
    }

    [HttpPost]
    public IActionResult Edit(Product obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product Updated successfully";
            return RedirectToAction("Index");
        }

        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Product productFormDb = _unitOfWork.Product.Get(u => u.Id == id)
            ?? throw new ArgumentException("Id is not found");

        if (productFormDb == null)
        {
            NotFound();
        }

        return View(productFormDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Product obj = _unitOfWork.Product.Get(u => u.Id == id)
            ?? throw new ArgumentException("Id is not found");

        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Product Deleted successfully";
        return RedirectToAction("Index");
    }

}

