using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public interface SlideService
    {
        public IQueryable<Slide> GetAllSlides();
        public IQueryable<Slide> GetAllSlidesWithFilter(string sortOrder, string status);
        public bool Create(Slide slide);
        public bool Update(Slide slide);
        public bool Remove(Slide slide);
        public int CountTotal();
        public Slide FindById(int id);
    }
}
