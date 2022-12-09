using Ecommerce.DataAccess.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public class PageServiceImpl : PageService
    {
        private readonly ApplicationDbContext _db;
        public PageServiceImpl(ApplicationDbContext db)
        {
            _db = db;
        }

        public int CountByStatus(bool status)
        {
            return _db.Pages.Where(p => p.Status== status).Count();
        }

        public int CountTotal()
        {
            return _db.Pages.Count();
        }

        public bool Create(Page page)
        {
            try
            {
                _db.Pages.Add(page);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public Page FindById(int id)
        {
            return _db.Pages.Include(p => p.ApplicationUser).FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Page> GetAllPages()
        {
            return _db.Pages.Include(p => p.ApplicationUser);
        }

        public IQueryable<Page> GetAllPagesWithFilter(string keyword, string sortOrder, string status)
        {
            IQueryable<Page> pages = GetAllPagesWithSearch(keyword);
            switch (status)
            {
                case "active":
                    pages = pages.Where(p => p.Status);
                    break;
                case "inactive":
                    pages = pages.Where(p => !p.Status);
                    break;
                default:
                    pages = pages;
                    break;
            }
            switch (sortOrder)
            {
                case "name_desc":
                    pages = pages.OrderByDescending(p => p.Name);
                    break;
                case "Date":
                    pages = pages.OrderBy(p => p.Created);
                    break;
                case "date_desc":
                    pages = pages.OrderByDescending(p => p.Created);
                    break;
                default:
                    pages = pages.OrderBy(p => p.Name);
                    break;
            }
            return pages;
        }

        public IQueryable<Page> GetAllPagesWithSearch(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return GetAllPages();
            }
            else
            {
                return GetAllPages().Where(p => p.Name.Contains(keyword));
            }
        }

        public bool Remove(Page page)
        {
            try
            {
                _db.Pages.Remove(page);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Page page)
        {
            try
            {
                _db.Pages.Update(page);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
