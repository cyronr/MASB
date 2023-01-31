using System.Text.Json;

namespace MABS.Application.Common.Pagination
{
    public class PagingMetadata
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }

        public PagingMetadata(int currentPage, int totalPages, int pageSize, int totalCount, bool hasPrevious, bool hasNext)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            PageSize = pageSize;
            TotalCount = totalCount;
            HasPrevious = hasPrevious;
            HasNext = hasNext;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
