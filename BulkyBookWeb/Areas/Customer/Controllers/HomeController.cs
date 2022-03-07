using BulkiBook.DataAccess.Repository.IRepository;
using BulkiBook.Models;
using BulkiBook.Models.ViewModels;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> productLust = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");

        return View(productLust);
    }

    public IActionResult Details(int productId)
    {
        ShoppingCardModel shoppingCart = new()
        {
            Count = 1,
            ProductId = productId,
            Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == productId, includeProperties: "Category,CoverType"),
        };

        return View(shoppingCart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Details(ShoppingCardModel shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        shoppingCart.ApplicationUserId = claim.Value;

        var cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.ApplicationUserId == claim.Value && x.ProductId == shoppingCart.ProductId);

        if(cartFromDb == null)
        {
            _unitOfWork.ShoppingCart.Add(shoppingCart);
            //_unitOfWork.SaveWithIdentiryInsert("Carts");
            _unitOfWork.Save();
        }
        else
        {
            _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, shoppingCart.Count);
            _unitOfWork.Save();
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
