using Nest;

namespace DanielMarket.Models
{
    [ElasticsearchType(RelationName = "department")]
    public class Department
    {
        [Keyword(Name = "_id")]
        public string Id { get; set; }

        [Text(Name = "name")]
        public string Name { get; set; }

        [Text(Name = "name.keyword")]
        public string NameKeyword { get; set; }

        [Nested]
        [PropertyName("employees")]
        public Employee Employees { get; set; }
    }
}
