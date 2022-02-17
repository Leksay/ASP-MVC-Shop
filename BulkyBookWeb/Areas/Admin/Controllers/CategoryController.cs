using BulkiBook.DataAccess.Repository.IRepository;
using BulkiBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _db;

    public CategoryController(IUnitOfWork db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        IEnumerable<Category> categoryList = _db.Category.GetAll();
        return View(categoryList);
    }

    //GET
    public IActionResult Create()
    {
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if(ModelState.IsValid && !CheckForCustomValidationErrors(obj))
        {
            _db.Category.Add(obj);
            _db.Save();

            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index", "Category");
        }

        return View(obj);
    }

    public IActionResult Edit(int? id)
    {
        if(id == null || id == 0)
        {
            return NotFound();
        }

        var category = _db.Category.GetFirstOrDefault(c => c.Id == id);

        if(category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category obj)
    {
        if (ModelState.IsValid && !CheckForCustomValidationErrors(obj))
        {
            _db.Category.Update(obj);
            _db.Save();

            TempData["success"] = "Category updated successfully";

            return RedirectToAction("Index", "Category");
        }

        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var category = _db.Category.GetFirstOrDefault(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    //POST
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var category = _db.Category.GetFirstOrDefault(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        _db.Category.Remove(category);
        _db.Save();

        TempData["success"] = "Category deleted successfully";

        return RedirectToAction(nameof(Index), "Category");
    }

    public bool CheckForCustomValidationErrors(Category obj)
    {
        bool hasErrors = false;
        if(obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("CustomError", "The DisplayOrder cannot exactly math Name");
            hasErrors = true;
        }

        if(string.IsNullOrEmpty(obj.Name))
        {
            ModelState.AddModelError(nameof(obj.Name), "Name must not be empty");
            hasErrors = true;
        }
        else if(obj.Name.Length < 5 || obj.Name.Length > 25)
        {
            ModelState.AddModelError(nameof(obj.Name), "Name characters length must be more than 5 and less than 25");
            hasErrors = true;
        }

        if(obj.DisplayOrder <= 0)
        {
            ModelState.AddModelError(nameof(obj.DisplayOrder), "Order number must be more than 0");
            hasErrors = true;
        }

        return hasErrors;
    }
}