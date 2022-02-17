using DataAccess.Context;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Implemented this version in such a way that it connects and reads/writes data to a db
    /// </summary>

    public class BlogsRepository : IBlogsRepository
    {
        private BloggingContext context;
        public BlogsRepository(BloggingContext _context)
        {
            context = _context;
        }

        public void AddBlog(Blog b)
        {
            context.Blogs.Add(b);
            context.SaveChanges();
        }

        public void DeleteBlog(Blog b)
        {
            context.Blogs.Remove(b);
            context.SaveChanges();
        }

        public Blog GetBlog(int id)
        {
            //approach 1:
            return context.Blogs.SingleOrDefault(x => x.Id == id); //lambda expression
/*
            //approach 2:
            var list = from b in context.Blogs
                       where b.Id==id
                       select b;
            return list.FirstOrDefault() ;

            //appraoch 3:
            foreach (Blog b in context.Blogs)
                if (b.Id == id)
                    return b;
*/
        }

        public IQueryable<Blog> GetBlogs()
        {
            return context.Blogs;
        }

        public void UpdateBlog(Blog b)
        {
            Blog originalBlog = GetBlog(b.Id); 
            originalBlog.DateUpdated = DateTime.Now;
            originalBlog.CategoryId = b.CategoryId;
            originalBlog.LogoImagePath = b.LogoImagePath;
            originalBlog.Name = b.Name;

            context.SaveChanges();
        }
    }
}
