using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application_1.Models.Models;
using Application_1.DataAccess.Repository.IRepository;

namespace Application_1.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CategoryController:Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;    
        }
       
        
        
        public IActionResult Index()
        {
             CategoryViewModel categoryViewModel=null;
            try
            {
            IEnumerable<Category> list = _unitOfWork.Category.GetAll();
            Category category = new Category();
            categoryViewModel = new CategoryViewModel
            {
                Categories = list,
                Category=category
            }; 
            }
            catch(Exception e)
            {
                TempData["error"] = e.Message;
            }

            
            return View(categoryViewModel);
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel categoryViewModel)
        {

            try
            {
                //Category Name should not be a number
            if (int.TryParse(categoryViewModel.Category.Name,out _))
            {
                ModelState.AddModelError("Category.Name","Category Name cannot be number!");
            }
            if (!ModelState.IsValid)
            {  
                CategoryViewModel model = new CategoryViewModel
                {
                Categories = _unitOfWork.Category.GetAll(),
                Category= new Category()
                };
            
                TempData["error"] = "Something went wrong!";
            return View("Index", model);
            } 
            _unitOfWork.Category.Add(categoryViewModel.Category);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully!";

            }
            catch(Exception e)
            {
            TempData["error"] = e.Message;
                
            }
 
            return RedirectToAction("Index");
  
        }

        public IActionResult Edit(CategoryViewModel categoryViewModel)
        {

            try{    
            var id = categoryViewModel.Category.Id;
            var categoryUpdate = _unitOfWork.Category.Get(c => c.Id == id);
            if(categoryUpdate == null || id == 0)
            {
                TempData["error"] = "Category not found!";
                return RedirectToAction("Index");
            }

            categoryUpdate.Name=categoryViewModel.Category.Name;
            categoryUpdate.DisplayOrder=categoryViewModel.Category.DisplayOrder;
            _unitOfWork.Category.Update(categoryUpdate);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully!";
            }
            catch(Exception e)
            {
                TempData["error"] = e.Message;
            }


            return RedirectToAction("Index");
        }

        public IActionResult Delete(CategoryViewModel categoryViewModel)
        {

            try{
            var id = categoryViewModel.Category.Id;
            var categoryUpdate = _unitOfWork.Category.Get(c => c.Id == id);
            if(categoryUpdate == null || id == 0)
            {
                return RedirectToAction("Index");
            }
                
            _unitOfWork.Category.Remove(categoryUpdate);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully!";
            }
            catch(Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction("Index");
        }


   
    }



    

}