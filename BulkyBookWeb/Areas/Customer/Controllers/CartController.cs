using BulkiBook.DataAccess.Repository.IRepository;
using BulkiBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ShoppingCartVM ShoppingCartVM{ get; set; }
    public int OrderTotal { get; set; }

    public CartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        ShoppingCartVM = new ShoppingCartVM()
        {
            ListCart = _unitOfWork.ShoppingCart.GetAll(U => U.ApplicationUserId == claim.Value
            , includeProperties: "Product")
        };

        foreach(var cart in ShoppingCartVM.ListCart)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
            ShoppingCartVM.Total += cart.Price;
        }

        return View(ShoppingCartVM);
    }

    public IActionResult Summary()
    {
        //var claimsIdentity = (ClaimsIdentity)User.Identity;
        //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        //ShoppingCartVM = new ShoppingCartVM()
        //{
        //    ListCart = _unitOfWork.ShoppingCart.GetAll(U => U.ApplicationUserId == claim.Value
        //    , includeProperties: "Product")
        //};

        //foreach (var cart in ShoppingCartVM.ListCart)
        //{
        //    cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
        //    ShoppingCartVM.Total += cart.Price;
        //}

        //return View(ShoppingCartVM);

        return View();
    }

    public IActionResult Plus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);
        _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);
        _unitOfWork.ShoppingCart.DecrementCount(cart, 1);

        if(cart.Count == 0)
        {
            Remove(cartId);
        }

        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);
        _unitOfWork.ShoppingCart.Remove(cart);
        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100) => quantity switch
    {
        < 50 => price,
        <= 100 => price50,
        >= 100 => price100
    };
}
