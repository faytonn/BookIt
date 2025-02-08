namespace BookIt.Application.DTOs.Common;

public class PaginateDTO<T> where T : IDTO
{
    public List<T> Items { get; set; } = new();

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public int TotalItems { get; set; }

    public int TotalPages
    {
        get
        {
            return PageSize == 0
                ? 0
                : (int)Math.Ceiling((double)TotalItems / PageSize);
        }
    }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

}
