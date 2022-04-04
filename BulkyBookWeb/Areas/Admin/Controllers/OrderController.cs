using BulkiBook.DataAccess.Repository.IRepository;
using BulkiBook.Models.ViewModels;
using BulkiBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    [BindProperty]
    public OrderVM OrderVM { get; set; }

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Details(int orderId)
    {
        OrderVM = new()
        {
            OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
            OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderId == orderId, includeProperties: "Product"),

        };
        return View(OrderVM);
    }

    #region API CALLS

    [HttpGet]
    public IActionResult GetAll(string status)
    {
        var orderHeaders = User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee) 
            ? _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser")
            : _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == GetClaims().Value, includeProperties: "ApplicationUser");

        orderHeaders = status switch
        {
            "pending" => orderHeaders.Where(u => u.PaymentStatus is SD.PaymentStatusDelayPayment),
            "inprocess" => orderHeaders.Where(u => u.OrderStatus == SD.StatusProcessing),
            "completed" => orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped),
            _ => orderHeaders
        };

        return Json(new { data = orderHeaders });
    }

    private Claim GetClaims()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
    }
    #endregion
}
