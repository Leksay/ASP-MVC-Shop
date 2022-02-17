using BulkiBook.DataAccess.Repository.IRepository;
using BulkiBook.Models;
using BulkiBook.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BulkyBookWeb.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private IUnitOfWork _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork db, IWebHostEnvironment hostEnvironment)
    {
        _db = db;
        _hostEnvironment = hostEnvironment;
    }

    public IActionResult Index()
    {
        return View();
    }

    //GET
    public IActionResult Upsert(int? id)
    {
        ProductViewModel productVM = new()
        {
            Product = new(),
            CategoryList = _db.Category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }),

            CoverTypeList = _db.CoverType.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
        };

        if (id == null || id == 0)
        {
            // create product
            return View(productVM);
        }
        else
        {
            productVM.Product = _db.Product.GetFirstOrDefault(x => x.Id == id);
            return View(productVM);
            // update
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductViewModel productVM, IFormFile? file)
    {
        if (productVM == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            TempData["error"] = "Please, fill all form values";
            return View(productVM);
        }

        var rootPath = _hostEnvironment.WebRootPath;

        if (file != null)
        {
            var fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(rootPath, @"images\products\");
            var extention = Path.GetExtension(file.FileName);

            using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            productVM.Product.ImageUrl = @"\images\products\" + fileName + extention;
        }

        TempData["success"] = "Product updated successfully";

        _db.Product.Add(productVM.Product);
        _db.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var coverType = _db.CoverType.GetFirstOrDefault(c => c.Id == id);

        if (coverType == null)
        {
            return NotFound();
        }

        return View(coverType);
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

        var coverType = _db.CoverType.GetFirstOrDefault(c => c.Id == id);

        if (coverType == null)
        {
            return NotFound();
        }

        _db.CoverType.Remove(coverType);
        _db.Save();

        TempData["success"] = $"Cover Type {coverType.Name} deleted successfully";

        return RedirectToAction(nameof(Index));
    }


    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _db.Product.GetAll(includeProperties: "Category,CoverType");
        return Json(new { data = productList });
    }
    #endregion
}
