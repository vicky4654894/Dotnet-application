using System.Collections.Generic;
namespace Application_1.Models.Models
{
public class ProductViewModel
{
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public Product Product { get; set; } = new Product();

    
}
}