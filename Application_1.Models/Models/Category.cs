using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace Application_1.Models.Models{

public class Category
{
    
    [Key]
    public int Id{set;get;}

    [Required(ErrorMessage = "Category name is required")]
    [MaxLength(100)]
    [DisplayName("Category Name")]
    public string Name{set;get;}

    [Required(ErrorMessage = "Display Order is required")]
    [DisplayName("Display Order")]
    [Range(1,100,ErrorMessage="Display Order should be in [1-100]")]
    public int DisplayOrder{set;get;}


    }
}