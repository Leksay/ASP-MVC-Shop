using BulkiBook.DataAccess.Repository.IRepository;
using BulkiBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;

[Area("Admin")]
public class CompanyController : Controller
{
    private IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public CompanyController(IUnitOfWork db, IWebHostEnvironment hostEnvironment)
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
        var company = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);

        if (id == null || id == 0)
        {
            // create product
            return View(new Company());
        }
        else
        {
            // update
            return View(company);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company company)
    {
        if (company == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            TempData["error"] = "Please, fill all form values";
            return View(company);
        }

        var rootPath = _hostEnvironment.WebRootPath;

        TempData["success"] = "Product updated successfully";

        if(company.Id == 0)
        {
            _unitOfWork.Company.Add(company);
        }
        else
        {
            _unitOfWork.Company.Update(company);
        }

        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }



    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Company.GetAll();
        return Json(new { data = productList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var company = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);

        if (company == null)
        {
            return Json(new
            {
                success = false,
                message = $"Error while deleting. Product with id {id} not found"
            });
        }

        _unitOfWork.Company.Remove(company);
        _unitOfWork.Save();

        return Json(new
        {
            success = true,
            message = $"Product {company.Name} successfully deleted."
        });
    }
    #endregion
}
