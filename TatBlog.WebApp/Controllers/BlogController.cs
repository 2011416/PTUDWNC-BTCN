using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {

        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            var postQuery = new PostQuery()
            {
                Published = true,

                Keyword = keyword

            };

            var postsList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);


            ViewBag.PostQuery = postQuery;

            return View(postsList);
        }


        public IActionResult About()
            => View();

        public IActionResult Contact()
            => View();

        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhật");



        public async Task<IActionResult> Tag([FromRoute(Name = "slug")] string slug)
        {
            var postQuery = new PostQuery()
            {
                TagSlug = slug,
            };
            ViewBag.PostQuery = postQuery;
            var postList = await _blogRepository.GetPagedPostsAsync(postQuery);
            return View("Index", postList);
        }

        public async Task<IActionResult> Category([FromRoute(Name = "slug")] string slug)
        {
            var postQuery = new PostQuery()
            {
                CategorySlug = slug,
            };
            ViewBag.PostQuery = postQuery; ;
            var postList = await _blogRepository.GetPagedPostsAsync(postQuery);
            return View("Index", postList);
        }

            public async Task<IActionResult> Post(
            string slug,
            int year,
            int month,
            int day)
        {

            var post = await _blogRepository.GetPostAsync(year, month, day, slug);
            await _blogRepository.IncreaseViewCountAsync(post.Id);
 
            return View(post);
        }

        public async Task<IActionResult> Author([FromRoute(Name = "slug")] string slug)
        {
            var postQuery = new PostQuery()
            {
                AuthorSlug = slug,
            };
            ViewBag.PostQuery = postQuery; ;
            var postList = await _blogRepository.GetPagedPostsAsync(postQuery);
            return View("Index", postList);
        }

        public async Task<IActionResult> Archives(int year, int month,[FromQuery(Name = "p")] int pageNumber = 1,[FromQuery(Name = "ps")] int pageSize = 3)
        {
            var postQuery = new PostQuery()
            {
                Month = month,
                Year = year,
                Published = true
            };

            var postsList = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
            ViewBag.Date = new DateTime(year, month, 1);
            ViewBag.PostQuery = postQuery;
            return View(postsList);
        }
    }
}
