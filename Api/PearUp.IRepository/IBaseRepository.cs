using PearUp.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PearUp.IRepository
{
    public interface IBaseRepository<D>
    {
        void Create(D entity);
        void Update(D entity);
        void Delete(D entity);
        Task<Result> SaveChangesAsync(string errorMessageOnFail);
    }
}
