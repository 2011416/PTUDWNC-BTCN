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
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogDbContext _context;

        public AuthorRepository(BlogDbContext context)
        {
            _context = context;
        }
        public async Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }


        public async Task<Author> GetAuthorBySlugAsync(string urlSlug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .FirstOrDefaultAsync(s => s.UrlSlug == urlSlug, cancellationToken);
        }



        public async Task<bool> DeleteAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .Include(p => p.Posts)
                .Select(x => new AuthorItem()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    UrlSlug = x.UrlSlug,
                    ImageUrl = x.ImageUrl,
                    JoinedDate = x.JoinedDate,
                    Email = x.Email,
                    Notes = x.Notes,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public IQueryable<AuthorItem> FilterAuthors(IAuthorQuery query)
        {
            IQueryable<AuthorItem> authorQuery = _context.Set<Author>()
                .Select(x => new AuthorItem()
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    UrlSlug = x.UrlSlug,
                    ImageUrl = x.ImageUrl,
                    JoinedDate = x.JoinedDate,
                    Email = x.Email,
                    Notes = x.Notes,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                authorQuery = authorQuery.Where(x => x.FullName.Contains(query.Keyword) ||
                                                    x.Notes.Contains(query.Keyword) ||
                                                    x.Email.Contains(query.Keyword));
            }
            if (query.Month != null)
            {
                authorQuery = authorQuery.Where(x => x.JoinedDate.Month == query.Month);
            }
            if (query.Year != null)
            {
                authorQuery = authorQuery.Where(x => x.JoinedDate.Year == query.Year);
            }

            return authorQuery;
        }

 
        public async Task<Author> CreateOrUpdateAuthorAsync(Author author, CancellationToken cancellationToken = default)
        {
            if (_context.Set<Author>().Any(s => s.Id == author.Id))
            {
                _context.Entry(author).State = EntityState.Modified;
            }
            else
            {
                _context.Authors.Add(author);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return author;
        }
    }
}

