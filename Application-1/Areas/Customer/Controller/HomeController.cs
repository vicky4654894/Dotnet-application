using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application_1.Models.Models;
using Application_1.DataAccess.Repository.IRepository;
using System.Collections.Generic;
using Application_1.Areas.Admin.Controllers;
namespace Application_1.Areas.Customer.Controllers;
using Application_1.Models.Models;

    [Area("Customer")]
public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork=unitOfWork;
    }


    public IActionResult Index()
    {

        IEnumerable<Product> productsList = _unitOfWork.Product.GetAll(includeProperties : "Category");

        return View(productsList);
    }

    public IActionResult Details(int? id)
    {

        Console.WriteLine(id);    
    
        var productFromDb = _unitOfWork.Product.Get(p => p.Id == id,includeProperties: "Category");

        if(productFromDb == null)
        {
            Console.WriteLine("Not Found");
            return NotFound();
        }


        return View(productFromDb);
        
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
