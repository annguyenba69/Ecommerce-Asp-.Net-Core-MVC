using Ecommerce.DataAccess.Data;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public class SlideServiceImpl : SlideService
    {
        private readonly ApplicationDbContext _db;
        public SlideServiceImpl(ApplicationDbContext db)
        {
            _db = db;
        }
        public int CountTotal()
        {
            return _db.Slides.Count();
        }

        public bool Create(Slide slide)
        {
            try
            {
                _db.Slides.Add(slide);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public Slide FindById(int id)
        {
            return _db.Slides.Include(s => s.ApplicationUser).FirstOrDefault(s => s.Id == id);
        }

        public IQueryable<Slide> GetAllSlides()
        {
            return _db.Slides.Include(s => s.ApplicationUser);
        }

        public IQueryable<Slide> GetAllSlidesWithFilter(string sortOrder, string status)
        {
            IQueryable<Slide> slides = GetAllSlides();
            switch (status)
            {
                case "active":
                    slides = slides.Where(s => s.Status);
                    break;
                case "inactive":
                    slides = slides.Where(s => !s.Status);
                    break;
                default:
                    slides = slides;
                    break;
            }
            if (sortOrder == "Date")
            {
                slides = slides.OrderBy(s => s.Created);
            }
            else
            {
                slides = slides.OrderByDescending(s => s.Created);
            }
            return slides;
        }


        public bool Remove(Slide slide)
        {
            try
            {
                _db.Slides.Remove(slide);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Slide slide)
        {
            try
            {
                _db.Slides.Update(slide);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
