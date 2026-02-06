using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application_1.Areas.Admin.Models;
using Application_1.Models.Models;
using Application_1.DataAccess.Repository.IRepository;
using System.Linq.Expressions;
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
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment =  hostEnvironment;
            _unitOfWork = unitOfWork;    
        }
        
        public IActionResult Index(string? search,int page = 1)
        {
             ProductViewModel productViewModel=null;
            try
            {
            int pageSize = 5;
            Expression<Func<Product,bool>> filter = null;

                if (!string.IsNullOrEmpty(search))
                {
                    filter = p =>
                    p.Title.Contains(search) ||
                    p.Author.Contains(search) ||
                    p.Category.Name.Contains(search) ||
                    p.ISBN.Contains(search)||
                    p.Description.Contains(search);
                }
            IEnumerable<Product> list = _unitOfWork.Product.GetAll(
                filter:filter,
                includeProperties : "Category",
                pageNumber:page,
                pageSize:pageSize);

            int totalRecords = _unitOfWork.Product.Count(filter);
            int totalPages = (int)Math.Ceiling((double)totalRecords/pageSize);

            productViewModel = new ProductViewModel
            {
                Products = list,
                CurrentPage=page,
                Search=search,
                TotalPages=totalPages
            }; 
            }
            catch(Exception e)
            {
                TempData["error"] = e.Message;
            }    
            return View(productViewModel);
        }

        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel=null;
            try
            {
                productViewModel = new()
            {
                CategoryList = _unitOfWork.Category.GetAll()
                .Select(i=> new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }).ToList()
            }; 
         
            if(id==0 || id==null)
            {
                return View(productViewModel);
            }
        
            var productFromDb = _unitOfWork.Product.Get(u=>u.Id==id,includeProperties : "Category");
            if(productFromDb==null)
            { 
                return NotFound();  
            }
            productViewModel.Products = _unitOfWork.Product.GetAll(includeProperties : "Category");
            productViewModel.Product = productFromDb;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View(productViewModel);
            
        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
    try
    {
        // 1️⃣ Validate image
        if (productViewModel.ImageFile == null && productViewModel.Product.Id == 0)
        {
            ModelState.AddModelError("ImageFile", "Please upload an image file.");
        }

        if (productViewModel.ImageFile != null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(productViewModel.ImageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("ImageFile",
                    "Invalid image file type. Allowed types: " + string.Join(", ", allowedExtensions));
            }
        }

        // 2️⃣ Stop here if invalid
        if (!ModelState.IsValid)
        {
            productViewModel.CategoryList = _unitOfWork.Category.GetAll()
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }).ToList();
            
            foreach (var entry in ModelState)
    {
        string fieldName = entry.Key;

        foreach (var error in entry.Value.Errors)
        {
            string errorMessage = error.ErrorMessage;
            Console.WriteLine($"Field: {fieldName}, Error: {errorMessage}");
        }
    }

            TempData["error"] = "Model is not valid!";
            return View(productViewModel);
        }
        
        // 3️⃣ Handle file upload
        if (productViewModel.ImageFile != null)
        {
            Console.WriteLine("<-------------------------------Uploading image file-------------------------------> ");
            string uploads = Path.Combine(_hostEnvironment.WebRootPath, "images");
            Directory.CreateDirectory(uploads);

            string extension = Path.GetExtension(productViewModel.ImageFile.FileName);
            string fileName = Guid.NewGuid() + extension;
            string filePath = Path.Combine(uploads, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                productViewModel.ImageFile.CopyTo(fileStream);
            }

            var productFromDb1 = _unitOfWork.Product.Get(u => u.Id == productViewModel.Product.Id);
            if (productFromDb1 != null)
            {
                var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, productFromDb1.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                    Console.WriteLine("Old image deleted successfully: " + oldImagePath);
                }
            }
            

            productViewModel.Product.ImageUrl = "/images/" + fileName;
            Console.WriteLine("Image uploaded successfully: " + productViewModel.Product.ImageUrl);

        }

        // 4️⃣ Insert or Update
        int id = productViewModel.Product.Id;
        var productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

        if (productFromDb == null)
        {
            _unitOfWork.Product.Add(productViewModel.Product);
            TempData["success"] = "Product created successfully";
        }
        else
        {
            productFromDb.Title = productViewModel.Product.Title;
            productFromDb.Description = productViewModel.Product.Description;
            productFromDb.ISBN = productViewModel.Product.ISBN;
            productFromDb.Author = productViewModel.Product.Author;
            productFromDb.ListPrice = productViewModel.Product.ListPrice;
            productFromDb.Price50 = productViewModel.Product.Price50;
            productFromDb.Price100 = productViewModel.Product.Price100;
            productFromDb.CategoryId = productViewModel.Product.CategoryId;
            if (!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
            {
                productFromDb.ImageUrl = productViewModel.Product.ImageUrl;
            }
            _unitOfWork.Product.Update(productViewModel.Product);
            
            TempData["success"] = "Product updated successfully!";
        }
        _unitOfWork.Save();
        }
        catch (Exception e)
        {
        TempData["error"] = e.Message;
        Console.WriteLine("Exception caught in Product Create Post: " + e.Message);
        }
        return RedirectToAction("Index");
    }

        public IActionResult Delete(int? id)
        {
            try
            {
                var productFromDb = _unitOfWork.Product.Get(u=>u.Id==id);
            if(productFromDb==null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(productFromDb);
            _unitOfWork.Save();
            //delete the image
              if (productFromDb != null)
            {
                var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, productFromDb.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                    Console.WriteLine("Old image deleted successfully: " + oldImagePath);
                }
            }
            TempData["success"] = "Product deleted successfully!";
            }
            catch(Exception e)
            {
                TempData["error"]= e.Message;
                Console.WriteLine(e.Message);
            }

            if(id==null || id==0)
            {
                return NotFound();
            }
            
            return RedirectToAction("Index");
        }

    }
}
