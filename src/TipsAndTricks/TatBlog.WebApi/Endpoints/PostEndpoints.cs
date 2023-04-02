using Mapster;
using MapsterMapper;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class PostEndpoints
    {
        public static WebApplication MapPostEndpoints(
          this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");

            routeGroupBuilder.MapGet("/", GetPost)
               .WithName("GetPosts")
               .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetPostById)
                .WithName("GetPostById")
                .Produces<ApiResponse<PostDetail>>();

            routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPosts)
                .WithName("GetRandomPosts")
                .Produces<ApiResponse<IList<PostDto>>>();

            routeGroupBuilder.MapGet("/archive/{limit:int}", GetArchivePosts)
              .WithName("GetArchivePosts")
              .Produces<ApiResponse<IList<MonthPostCount>>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
               .WithName("DeletePost")
               .Produces(401)
               .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetPost(
       [AsParameters] PostFilterModel model,
       IBlogRepository blogRepository,
       IMapper mapper)
        {
            var postsQuery = mapper.Map<PostQuery>(model);

            var postList = await blogRepository.GetPagedPostsAsync(postsQuery,model, p => p.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPostById(int id, IBlogRepository blogRepository, IMapper mapper)
        {
            var post = await blogRepository.GetPostByIdAsync(id);

            return post != null

                ? Results.Ok(ApiResponse.Success(
                    mapper.Map<PostDetail>(post)))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã số {id}"));
        }

   
        private static async Task<IResult> GetRandomPosts(int number, IBlogRepository blogRepository, IMapper mapper)
        {
            var posts = await blogRepository.GetRandomPostsAsync(number);

            return posts.Count != 0

                ? Results.Ok(ApiResponse.Success(
                    mapper.Map<IList<PostDto>>(posts)))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy bài viết"));
        }

        private static async Task<IResult> GetArchivePosts(int number, IBlogRepository blogRepository, IMapper mapper)
        {
            var posts = await blogRepository.CountMonthPostsAsync(number);

            return posts.Count != 0

                ? Results.Ok(ApiResponse.Success(
                    mapper.Map<IList<MonthPostCount>>(posts)))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy bài viết"));
        }

        private static async Task<IResult> DeletePost(int id, IBlogRepository blogRepository)
        {
            return await blogRepository.DeletePostById(id)

                ? Results.Ok(ApiResponse.Success("Xóa thành công", 
                HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã số {id}"));
        }
    }
}
