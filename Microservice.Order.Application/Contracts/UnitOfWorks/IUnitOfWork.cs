using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Contracts.UnitOfWorks
{
   public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken=default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTranscationAsync(CancellationToken cancellationToken=default);
    }
}
