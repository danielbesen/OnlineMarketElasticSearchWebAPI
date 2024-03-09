using Nest;

namespace DanielMarket.Services
{
    public interface IElasticSearchService<T> where T : class
    {
        string GetRequestBody(ISearchResponse<T> response);
        IEnumerable<T> GetDocumentsIds(ISearchResponse<T> response);
        Task<IEnumerable<T>> GetAllDocumentsAsync(string indexName);
        Task<IEnumerable<T>> GetDocumentsByTermAsync(string indexName, string fieldName, string fieldValue);
        Task<IEnumerable<T>> GetDocumentsByTermsAsync(string indexName, string fieldName, List<string> fieldValue);
        Task<IEnumerable<T>> GetDocumentsByIdsAsync(string indexName, List<string> fieldValue);
        Task<IEnumerable<T>> GetDocumentsGreaterThan(string indexName, string fieldName, string fieldValue);
        Task<IEnumerable<T>> GetDocumentsByPrefix(string indexName, string fieldName, string fieldValue);
        Task<IEnumerable<T>> GetDocumentsWithNotNullField(string indexName, string fieldName);
        Task<IEnumerable<T>> GetDocumentsWithNullField(string indexName, string fieldName);
        Task<IEnumerable<T>> GetDocumentsFullTextQuery(string indexName, string fieldName, string fieldValue);
        Task<IEnumerable<T>> GetDocumentsMultiFieldFullTextQuery(string indexName, string fieldName, string[] fieldValue);
    }
}
