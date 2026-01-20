using System.Collections.Generic;

namespace Application_1.Models.Models
{
public class CategoryViewModel
{

    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    public Category Category { get; set; } = new Category();

    
}
}