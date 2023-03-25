using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;

using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(
            IMapper mapper,
            IBlogRepository blogRepository,
            IAuthorRepository authorRepository)
        {
            _blogRepository = blogRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index(AuthorFilterModel model, [FromQuery(Name = "p")] int pageNumber = 1, [FromQuery(Name = "ps")] int pageSize = 5)
        {
            IPagingParams pagingParams = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            // Sử dụng Mapster để tạo đối tượng PostQuery
            // từ đối tượng PostFillterModel model
            var authorQuery = _mapper.Map<AuthorQuery>(model);


            ViewBag.AuthorsList = await _authorRepository
                .GetPagedAuthorsAsync(pagingParams);


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            // ID = 0 <=> Thêm bài viết mới
            // ID > 0 : Đọc dữ liệu của bài viết từ CSDL
            var author = id > 0
                ? await _authorRepository.GetAuthorByIdAsync(id)
                : null;

            // Tạo view model từ dữ liệu của bài viết
            var model = author == null
                ? new AuthorEditModel()
                : _mapper.Map<AuthorEditModel>(author);

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(
            AuthorEditModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var  author = model.Id > 0
                ? await _authorRepository.GetAuthorByIdAsync(model.Id)
                : null;

            if (author == null)
            {
                author = _mapper.Map<Author>(model);

                author.Id = 0;
                author.JoinedDate = DateTime.Now;
            }
            else
            {
                _mapper.Map(model, author);

            }


            await _authorRepository.CreateOrUpdateAuthorAsync(author);

            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorRepository.DeleteAuthorByIdAsync(id);

            return RedirectToAction("Index");
        }

    }
} 
