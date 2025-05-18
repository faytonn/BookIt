namespace BookIt.Application.DTOs.Common;

public class PaginateDTO<T> where T : IDTO
{
    public List<T> Items { get; set; } = [];

    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }

}
