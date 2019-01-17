using PearUp.CommonEntities;
using PearUp.IRepository;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PearUp.Repository
{
    public abstract class BaseRepository<TDomain> where TDomain : BusinessEntity.Entity
    {
        protected readonly PearUpContext _dbContext;

        public BaseRepository(PearUpContext context)
        {
            this._dbContext = context;
        }

        public virtual void Create(TDomain entity)
        {
            _dbContext.Set<TDomain>().Add(entity);
        }

        public virtual void Delete(TDomain entity)
        {
            Update(entity);
        }

        public virtual void Update(TDomain entity)
        {
            _dbContext.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        protected async Task<Result<TDomain>> Single(Expression<Func<TDomain, bool>> filter, string errorMessage)
        {
            var domain = await _dbContext.Set<TDomain>().Where(filter).FirstOrDefaultAsync();
            return ReturnResult(domain, errorMessage);
        }

        protected async Task<Result<TDomain>> Single<TProperty>(Expression<Func<TDomain, bool>> filter, Expression<Func<TDomain, TProperty>> includeProperty, string errorMessage)
        {
            var domain = await _dbContext.Set<TDomain>().Where(filter).Include(includeProperty).FirstOrDefaultAsync();
            return ReturnResult(domain, errorMessage);
        }

        private Result<TDomain> ReturnResult(TDomain domain, string errorMessage)
        {
            if (domain == null)
            {
                return Result.Fail<TDomain>(errorMessage);
            }
            return Result.Ok(domain);
        }

        public async Task<Result> SaveChangesAsync(string errorMessageOnFail)
        {
            var success = await _dbContext.SaveChangesAsync().ConfigureAwait(false) > 0;
            return success ? Result.Ok() : Result.Fail(errorMessageOnFail);
        }
    }
}
