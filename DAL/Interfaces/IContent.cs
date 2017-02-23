using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IContent
    {
        int Id { get; set; }
        string SourseLink { get; set; }
        int PageId { get; set; }
    }
}
