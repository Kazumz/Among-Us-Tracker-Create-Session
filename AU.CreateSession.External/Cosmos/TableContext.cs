using System.Collections.Generic;
using System.Threading.Tasks;
using AU.CreateSession.Services.External.Cosmos;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;

namespace AU.CreateSession.External.Cosmos
{
    public class TableContext : ITableContext
    {
        private readonly ILogger<TableContext> logger;

        public TableContext(ILogger<TableContext> logger)
        {
            this.logger = logger;
        }

        public async Task<T> InsertOrUpdate<T>(CloudTable cloudTable, T entity) where T : ITableEntity
        {
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

            TableResult result = await cloudTable.ExecuteAsync(insertOrMergeOperation);
            T insertedEntity = (T)result.Result;

            if (result.RequestCharge.HasValue)
            {
                logger.LogInformation($"Request Charge of InsertOrMerge Operation: {result.RequestCharge}");
            }

            return insertedEntity;
        }

        public async Task<T> Retrieve<T>(CloudTable cloudTable, string partitionKey, string rowKey) where T : ITableEntity
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult result = await cloudTable.ExecuteAsync(retrieveOperation);
            T retrievedEntity = (T)result.Result;

            if (result.RequestCharge.HasValue)
            {
                logger.LogInformation($"Request Charge of Retrieve Operation: {result.RequestCharge}");
            }

            return retrievedEntity;
        }

        public async Task Delete<T>(CloudTable cloudTable, T entity) where T : ITableEntity
        {
            TableOperation deleteOperation = TableOperation.Delete(entity);
            TableResult result = await cloudTable.ExecuteAsync(deleteOperation);

            if (result.RequestCharge.HasValue)
            {
                logger.LogInformation($"Request Charge of Retrieve Operation: {result.RequestCharge}");
            }
        }

        public async Task<IEnumerable<T>> RetrieveByPartitionKey<T>(CloudTable cloudTable, string partitionKey) where T : ITableEntity, new()
        {
            List<T> collection = new List<T>();

            TableQuery<T> partitionScanQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<T> segment = await cloudTable.ExecuteQuerySegmentedAsync(partitionScanQuery, token);
                token = segment.ContinuationToken;

                if (segment.RequestCharge.HasValue)
                {
                    logger.LogInformation($"Request Charge of RetrieveByPartitionKey Operation: {segment.RequestCharge}");
                }

                collection.AddRange(segment.Results);
            }
            while (token != null);

            return collection;
        }
    }
}
