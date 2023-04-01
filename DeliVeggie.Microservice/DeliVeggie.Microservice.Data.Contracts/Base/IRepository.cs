using System;

namespace DeliVeggie.Microservice.Data.Contracts.Repository.Base
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {

    }
}
