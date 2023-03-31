using MapsterMapper;
using System.Net;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class TagEndpoints
    {
        public static WebApplication MapTagEndPoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/tags");

            routeGroupBuilder.MapGet("/{id:int}", GetTagById)
               .WithName("GetTagById")
               .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
                .WithName("DeleteTag")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetTagById(int id, IBlogRepository blogRepository, IMapper mapper)
        {
            var tag = await blogRepository.GetTagByIdAsync(id);

            return tag == null

                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy thẻ có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag)));
        }

        private static async Task<IResult> DeleteTag(int id, IBlogRepository blogRepository)
        {
            return await blogRepository.DeleteTagByIdAsync(id)

                ? Results.Ok(ApiResponse.Success("Xóa thành công",
                HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, 
                $"Không thể tìm thấy thẻ có mã số {id}"));
        }

    }
}
