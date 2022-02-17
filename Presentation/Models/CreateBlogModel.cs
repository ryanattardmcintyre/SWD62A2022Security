using Application.ViewModels;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class CreateBlogModel
    {
        public IQueryable<Category> Categories { get; set; }
        public AddBlogViewModel Blog { get; set; }
    }
}
