namespace DanielMarket.Models.Utils
{
    public class ResponseResult<T>
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Results { get; set; }

        public ResponseResult(IEnumerable<T> results)
        {
            Results = results;
            TotalCount = Results?.Count() ?? 0;
        }

    }
}
