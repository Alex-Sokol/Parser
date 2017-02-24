namespace DAL.Interfaces
{
    public interface IContent
    {
        int Id { get; set; }
        string SourseLink { get; set; }
        int PageId { get; set; }
    }
}