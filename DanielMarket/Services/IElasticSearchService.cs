namespace DanielMarket.Services
{
    public interface IElasticSearchService<T>
    {
        Task<IEnumerable<T>> GetDocumentsByTermAsync(string indexName, string fieldName, string fieldValue);

        Task<IEnumerable<T>> GetAllDocumentsAsync(string indexName);
    }
}
