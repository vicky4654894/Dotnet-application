using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Application_1.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace Application_1.Areas.Admin.Models
{
public class ProductViewModel
{
    [ValidateNever]
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public Product Product { get; set; } = new Product();

    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();

    public IFormFile? ImageFile { get; set; }

    public int CurrentPage{get;set;}

    public int TotalPages{get;set;}

    public string Search{get;set;}

    
}
}