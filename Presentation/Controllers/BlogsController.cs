using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.ViewModels;
using DataAccess.Repositories;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    //Application Service: e.g. IBlogsService, ICategoriesService
    //Framework Service: e.g. IWebHostEnvironment
    [Authorize]
    public class BlogsController : Controller
    {
        
        private IBlogsService blogsService;
        private ICategoriesService categoriesService;
        private IWebHostEnvironment webHostEnvironment;
        public BlogsController(IBlogsService _blogsService, ICategoriesService _categoriesService, IWebHostEnvironment _webHostEnvironment)
        {
            
            webHostEnvironment = _webHostEnvironment;
            blogsService = _blogsService;
            categoriesService = _categoriesService;
        }
         
        public IActionResult Index()
        {
            
            var list = blogsService.GetBlogs();
            return View(list);
        }

        //View >> Controller >> Service >> Repository >> database
        //View << Controller << Service << Repository << database
        public IActionResult Details(int id)
        {
            var blog = blogsService.GetBlog(id);
            return View(blog);
        }

        //called before the Add Page is loaded/rendered
        [HttpGet] 
        public IActionResult Create()
        {
            var categories = categoriesService.GetCategories();
            ViewBag.Categories = categories;
            //CreateBlogModel myModel = new CreateBlogModel() { Categories = categories };
           
            return View();
        }

        [HttpPost]
       
        public IActionResult Create(AddBlogViewModel model, IFormFile logo)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                var categories = categoriesService.GetCategories();
                ViewBag.Categories = categories;

                ViewBag.Error = "Name should not be left empty";
                return View();
            }
            else
            {
                //start uploading the file
                if (logo != null)
                {
                    //1. to give the file a unique name
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(logo.FileName);

                    //2. to read the absolute path where we are going to save the file
                    string absolutePath =  webHostEnvironment.WebRootPath+ "\\files\\"+ fileName;

                    //3. we save the physical file on the web server
                    using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write ))
                    {
                        logo.CopyTo(fs);
                        fs.Close(); //flushes the data into the recipient file
                    }
                    model.LogoImagePath = @"\files\" + fileName;
                }
                blogsService.AddBlog(model);
                ViewBag.Message = "Blog saved successfully";
                
            }

            return RedirectToAction("Create");
        }
    }
}
