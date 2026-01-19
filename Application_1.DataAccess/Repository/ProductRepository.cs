using System;
using Application_1.Models.Models;
using Microsoft.EntityFrameworkCore;
using Application_1.DataAccess.Data;
using Application_1.DataAccess.Repository.IRepository;



namespace Application_1.DataAccess.Repository
{
public class ProductRepository:Repository<Product>, IProductRepository
{

    private readonly ApplicationDbContext _db;
    public ProductRepository(ApplicationDbContext db):base(db)
    {
        _db = db;
    }


    public void Update(Product product)
    {
            var productFromDb = _db.Products.FirstOrDefault(u => u.Id == product.Id);
            if(productFromDb == null)
            {
                return;
            }
            if (productFromDb != null)
            {
                productFromDb.Title = product.Title;
                productFromDb.Description = product.Description;
                productFromDb.ISBN = product.ISBN;
                productFromDb.Author = product.Author;
                productFromDb.ListPrice = product.ListPrice;
                productFromDb.Price50 = product.Price50;
                productFromDb.Price100 = product.Price100;
            }
            _db.Products.Update(productFromDb);
            _db.SaveChanges();

    }
    
}
}