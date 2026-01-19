using System;
using Application_1.Models.Models;
using Microsoft.EntityFrameworkCore;
using Application_1.DataAccess.Data;
using Application_1.DataAccess.Repository.IRepository;



namespace Application_1.DataAccess.Repository
{
public class CategoryRepository:Repository<Category>, ICategoryRepository
{

    private readonly ApplicationDbContext _db;
    public CategoryRepository(ApplicationDbContext db):base(db)
    {
        _db = db;
    }


    public void Update(Category category)
    {
            var categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == category.Id);
            if (categoryFromDb != null)
            {
                categoryFromDb.Name = category.Name;
                categoryFromDb.DisplayOrder = category.DisplayOrder;
            }

            _db.Categories.Update(categoryFromDb);
            _db.SaveChanges();

    }
    
}
}