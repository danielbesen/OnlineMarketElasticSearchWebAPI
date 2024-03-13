using Nest;

namespace DanielMarket.Models
{
    [ElasticsearchType(RelationName = "salesman")]
    public class Salesman
    {
        [Number(NumberType.Integer, Name = "id")]
        public int Id { get; set; }

        [Text(Name = "name")]
        public string Name { get; set; }
    }
}
