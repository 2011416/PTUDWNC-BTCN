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
    public class TagsController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public TagsController(
            IMapper mapper,
            IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index(TagFilterModel model, [FromQuery(Name = "p")] int pageNumber = 1, [FromQuery(Name = "ps")] int pageSize = 5)
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
            var tagQuery = _mapper.Map<TagQuery>(model);


            ViewBag.TagsList = await _blogRepository
                .GetPagedTagsAsync(pagingParams);


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            // ID = 0 <=> Thêm bài viết mới
            // ID > 0 : Đọc dữ liệu của bài viết từ CSDL
            var tag = id > 0
                ? await _blogRepository.GetTagByIdAsync(id)
                : null;

            // Tạo view model từ dữ liệu của bài viết
            var model = tag == null
                ? new TagEditModel()
                : _mapper.Map<TagEditModel>(tag);

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(
            TagEditModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var tag = model.Id > 0
                ? await _blogRepository.GetTagByIdAsync(model.Id)
                : null;

            if (tag == null)
            {
                tag = _mapper.Map<Tag>(model);

                tag.Id = 0;
            }
            else
            {
                _mapper.Map(model, tag);

            }


            await _blogRepository.CreateOrUpdateTagAsync(tag);

            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> DeleteTag(int id)
        {
            await _blogRepository.DeleteTagByIdAsync(id);

            return RedirectToAction("Index");
        }
    }
}
