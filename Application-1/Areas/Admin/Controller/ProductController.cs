using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application_1.Areas.Admin.Models;
using Application_1.Models.Models;
using Application_1.DataAccess.Repository.IRepository;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;


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
            productViewModel = new ProductViewModel
            {
                Products = list,
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
            ProductViewModel productViewModel=null;
            try
            {
            IEnumerable<Product> list = _unitOfWork.Product.GetAll();
            Product product = new Product();
            productViewModel = new ProductViewModel
            {
                Products = list,
                Product=product,
                CategoryList = _unitOfWork.Category.GetAll().Select(i=> new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }).ToList()
            }; 
            }
            catch(Exception e)
            {
                TempData["error"] = e.Message;
            }    
            return View(productViewModel);
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
                    Product= new Product(),
                     CategoryList = _unitOfWork.Category.GetAll().Select(i=> new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }).ToList()
                    };
                    TempData["error"] = "Something went wrong";
                    return View(model);
                }
                productViewModel.Product.Category=null;
                _unitOfWork.Product.Add(productViewModel.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                TempData["error"] = e.Message;
                Console.WriteLine("Exception caught in Product Create Post: " + e.Message);
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
