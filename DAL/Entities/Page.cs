using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Page
    {
        public Page()
        {
            Images = new List<Image>();
            CssFiles = new List<CssFile>();
        }

        public int Id { get; set; }

        [StringLength(450)]
        [Index("UrlIndex")]
        public string Url { get; set; }

        public decimal Ping { get; set; }
        public double Size { get; set; }
        public int SiteId { get; set; }
        public string ParentUrl { get; set; }
        public bool IsExternal { get; set; }
        public int Depth { get; set; }

        [ForeignKey("SiteId")]
        public Site Site { get; set; }

        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<CssFile> CssFiles { get; set; }
    }
}