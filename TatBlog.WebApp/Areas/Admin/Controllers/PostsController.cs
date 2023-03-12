using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController :Controller
    {

        private readonly IBlogRepository _blogRepository;

        public PostsController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public IActionResult Index()
        {
            return View(); 
        }

        private async Task PopulatePostFilterModelAsync(PostFilterModel model)
        {
            var authors = await _blogRepository.GetAuthorsAsync();
            var categories = await _blogRepository.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            });

            model.CategoryList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }
    }
}
