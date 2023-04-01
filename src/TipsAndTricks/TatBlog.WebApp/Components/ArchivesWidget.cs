using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class ArchivesWidget : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public ArchivesWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var monthPosts = await _blogRepository.CountMonthPostsAsync(12);

            return View(monthPosts);
        }
    }
}
