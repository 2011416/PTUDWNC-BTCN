using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class CategoriesController :Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public CategoriesController(
            IMapper mapper,
            IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index(CategoryFilterModel model, [FromQuery(Name = "p")] int pageNumber = 1, [FromQuery(Name = "ps")] int pageSize = 5)
        {
            IPagingParams pagingParams = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortOrder = "DESC",
                SortColumn = "PostCount"
            };

            // Sử dụng Mapster để tạo đối tượng PostQuery
            // từ đối tượng PostFillterModel model
            var categoryQuery = _mapper.Map<CategoryQuery>(model);


            ViewBag.CategoriesList = await _blogRepository
                .GetPagedCategoriesAsync(pagingParams);
   

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            // ID = 0 <=> Thêm bài viết mới
            // ID > 0 : Đọc dữ liệu của bài viết từ CSDL
            var post = id > 0
                ? await _blogRepository.GetCategoryByIdAsync(id)
                : null;

            // Tạo view model từ dữ liệu của bài viết
            var model = post == null
                ? new CategoryEditModel()
                : _mapper.Map<CategoryEditModel>(post);

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(
            CategoryEditModel model)
        {        

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = model.Id > 0
                ? await _blogRepository.GetCategoryByIdAsync(model.Id)
                : null;

            if (category == null)
            {
                category = _mapper.Map<Category>(model);

                category.Id = 0;
            }
            else
            {
                _mapper.Map(model, category);

            }

         
            await _blogRepository.CreateOrUpdateCategoryAsync(category);

            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _blogRepository.DeleteCategoryByIdAsync(id);

            return RedirectToAction("Index");
        }

    }
}

