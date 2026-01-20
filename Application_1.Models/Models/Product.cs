using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace Application_1.Models.Models{

public class Product
{
    
    [Key]
    public int Id{set;get;}

    [Required(ErrorMessage = "Product title is required")]
    [MaxLength(100, ErrorMessage ="Title cannot be more than 100 characters")]
    [DisplayName("Product Title")]
    public string Title{set;get;}

    [DisplayName("Product Description")]
    [Required(ErrorMessage = "Product description is required")]
    [MinLength(40, ErrorMessage ="Description should be at least 40 characters long")]
    public string Description{set;get;}

    [Required(ErrorMessage = "ISBN is required")]
    [MaxLength(13, ErrorMessage ="ISBN cannot be more than 13 characters")]
    [DisplayName("ISBN")]
    public string ISBN{set;get;}

    [Required(ErrorMessage = "Author name is required")]
    [MaxLength(500, ErrorMessage ="Author name cannot be more than 500 characters")]
    [DisplayName("Author Name")]
    public string Author{set;get;}

    [Required(ErrorMessage = "List Price is required")]
    [Range(1, 10000, ErrorMessage ="List Price must be between 1 and 10,000")]
    [DisplayName("List Price")]
    public double ListPrice{set;get;}


    [Required(ErrorMessage = "Price for 1-49 units is required")]
    [Range(1, 100000, ErrorMessage ="Price must be between 1 and 10,000")]
    [DisplayName("Price for 1-49 Units")]
    public double Price50{set;get;}


    [Required(ErrorMessage = "Price for 50-99 units is required")]
    [Range(1, 100000, ErrorMessage ="Price must be between 1 and 10,000")]
    [DisplayName("Price for 50-99 Units")]
    public double Price100{set;get;}  

    [Required(ErrorMessage ="Category is required")]
    public int CategoryId{set;get;}
    [ForeignKey("CategoryId")]

    [ValidateNever]
    public Category Category{set;get;}  

    [DisplayName("Image URL")]
    [MaxLength(3000, ErrorMessage ="Image URL cannot be more than 3000 characters")]
    [Required(ErrorMessage = "Image URL is required")]
    public string ImageUrl{set;get;}




    }
}