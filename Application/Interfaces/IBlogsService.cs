using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Interfaces
{
    public interface IBlogsService
    {
        public IQueryable<BlogViewModel> GetBlogs();
        public BlogViewModel GetBlog(int id);

        public void AddBlog(AddBlogViewModel model);

        public void DeleteBlog(int id);



    }
}
