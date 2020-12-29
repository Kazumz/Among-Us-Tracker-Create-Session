using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace AU.CreateSession.Services.External.Cosmos
{
    public interface ITableContext
    {
        Task<T> InsertOrUpdate<T>(CloudTable cloudTable, T entity) where T : ITableEntity;

        Task<T> Retrieve<T>(CloudTable cloudTable, string partitionKey, string rowKey) where T : ITableEntity;

        Task<IEnumerable<T>> RetrieveByPartitionKey<T>(CloudTable cloudTable, string partitionKey) where T : ITableEntity, new();

        Task Delete<T>(CloudTable cloudTable, T entity) where T : ITableEntity;
    }
}
