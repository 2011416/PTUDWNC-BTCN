using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class BestAuthorsWidget : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IAuthorRepository _authorRepository;

        public BestAuthorsWidget(IBlogRepository blogRepository, IAuthorRepository authorRepository)
        {
            _blogRepository = blogRepository;
            _authorRepository = authorRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var author = await _authorRepository.GetAuthorsAsync(4);

            return View(author);
        }
    }
}
