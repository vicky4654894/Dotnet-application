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
            Console.WriteLine("Inside Category Index");
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
    Console.WriteLine("Inside Category Create");
    bool isValid=true;
    bool exists = _unitOfWork.Category
        .GetAll()
        .Any(c => c.Name.Trim().ToLower() ==
                  categoryViewModel.Category.Name.Trim().ToLower());
    if (exists)
    {
        ModelState.AddModelError(
            "Category.Name",
            "Category with this name already exists!"
        );
        isValid=false;
    }

    if (!string.IsNullOrWhiteSpace(categoryViewModel.Category.Name) &&
        int.TryParse(categoryViewModel.Category.Name, out _))
    {
        ModelState.AddModelError(
            "Category.Name",
            "Category Name cannot be a number!"
        );
        isValid=false;
    }

    if (!string.IsNullOrWhiteSpace(categoryViewModel.Category.Name) &&
        categoryViewModel.Category.Name.Trim().ToLower() ==
        categoryViewModel.Category.DisplayOrder.ToString())
    {
        ModelState.AddModelError(
            "Category.Name",
            "Category Name and Display Order cannot be the same!"
        );

        ModelState.AddModelError(
            "Category.DisplayOrder",
            "Display Order must be different from Category Name!"
        );
        isValid=false;
    }
    if (!isValid)
    {
        var categories = _unitOfWork.Category.GetAll();
        categoryViewModel.Categories = categories;
        
        return View("Index", categoryViewModel);
    }

    _unitOfWork.Category.Add(categoryViewModel.Category);
    _unitOfWork.Save();

    TempData["success"] = "Category created successfully!";

    return RedirectToAction("Index");
}

        [HttpPost]
           public IActionResult Edit(CategoryViewModel categoryViewModel)
        {
            Console.WriteLine("Inside Category Edit");
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
            Console.WriteLine("Inside Category Delete");
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