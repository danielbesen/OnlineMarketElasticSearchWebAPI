using Nest;

namespace DanielMarket.Models
{
    [ElasticsearchType(RelationName = "employees")]
    public class Employee
    {
        [Number(NumberType.Long, Name = "age")]
        public long Age { get; set; }

        [Text(Name = "gender")]
        public string Gender { get; set; }

        [Text(Name = "gender.keyword")]
        public string GenderKeyword { get; set; }

        [Text(Name = "name")]
        public string Name { get; set; }

        [Text(Name = "name.keyword")]
        public string NameKeyword { get; set; }

        [Text(Name = "position")]
        public string Position { get; set; }

        [Text(Name = "position.keyword")]
        public string PositionKeyword { get; set; }
    }
}
