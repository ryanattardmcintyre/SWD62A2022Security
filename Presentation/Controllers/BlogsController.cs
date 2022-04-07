using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.ViewModels;
using DataAccess.Repositories;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.Models;
using Presentation.Utilities;

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
        private ILogger<BlogsController> logger;
        public BlogsController(IBlogsService _blogsService, ICategoriesService _categoriesService, IWebHostEnvironment _webHostEnvironment
            , ILogger<BlogsController> _logger)
        {
            logger = _logger;
            webHostEnvironment = _webHostEnvironment;
            blogsService = _blogsService;
            categoriesService = _categoriesService;
        }
         
        public IActionResult Index()
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
           
            logger.LogInformation($"User {User.Identity.Name} with ip {remoteIpAddress} accessed Blogs\\Index method");
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
        [ValidateAntiForgeryToken()]
        public IActionResult Create(AddBlogViewModel model, IFormFile logo)
        {
            try
            {
                logger.LogInformation($"User { User.Identity.Name} accessed the Create Blog method");
                var categories = categoriesService.GetCategories();
                ViewBag.Categories = categories;

                if (model.CategoryId < categories.ToList().OrderBy(x => x.Id).ElementAt(0).Id
                    ||
                    model.CategoryId > categories.ToList().OrderByDescending(x => x.Id).ElementAt(0).Id
                    )
                {
                    logger.LogWarning($"User { User.Identity.Name}  did not input a valid category");
                    ModelState.AddModelError("", "Category selected is not valid");
                    return View();
                }
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.Name))
                    {
                        logger.LogWarning($"User { User.Identity.Name}  did not input a blog name");

                        ViewBag.Error = "Name should not be left empty";
                        return View();
                    }
                    else
                    {
                        if (logo.Length > 10000)
                        {
                            logger.LogWarning($"User { User.Identity.Name}  uploaded a file greater than 4mb");
                            throw new ArgumentException($"File size is too big, User file size was: {logo.Length}");
                           // return RedirectToAction("Error", "Home", new { message = "Files over 4MB are not accepted." });
                        }


                        byte[] dictionary = new byte[] { 255, 216 }; //represents a jpg
                        //checking file type
                        using (Stream myFileForCheckingType = logo.OpenReadStream())
                        {

                            byte[] toBeVerified = new byte[dictionary.Length];
                            myFileForCheckingType.Read(toBeVerified, 0, dictionary.Length);

                            for (int i = 0; i < dictionary.Length; i++)
                            {
                                //you need to compare dictionary[i] with toBeVerified[i]
                                //if you find that there is a mismatch  throw new ArgumentException($"File format is not acceptable");
                            }
                        }

                        Cryptographic myCryptographic = new Cryptographic();
                        myCryptographic.HybridEncryption(logo.OpenReadStream(), )

                        if (Path.GetExtension(logo.FileName) != ".jpg")
                        {
                            throw new ArgumentException($"File type is not accepted, User file type was: {Path.GetExtension(logo.FileName)}");
                        }


                        //start uploading the file
                        if (logo != null)
                        {
                            logger.LogWarning($"User { User.Identity.Name}  uploaded a file : {logo.FileName}");
                            try
                            { //1. to give the file a unique name
                                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(logo.FileName);
                                logger.LogCritical($"User { User.Identity.Name}  uploded a file and a new filename {fileName} is generated");
                                //2. to read the absolute path where we are going to save the file
                                string absolutePath = webHostEnvironment.WebRootPath + "\\files\\" + fileName;

                                //3. we save the physical file on the web server
                                using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                                {
                                    logger.LogCritical($"User { User.Identity.Name} - File is about to be saved on server");
                                    logo.CopyTo(fs);
                                    fs.Close(); //flushes the data into the recipient file
                                    logger.LogCritical($"User { User.Identity.Name} - File copied successfully on server");
                                }
                                model.LogoImagePath = @"\files\" + fileName;
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, $"User { User.Identity.Name} - File {logo.FileName} generated an error");
                                return RedirectToAction("Error", "Home", new { message = "Error while saving your file. we are working on it" });
                            }
                        }

                        string anEncodedName = HtmlEncoder.Default.Encode(model.Name);
                        model.Name = anEncodedName;
                        blogsService.AddBlog(model);
                        ViewBag.Message = "Blog saved successfully";

                    }
                }
                else
                {

                    ModelState.AddModelError("", "some of your inputs are not acceptable. check again");

                    return View();
                }
               
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"User { User.Identity.Name} - File {logo.FileName} generated an error");
                return RedirectToAction("Error", "Home", new { message = "An error occurred, we are working on it." });
            }
            return RedirectToAction("Create");
        }
    }
}
