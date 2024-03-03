using Nest;

namespace DanielMarket.Models
{
    [ElasticsearchType(RelationName = "products")]
    public class Product
    {
        [Keyword(Name ="_id")]
        public string Id { get; set; }

        [Text(Name = "name")]
        public string Name { get; set; }

        [Keyword(Name = "name.keyword")]
        public string NameKeyword { get; set; }

        [Text(Name = "description")]
        public string Description { get; set; }

        [Keyword(Name = "description.keyword")]
        public string DescriptionKeyword { get; set; }

        [Number(NumberType.Long, Name = "price")]
        public float Price { get; set; }

        [Number(NumberType.Integer, Name = "in_stock")]
        public int Stock { get; set; }

        [Number(NumberType.Long, Name = "sold")]
        public int Sold { get; set; }

        [Text(Name = "tags")]
        public string[] Tags { get; set; }

        [Text(Name = "tags.keyword")]
        public string[] TagsKeyword { get; set; }

        [Boolean(Name = "is_active")]
        public bool IsActive { get; set; }

        [Date(Name = "created")]
        public string CreatedAt { get; set; }

        public Product() { }
    }
}
