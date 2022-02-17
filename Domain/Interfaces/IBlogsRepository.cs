using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Interfaces
{
    //In a SOA we define contracts
    //Contracts(Interfaces) are made up of declaration of methods one must implement if that person
    //would like to use that contract
    public interface IBlogsRepository
    {
        public IQueryable<Blog> GetBlogs();
        public Blog GetBlog(int id);
        public void AddBlog(Blog b);
        public void DeleteBlog(Blog b);
        public void UpdateBlog(Blog b);

       
    }
}
