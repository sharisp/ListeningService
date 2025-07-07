namespace Listening.Infrastructure.Options
{
    public class PaginationResponse<T>
    {
        public int TotalCount { get; set; }
        public List<T> DataList { get; set; } = new List<T>();
        public PaginationResponse(int totalCount, List<T> dataList)
        {
            TotalCount = totalCount;
            DataList = dataList ?? new List<T>();
        }
    }
}
