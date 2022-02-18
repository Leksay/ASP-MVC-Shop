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
    private IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork db, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = db;
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
            CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }),

            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(x => new SelectListItem
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
            productVM.Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
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
            var currentImageUrl = productVM.Product.ImageUrl;

            if(!string.IsNullOrEmpty(currentImageUrl))
            {
                var currentImageFile = Path.Combine(rootPath, currentImageUrl.TrimStart('\\'));

                if(System.IO.File.Exists(currentImageFile))
                {
                    System.IO.File.Delete(currentImageFile);
                }
            }

            using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            productVM.Product.ImageUrl = @"\images\products\" + fileName + extention;
        }

        TempData["success"] = "Product updated successfully";

        if(productVM.Product.Id == 0)
        {
            _unitOfWork.Product.Add(productVM.Product);
        }
        else
        {
            _unitOfWork.Product.Update(productVM.Product);
        }

        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }



    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
        return Json(new { data = productList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var product = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);

        if (product == null)
        {
            return Json(new
            {
                success = false,
                message = $"Error while deleting. Product with id {id} not found"
            });
        }

        var currentImageUrl = product.ImageUrl;

        if (!string.IsNullOrEmpty(currentImageUrl))
        {
            var currentImageFile = Path.Combine(_hostEnvironment.WebRootPath, currentImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(currentImageFile))
            {
                System.IO.File.Delete(currentImageFile);
            }
        }

        _unitOfWork.Product.Remove(product);
        _unitOfWork.Save();

        return Json(new
        {
            success = true,
            message = $"Product {product.Title} successfully deleted."
        });
    }
    #endregion
}
