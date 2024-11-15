namespace VBDQ_API.Conmon
{
    public class GridQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; } = "asc"; // desc                                                 
        public string? Filter { get; set; }
    }
}
