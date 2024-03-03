
using DanielMarket.Models;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
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

            var documentsWithIds = GetDocumentsIds(response);
            return documentsWithIds;
        }
 
        public async Task<IEnumerable<T>> GetDocumentsByTermAsync(string indexName, string fieldName, string fieldValue)
        {
            var response = await _elasticClient.SearchAsync<T>(s => s
            .Index(indexName)
            .Size(1000)
            .Query(q => q
            .Term(t => t
            .Field(fieldName).Value(fieldValue.ToLower()).CaseInsensitive(true))));

            var documentsWithIds = GetDocumentsIds(response);
            return documentsWithIds;
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

            var documentsWithIds = GetDocumentsIds(response);
            return documentsWithIds;
        }

        public string GetRequestBody(ISearchResponse<T> response)
        {
            var debugInfo = response.DebugInformation;
            var requestBodyStartIndex = debugInfo.IndexOf("# Request:");
            var requestBodyEndIndex = debugInfo.IndexOf("# Response:");
            return debugInfo.Substring(requestBodyStartIndex, requestBodyEndIndex - requestBodyStartIndex).Trim();
        }
        public IEnumerable<T> GetDocumentsIds(ISearchResponse<T> response)
        {
            var documentsWithIds = response.Hits.Select(h =>
            {
                var sourceType = h.Source.GetType();
                var idProperty = sourceType.GetProperty("Id");
                if (idProperty != null)
                    idProperty.SetValue(h.Source, h.Id);
                return h.Source;
            });

            return documentsWithIds;
        }
    }
}
