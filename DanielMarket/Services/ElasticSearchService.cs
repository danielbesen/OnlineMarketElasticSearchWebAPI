
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
        public async Task<AggsValues> GetStatsExplicityAsync(string indexName, string fieldName)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(0)
                .Aggregations(aggs => aggs
                .Sum("total_sales", sum => sum.Field(fieldName))
                .Average("avg_sale", avg => avg.Field(fieldName))
                .Min("min_sale", min => min.Field(fieldName))
                .Max("max_sale", max => max.Field(fieldName))));

                AggsValues orderStats = new AggsValues()
                {
                    TotalSalesValue = response.Aggregations.Sum("total_sales").Value,
                    AverageSalePrice = response.Aggregations.Average("avg_sale").Value,
                    MinimumSalePrice = response.Aggregations.Min("min_sale").Value,
                    MaximumSalePrice = response.Aggregations.Max("max_sale").Value
                };

                return orderStats;
            }
            catch (Exception e)
            {
                throw new Exception($"Error : {e}");
            }
        }
        public async Task<AggsValues> GetHowManyDifferentValuesAsync(string indexName, string fieldName)
        {
            try
            {
                var query = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(0)
                .Aggregations(aggs => aggs
                .Cardinality("total_salesman", c => c.Field(fieldName))));

                AggsValues aggsValues = new AggsValues()
                {
                    Amount = query.Aggregations.Cardinality("total_salesman").Value
                };

                return aggsValues;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
        public async Task<AggsValues> GetStatsImplicitAsync(string indexName, string fieldName)
        {
            try
            {
                var response = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(0)
                .Aggregations(aggs => aggs
                .Stats("sales_stats", st => st.Field(fieldName))));

                AggsValues orderStats = new AggsValues()
                {
                    TotalSalesValue = response.Aggregations.Stats("sales_stats").Sum,
                    AverageSalePrice = response.Aggregations.Stats("sales_stats").Average,
                    MinimumSalePrice = response.Aggregations.Stats("sales_stats").Min,
                    MaximumSalePrice = response.Aggregations.Stats("sales_stats").Max
                };

                return orderStats;
            }
            catch (Exception e)
            {
                throw new Exception($"Error : {e}");
            }
        }
        public async Task<Dictionary<string, long>> GetDocsCountByDiferentStatusAsync(string indexName, string fieldName)
        {
            try
            {
                Dictionary<string, long> kvp = new Dictionary<string, long>();

                var query = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(0)
                .Aggregations(aggs => aggs
                .Terms("status_terms", t => t.Field(fieldName)
                                                  .Missing("N/A")
                                                  .MinimumDocumentCount(0)
                                                  .Order(o => o.Ascending("_key")))));

                var buckets = query.Aggregations.Terms("status_terms").Buckets;
                foreach (var bucket in buckets)
                {
                    kvp.Add(bucket.Key, (long)bucket.DocCount);
                }

                return kvp;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
        public async Task<Dictionary<string, long>> AggregationByRangeAsync(string indexName, string fieldName)
        {
            try
            {
                Dictionary<string, long> kvp = new Dictionary<string, long>();

                var query = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(0)
                .Aggregations(aggs => aggs
                .Range("amount_distribution", r => r
                .Field(fieldName)
                .Ranges(r => r.To(50), r => r.From(50).To(100), r => r.From(100)))));

                var buckets = query.Aggregations.Range("amount_distribution").Buckets;
                foreach (var bucket in buckets)
                {
                    kvp.Add(bucket.Key, bucket.DocCount);
                }

                return kvp;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
        public async Task<Dictionary<string, double>> GetNestedAggregationAsync(string indexName, string pathName, string fieldName)
        {
            try
            {
                Dictionary<string, double> kvp = new Dictionary<string, double>();

                var query = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(0)
                .Aggregations(aggs => aggs
                .Nested("age_employees", n => n.Path(pathName)
                .Aggregations(agg => agg
                .Min("minimum_age", m => m.Field(fieldName))))));

                var doc_count = query.Aggregations.Nested("age_employees").DocCount;
                var value = query.Aggregations.Nested("age_employees").Min("minimum_age").Value;

                kvp.Add("doc_count", doc_count);
                kvp.Add("minimum_age", (double)value);

                return kvp;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }

        public async Task<IEnumerable<T>> GetDocumentsHandlingTypoErrorsAsync(string indexName, string fieldName, string fieldValue)
        {
            try
            {
                var query = await _elasticClient.SearchAsync<T>(s => s
                .Index(indexName)
                .Size(1000)
                .Query(q => q
                .Match(m => m
                .Field(fieldName).Query(fieldValue).Fuzziness(Fuzziness.Auto))));

                var documentsWithIds = GetDocumentsIds(query);
                return documentsWithIds;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e}");
            }
        }
    }
}
