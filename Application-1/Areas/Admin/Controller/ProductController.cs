using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application_1.Models.Models;
using Application_1.DataAccess.Repository.IRepository;

namespace Application_1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController:Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;    
        }
        
        public IActionResult Index()
        {
             ProductViewModel productViewModel=null;
            try
            {
            IEnumerable<Product> list = _unitOfWork.Product.GetAll();
            Product product = new Product();
            productViewModel = new ProductViewModel
            {
                Products = list,
                Product=product
            }; 
            }
            catch(Exception e)
            {
                TempData["error"] = e.Message;
            }    
            return View(productViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
            var productFromDb = _unitOfWork.Product.Get(u=>u.Id==id);
            if(productFromDb==null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel productViewModel)
        {   
            try
            {
                if (!ModelState.IsValid)
                {  
                    ProductViewModel model = new ProductViewModel
                    {
                    Products = _unitOfWork.Product.GetAll(),
                    Product= new Product()
                    };
                    TempData["error"] = "Something went wrong";
                    return View(model);
                }
                _unitOfWork.Product.Add(productViewModel.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                TempData["error"] = e.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["error"] = "Something went wrong!";
                    return View(product);
                }
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                TempData["error"] = e.Message;
                return RedirectToAction("Index");      
            }
        }

        public IActionResult Delete(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
            var productFromDb = _unitOfWork.Product.Get(u=>u.Id==id);
            if(productFromDb==null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(productFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }

    }
}
