using Crack.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Crack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext appDbContext;

        public FileUploadController(IWebHostEnvironment webHostEnvironment, AppDbContext appDbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            this.appDbContext = appDbContext;
        }

        [HttpGet("[action]")]

        public IActionResult GetAllThePost()
        {
            var files = appDbContext.Posts.ToList();
            return Ok(files);
        }

        [HttpGet]
        [Route("FindByName")]

        public IActionResult Find(string Name)
        {
            Post? post = appDbContext.Posts.FirstOrDefault(x => x.Title.Contains(Name));

            if (post == null)
            {
                return NotFound();
            }

            else
            {
                return Ok(post);
            }
        }



        [HttpPost("[action]")]


        public IActionResult CreatePost(Post post)
        {
             appDbContext.Posts.Add(post);
             appDbContext.SaveChanges();
            return Ok(post);
        }

        [HttpPost("[action]")]
        public IActionResult UploadData(List<IFormFile> files)
        {
           

            string directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "UploadedFiles");

            foreach (var file in files)
            {
                string filePath = Path.Combine(directoryPath, file.Name);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

           

            return Ok(files);
        }

        [HttpPut]
        [Route("Update")]

        public IActionResult Update(int id, Post post)
        {
            Post p = appDbContext.Posts.FirstOrDefault(x => x.Id == id);

            if (p == null)
            {
                return NotFound();
            }
            else
            {
                post.Title = p.Title;
                post.Category = p.Category;

                appDbContext.Posts.Update(post);
                appDbContext.SaveChanges();
                return Ok(post);
            }
        }

        [HttpDelete]
        [Route("Delete")]

        public IActionResult Delete(int id)
        {
            Post? post = appDbContext.Posts.FirstOrDefault(x => x.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            appDbContext.Posts.Remove(post);
            appDbContext.SaveChanges();
            return Ok();

        }



    }
}
