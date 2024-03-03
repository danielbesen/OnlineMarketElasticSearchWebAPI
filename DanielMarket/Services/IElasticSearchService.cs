namespace DanielMarket.Services
{
    public interface IElasticSearchService<T>
    {
        Task<IEnumerable<T>> GetAllDocumentsAsync(string indexName);
        Task<IEnumerable<T>> GetDocumentsByTermAsync(string indexName, string fieldName, string fieldValue);
        Task<IEnumerable<T>> GetDocumentsByTermsAsync(string indexName, string fieldName, List<string> fieldValue);
        
    }
}
