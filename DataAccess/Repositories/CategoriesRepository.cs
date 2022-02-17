using DataAccess.Context;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class CategoriesRepository:ICategoriesRepository
    {
        private BloggingContext context;
        public CategoriesRepository(BloggingContext _context)
        {
            context = _context;
        }

        public IQueryable<Category> GetCategories()
        {
            return context.Categories;
        }
    }
}
