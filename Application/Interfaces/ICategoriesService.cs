using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Interfaces
{
    public interface ICategoriesService
    {
        public IQueryable<Category> GetCategories();
    }
}
