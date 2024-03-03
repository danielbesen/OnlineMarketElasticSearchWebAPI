
using DanielMarket.Models;
using Elasticsearch.Net;
using Nest;
using System.Xml.Linq;

namespace DanielMarket.Services
{
    public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
    {
        private readonly ElasticClient _elasticClient;

        public ElasticSearchService(ElasticClient elasticClient)
        {
            this._elasticClient = elasticClient;
        }

        public async Task<IEnumerable<T>> GetAllDocumentsAsync(string indexName)
        {
            var response = await _elasticClient.SearchAsync<T>(s => s
                            .Index(indexName)
                            .Query(q => q.MatchAll())
                            .Size(1000));

            return response.Documents;
        }
 
        public async Task<IEnumerable<T>> GetDocumentsByTermAsync(string indexName, string fieldName, string fieldValue)
        {
            var response = await _elasticClient.SearchAsync<T>(s => s
            .Index(indexName)
            .Size(1000)
            .Query(q => q
            .Term(t => t
            .Field(fieldName).Value(fieldValue.ToLower()).CaseInsensitive(true))));


            return response.Documents;
        }

        public async Task<IEnumerable<T>> GetDocumentsByTermsAsync(string indexName, string fieldName, List<string> fieldValue)
        {
            var response = await _elasticClient.SearchAsync<T>(s => s
            .Index(indexName)
            .Size(1000)
            .Query(q => q
            .Terms(t => t
            .Field(fieldName)
            .Terms(fieldValue))));

            return response.Documents;
        }
    }
}
