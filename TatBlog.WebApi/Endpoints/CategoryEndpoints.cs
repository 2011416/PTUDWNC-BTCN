using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static WebApplication MapCategoryEndpoints (
            this WebApplication app) 
        {
            var routeGroupBuilder = app.MapGroup("/api/categories");


            routeGroupBuilder.MapGet("/", GetCategories)
                .WithName("GetCategories")
                .Produces<ApiResponse<PaginationResult<CategoryItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCategoryById)
                .WithName("GetCategoryById")
                .Produces<ApiResponse<CategoryItem>>();
            

            routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByCategorySlug)
                .WithName("GetPostsByCategorySlug")
                .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
                .WithName("DeleteCategory")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            return app;
        }

        private static async Task<IResult> GetCategories(
       [AsParameters] CategoryFilterModel model,
       IBlogRepository blogRepository)
        {
            var categoryList = await blogRepository.GetPagedCategoriesAsync(model, model.Name);

            var paginationResult = new PaginationResult<CategoryItem>(categoryList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetCategoryById(int id, IBlogRepository blogRepository, IMapper mapper)
        {
            var category = await blogRepository.GetCategoryByIdAsync(id);

            return category == null

                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy chủ đề có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category)));
        }

        private static async Task<IResult> GetPostsByCategorySlug(
            [FromRoute] string slug,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                CategorySlug = slug,
                Published = true
            };

            var postsLists = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsLists);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> DeleteCategory(int id, IBlogRepository blogRepository)
        {
            return await blogRepository.DeleteCategoryByIdAsync(id)
                ? Results.Ok(ApiResponse.Success("Xóa thành công",
                HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                $"Không thể tìm thấy chủ đề có mã số {id}"));
        }

       
    }
}
