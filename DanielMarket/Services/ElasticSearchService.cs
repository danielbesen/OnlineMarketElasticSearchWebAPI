﻿
using DanielMarket.Models;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
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
        public string GetRequestBody(ISearchResponse<T> response)
        {
            try
            {
                string requestJson = Encoding.UTF8.GetString(response?.ApiCall?.RequestBodyInBytes);
                return requestJson;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
        public IEnumerable<T> GetDocumentsIds(ISearchResponse<T> response)
        {
            try
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
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }

        }
        public async Task<IEnumerable<T>> GetAllDocumentsAsync(string indexName)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Query(q => q.MatchAll())
                .Size(1000));

                var documentsWithIds = GetDocumentsIds(response);
                return documentsWithIds;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }

        }
        public async Task<IEnumerable<T>> GetDocumentsByTermAsync(string indexName, string fieldName, string fieldValue)
        {
            try
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
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }

        }
        public async Task<IEnumerable<T>> GetDocumentsByTermsAsync(string indexName, string fieldName, List<string> fieldValue)
        {
            try
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
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }

        }
        public async Task<IEnumerable<T>> GetDocumentsByIdsAsync(string indexName, List<string> fieldValue)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(1000)
                .Query(q => q
                .Ids(i => i
                .Values(fieldValue)))
                .Sort(ss => ss
                .Descending("_score")));

                var documentsWithIds = GetDocumentsIds(response);
                return documentsWithIds;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
        public async Task<IEnumerable<T>> GetDocumentsGreaterThanAsync(string indexName, string fieldName, string fieldValue)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(1000)
                .Query(q => q
                .Range(r => r
                .Field(fieldName)
                .GreaterThan(Convert.ToInt32(fieldValue)))));

                var documentsWithIds = GetDocumentsIds(response);
                return documentsWithIds;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }


        }
        public async Task<IEnumerable<T>> GetDocumentsByPrefixAsync(string indexName, string fieldName, string fieldValue)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(1000)
                .Query(q => q
                .Prefix(p => p
                .Field(fieldName).Value(fieldValue))));

                var DocumentsWithIds = GetDocumentsIds(response);

                return DocumentsWithIds;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }

        }
        public async Task<IEnumerable<T>> GetDocumentsWithNotNullFieldAsync(string indexName, string fieldName)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(1000)
                .Query(q => q
                .Exists(e => e
                .Field(fieldName))));

                var documentsWithIds = GetDocumentsIds(response);
                return documentsWithIds;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
        public async Task<IEnumerable<T>> GetDocumentsWithNullFieldAsync(string indexName, string fieldName)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(1000)
                .Query(q => q
                .Bool(b => b
                .MustNot(mn => mn
                .Exists(e => e
                .Field(fieldName))))));

                var documentsWithIds = GetDocumentsIds(response);
                return documentsWithIds;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
        public async Task<IEnumerable<T>> GetDocumentsFullTextQueryAsync(string indexName, string fieldName, string fieldValue)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(1000)
                .Query(q => q
                .Match(m => m
                .Field(fieldName)
                .Query(fieldValue)
                .Operator(Operator.Or))));

                var documentsWithIds = GetDocumentsIds(response);
                return documentsWithIds;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
        public async Task<IEnumerable<T>> GetDocumentsMultiFieldFullTextQueryAsync(string indexName, string fieldName, string[] fieldValue)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(1000)
                .Query(q => q
                .MultiMatch(mm => mm
                .Query(fieldName).Fields(fieldValue))));

                var test = GetRequestBody(response);

                var documentsWithIds = GetDocumentsIds(response);
                return documentsWithIds;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }

        }
        public async Task<IEnumerable<T>> GetDocumentsByPhraseAsync(string indexName, string fieldName, string fieldValue)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(1000)
                .Query(q => q
                .MatchPhrase(mp => mp
                .Field(fieldName).Query(fieldValue))));

                var documentsWithIds = GetDocumentsIds(response);
                return documentsWithIds;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
        public async Task<IEnumerable<T>> GetDocumentsBooleanLogicExampleAsync(string indexName)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(1000)
                .Query(q => q
                .Bool(b => b
                .Must(m => m
                .Term(t => t
                .Field("tags.keyword").Value("Alcohol")))
                .MustNot(mn => mn
                .Term(tt => tt
                .Field("tags.keyword").Value("Wine")))
                .Should(sh => sh
                .Term(te => te
                .Field("tags.keyword").Value("Beer")),
                sh => sh
                .Match(ma => ma
                .Field("name").Query("beer")),
                sh => sh
                .Match(ma => ma
                .Field("description").Query("beer")))
                .MinimumShouldMatch(1))));

                var queryBody = GetRequestBody(response);

                var documentsWithIds = GetDocumentsIds(response);
                return documentsWithIds;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
    }
}
