using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public interface PageService
    {
        public IQueryable<Page> GetAllPages();
        public IQueryable<Page> GetAllPagesWithSearch(string keyword);
        public IQueryable<Page> GetAllPagesWithFilter(string keyword, string sortOrder, string status);
        public bool Create(Page page);
        public bool Update(Page page);
        public bool Remove(Page page);
        public int CountTotal();
        public int CountByStatus(bool status);
        public Page FindById(int id);
    }
}
