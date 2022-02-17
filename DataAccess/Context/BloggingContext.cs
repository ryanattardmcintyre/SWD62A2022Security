using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccess.Context
{
    //this is the class which is the gateway to our database
    //this inherits from a built-in class called xxxDbContext
    //the dbcontext class contains methods which will allow us to
    //amongst other things to connect with the database, add an item to the database
    //delete, update, querying using LINQ....
    public class BloggingContext : IdentityDbContext<CustomUser>
    {
        public BloggingContext(DbContextOptions<BloggingContext> options):
            base(options)
        { }

        public DbSet<Blog> Blogs { get; set; }  //DbSet will eventually be converted into a table
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
