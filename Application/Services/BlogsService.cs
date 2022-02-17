using Application.Interfaces;
using Application.ViewModels;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Services
{
    public class BlogsService : IBlogsService
    {
        private IBlogsRepository blogsRepo;
        
        //making reference to the interface makes the code more efficient and flexible
        public BlogsService(IBlogsRepository _blogsRepo) 
        {
            blogsRepo = _blogsRepo;
        }

        public void AddBlog(AddBlogViewModel model)
        {
            blogsRepo.AddBlog(new Domain.Models.Blog()
            {
                CategoryId = model.CategoryId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                LogoImagePath = model.LogoImagePath,
                Name = model.Name
            });
        }

        public void DeleteBlog(int id)
        {
            var blog = blogsRepo.GetBlog(id);
            if (blog != null)
                blogsRepo.DeleteBlog(blog);
            else
                throw new Exception("Blog doesnt exist");
        }

        public BlogViewModel GetBlog(int id)
        { //all this code will be merged into 1 line
            //when we introduce AutoMapper
            var b = blogsRepo.GetBlog(id);
            BlogViewModel result = new BlogViewModel()
            {
                Category = b.Category,
                DateUpdated = b.DateUpdated,
                Id = b.Id,
                LogoImagePath = b.LogoImagePath,
                Name = b.Name
            };

            return result;
        }

        public IQueryable<BlogViewModel> GetBlogs()
        {
            //all this code will be merged into 1 line
            //when we introduce AutoMapper
            var list = from b in blogsRepo.GetBlogs()
                       orderby b.DateUpdated descending
                       select new BlogViewModel()
                       {
                           Category = b.Category,
                           DateUpdated = b.DateUpdated,
                           Id = b.Id,
                           LogoImagePath = b.LogoImagePath,
                           Name = b.Name
                       };
            return list;
        }
    }
}
