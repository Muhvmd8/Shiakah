namespace Shared.DTOs;

public class PaginatedResultDto<T>(List<T> result, int totalCount, int pageNumber, int pageSize)
{
    public IEnumerable<T> Data { get; set; } = result;
    public int TotalCount { get; set; } = totalCount;
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
}
