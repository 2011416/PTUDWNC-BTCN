using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs
{
    public interface IAuthorRepository
    {

        Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Author> GetAuthorBySlugAsync(string urlSlug, CancellationToken cancellationToken = default);

        Task<bool> DeleteAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);
        Task<IPagedList<AuthorItem>> GetPagedAuthorsByQueryAsync(IAuthorQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);
        IQueryable<AuthorItem> FilterAuthors(IAuthorQuery query);
        Task<Author> CreateOrUpdateAuthorAsync(Author author, CancellationToken cancellationToken = default);
    }
}
