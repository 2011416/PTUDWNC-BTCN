using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class BestAuthorsWidget : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public BestAuthorsWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var author = await _blogRepository.GetAuthorsAsync(4);

            return View(author);
        }
    }
}
