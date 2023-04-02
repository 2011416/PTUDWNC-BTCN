﻿using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class AuthorEndpoints
    {
        public static WebApplication MapAuthorEndpoints(
            this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/authors");

            routeGroupBuilder.MapGet("/", GetAuthors)
                .WithName("GetAuthors")
                .Produces<ApiResponse<PaginationResult<AuthorItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
               .WithName("GetAuthorById")
               .Produces<ApiResponse<AuthorItem>>();

            routeGroupBuilder.MapGet(
                "/{slug:regex(^[a-z0-9_-]+$)}/posts", 
                GetPostsByAuthorSlug)
                .WithName("GetPostsByAuthorSlug")
                .Produces < ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapGet("/best/{limit:int}", GetAuthorsMostArticles)
               .WithDescription("GetBestAuthors")
               .Produces<ApiResponse<IPagedList<AuthorItem>>>();

            routeGroupBuilder.MapPost("/", AddAuthor)             
                .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
                .WithName("AddNewAuthor")
                .RequireAuthorization()
                .Produces(401)
                .Produces<ApiResponse<AuthorItem>>();


            routeGroupBuilder.MapPost("/{id:int}/picture", SetAuthorPicture)
                .WithName("SetAuthorPicture")
                .RequireAuthorization()
                .Accepts<IFormFile>("multipart/form-data")
                .Produces(401)
                .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
                .WithName("UpdateAnAuthor")
                .RequireAuthorization()
                .Produces(401)
                .Produces<ApiResponse<string>>();


            routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
                .WithName("DeleteAnAuthor")
                .RequireAuthorization()
                .Produces(401)
                .Produces<ApiResponse<string>>();

            return app;
        }



        private static async Task<IResult> GetAuthors(
            [AsParameters] AuthorFilterModel model,
            IAuthorRepository authorRepository)
        {
            var authorsList = await authorRepository
                .GetPagedAuthorsAsync(model, model.Name);

            var paginationResult =
                new PaginationResult<AuthorItem>(authorsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetAuthorDetails(
            int id, 
            IAuthorRepository authorRepository, 
            IMapper mapper)
        {
            var author = await authorRepository.GetCachedAuthorByIdAsync(id);

            return author == null
                ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                $"Không tìm thấy tác giả có mã số {id}"))
                : Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
        }

        private static async Task<IResult> GetPostsByAuthor(
            int id,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                AuthorId = id,
                Published = true
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPostsByAuthorSlug(
            [FromRoute] string slug, 
            [AsParameters] PagingModel pagingModel, 
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                AuthorSlug = slug,
                Published = true
            };

            var postsLists = await blogRepository.GetPagedPostsAsync(
                postQuery, pagingModel,
                posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsLists);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> AddAuthor(
            AuthorEditModel model, 
            IValidator<AuthorEditModel> validator,
            IAuthorRepository authorRepository, 
            IMapper mapper)
        {
            
            if (await authorRepository
                    .IsAuthorSlugExistedAsync(0, model.UrlSlug))

            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var author = mapper.Map<Author>(model);
            await authorRepository.AddOrUpdateAsync(author);

            return Results.Ok(ApiResponse.Success(              
                mapper.Map<AuthorItem>(author), HttpStatusCode.Created));
        }

        private static async Task<IResult> SetAuthorPicture(
            int id, IFormFile imageFile, 
            IAuthorRepository authorRepository, 
            IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(
                imageFile.OpenReadStream(), 
                imageFile.FileName, 
                imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.BadRequest,"Không lưu được tập tin"));
            }

            await authorRepository.SetImageUrlAsync(id, imageUrl);
            return Results.Ok(ApiResponse.Success(imageUrl));
        }

        private static async Task<IResult> UpdateAuthor(
            int id, AuthorEditModel model, 
            IValidator<AuthorEditModel> validator,
            IAuthorRepository authorRepository, 
            IMapper mapper)
        {

            var validationResult = await validator.ValidateAsync(model);
            
            if (!validationResult.IsValid)
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.BadRequest, validationResult));
            }

            if (await authorRepository
                    .IsAuthorSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.Conflict, 
                    $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var author = mapper.Map<Author>(model);
            author.Id = id;

            return await authorRepository.AddOrUpdateAsync(author)
                ? Results.Ok(ApiResponse.Success("Author is updated",
                HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author"));
        }

        private static async Task<IResult> DeleteAuthor(
            int id, IAuthorRepository authorRepository)
        {
            return await authorRepository.DeleteAuthorAsync(id)
                ? Results.Ok(ApiResponse.Success("Author is deleted",
                HttpStatusCode.NoContent))
                : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find author "));
        }

        private static async Task<IResult> GetAuthorsMostArticles(int numberAuthor, IAuthorRepository authorRepository)
        {
            var authors = await authorRepository.GetAuthorsAsync(numberAuthor);

            return Results.Ok(ApiResponse.Success(authors));
        }
    }
}
