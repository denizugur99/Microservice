using Microservice.Order.Application.Contracts.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Persistence.UnitOfWork
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
             await context.Database.BeginTransactionAsync( cancellationToken = default);
        }

        public Task CommitTranscationAsync(CancellationToken cancellationToken=default)
        {
            return context.Database.CommitTransactionAsync(cancellationToken);
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken=default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }

      
    }
}
