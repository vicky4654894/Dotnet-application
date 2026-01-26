using Application_1.Models.Models;
using Microsoft.EntityFrameworkCore;
namespace Application_1.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {

        void Update(Category category);
    }
}