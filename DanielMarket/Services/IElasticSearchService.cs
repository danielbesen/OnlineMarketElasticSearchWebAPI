using DanielMarket.Models;
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
        Task<IEnumerable<T>> GetDocumentsGreaterThanAsync(string indexName, string fieldName, string fieldValue);
        Task<IEnumerable<T>> GetDocumentsByPrefixAsync(string indexName, string fieldName, string fieldValue);
        Task<IEnumerable<T>> GetDocumentsWithNotNullFieldAsync(string indexName, string fieldName);
        Task<IEnumerable<T>> GetDocumentsWithNullFieldAsync(string indexName, string fieldName);
        Task<IEnumerable<T>> GetDocumentsFullTextQueryAsync(string indexName, string fieldName, string fieldValue);
        Task<IEnumerable<T>> GetDocumentsMultiFieldFullTextQueryAsync(string indexName, string fieldName, string[] fieldValue);
        Task<IEnumerable<T>> GetDocumentsByPhraseAsync(string indexName, string fieldName, string fieldValue);
        Task<IEnumerable<T>> GetDocumentsBooleanLogicExampleAsync(string indexName);
        Task<AggsValues> GetStatsExplicityAsync(string indexName, string fieldName);
        Task<AggsValues> GetHowManyDifferentValuesAsync(string indexName, string fieldName);
        Task<AggsValues> GetStatsImplicitAsync(string indexName, string fieldName);
        Task<Dictionary<string, long>> GetDocsCountByDiferentStatusAsync(string indexName, string fieldName);
    }
}
