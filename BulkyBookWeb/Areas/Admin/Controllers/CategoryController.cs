using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: CategoryController
    public IActionResult Index()
    {
        List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
        return View(objCategoryList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category obj)
    {
        // if (obj.Name == obj.DisplayOrder.ToString())
        // {
        //     ModelState.AddModelError("name", "The Display Order can't exactly match the Name.");
        // }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Category categoryFormDb = _unitOfWork.Category.Get(u => u.Id == id) ?? throw new ArgumentException("Id is not found");
        // Category categoryFormDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
        // Category categoryFormDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

        if (categoryFormDb == null)
        {
            NotFound();
        }

        return View(categoryFormDb);
    }

    [HttpPost]
    public IActionResult Edit(Category obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully";
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

        Category categoryFormDb = _unitOfWork.Category.Get(u => u.Id == id) ?? throw new ArgumentException("Id is not found");
        // Category categoryFormDb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
        // Category categoryFormDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

        if (categoryFormDb == null)
        {
            return NotFound();
        }
        return View(categoryFormDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Category obj = _unitOfWork.Category.Get(u => u.Id == id) ?? throw new ArgumentException("Id is not found");

        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Category delete successfully";
        return RedirectToAction("Index");
    }
}

