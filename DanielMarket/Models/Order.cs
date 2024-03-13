using Nest;

namespace DanielMarket.Models
{
    [ElasticsearchType(RelationName = "orders")]
    public class Order
    {
        [Keyword(Name = "_id")]
        public string Id { get; set; }

        [Keyword(Name = "sales_chanel")]
        public string SalesChanel { get; set; }

        [Keyword(Name = "status")]
        public string Status { get; set; }

        [Date(Name = "purchased_at")]
        public string PurchasedAt { get; set; }

        [Number(NumberType.Double, Name = "total_amount")]
        public double TotalPrice { get; set; }

        [Nested]
        [PropertyName("salesman")]
        public Salesman Salesman { get; set; }

        [Nested]
        [PropertyName("lines")]
        public IEnumerable<Line> Lines { get; set; }
    }
}
