using Nest;

namespace DanielMarket.Models
{
    [ElasticsearchType(RelationName = "lines")]
    public class Line
    {
        [Number(NumberType.Integer, Name = "product_id")]
        public int ProductId { get; set; }

        [Number(NumberType.Short, Name = "quantity")]
        public short Quantity { get; set; }

        [Number(NumberType.Double, Name = "amount")]
        public double Price { get; set; }
    }
}
