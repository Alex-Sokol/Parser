using System.ComponentModel.DataAnnotations.Schema;
using DAL.Interfaces;

namespace DAL
{
    public class CssFile : IContent
    {
        public int Id { get; set; }
        public string SourseLink { get; set; }
        public int PageId { get; set; }

        [ForeignKey("PageId")]
        public virtual Page Page { get; set; }
    }
}