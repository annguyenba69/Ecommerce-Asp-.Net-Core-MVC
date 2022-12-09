using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public interface PostCatService
    {
        public IQueryable<PostCat> GetAllPostCats();
        public IQueryable<PostCat> GetAllPostCatsWithSearch(string keyword);
        public PostCat FindById(int id);
        public bool Create(PostCat postCat);
        public bool Update(PostCat postCat);
        public bool Remove(PostCat postCat);
    }
}
