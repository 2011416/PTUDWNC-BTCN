using Microsoft.EntityFrameworkCore;
using SlugGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Post> GetPostAsync(
         string slug,
         CancellationToken cancellationToken = default)
        {
            var postQuery = new PostQuery()
            {
                PublishedOnly = false,
                TitleSlug = slug
            };

            return await FilterPosts(postQuery).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Post> GetPostByIdAsync(int id, bool published = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postQuery = _context.Set<Post>()
                                     .Include(p => p.Category)
                                     .Include(p => p.Author)
                                     .Include(p => p.Tags);

            if (published)
            {
                postQuery = postQuery.Where(x => x.Published);
            }

            return await postQuery.Where(p => p.Id.Equals(id))
                                  .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<Post>> GetRandomPostsAsync(int randomOfPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .Include(t => t.Tags)
                .Where(x => x.Published)
                .OrderBy(x => Guid.NewGuid())
                .Take(randomOfPosts)
                .ToListAsync(cancellationToken);
        }

        public async Task<Post> CreateOrUpdatePostAsync(
          Post post, IEnumerable<string> tags,
          CancellationToken cancellationToken = default)
        {
            if (post.Id > 0)
            {
                await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
            }
            else
            {
                post.Tags = new List<Tag>();
            }

            var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new
                {
                    Name = x,
                    Slug = x.GenerateSlug()
                })
                .GroupBy(x => x.Slug)
                .ToDictionary(g => g.Key, g => g.First().Name);


            foreach (var kv in validTags)
            {
                if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

                var tag = await GetTagBySlugAsync(kv.Key, cancellationToken) ?? new Tag()
                {
                    Name = kv.Value,
                    Description = kv.Value,
                    UrlSlug = kv.Key
                };

                post.Tags.Add(tag);
            }

            post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

            if (post.Id > 0)
                _context.Update(post);
            else
                _context.Add(post);

            await _context.SaveChangesAsync(cancellationToken);

            return post;
        }

        public async Task PostPublishedStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.Published, x => !x.Published), cancellationToken);
        }

        public async Task<bool> DeletePostById(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }


        public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug,
                    cancellationToken);
        }

        public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p =>
                p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);
        }

        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }

            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });
                return await tagQuery
                      .ToPagedListAsync(pagingParams, cancellationToken);
        }
        public async Task<Tag> GetTagSlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tagsQuery = _context.Set<Tag>();


            if (!string.IsNullOrEmpty(slug))
            {
                tagsQuery = tagsQuery.Where(x => x.UrlSlug == slug);
            }

            return await tagsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                            .Include(p => p.Posts)
                            .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
        }

        public async Task<bool> DeleteTagByNameAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }

        public async Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    UrlSlug = x.UrlSlug,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync();
        }

        public async Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            return await categories
                .FirstOrDefaultAsync(x => x.UrlSlug.ToLower().Contains(slug.ToLower()), cancellationToken);
        }

        public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            return await categories
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> IsCategorySlugExistedAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AnyAsync(x => x.UrlSlug.CompareTo(slug) == 0, cancellationToken);
        }

        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var categoryQuery = _context.Set<Category>()
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await categoryQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(
        int numMonths, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .GroupBy(x => new { x.PostedDate.Year, x.PostedDate.Month })
                .Select(g => new MonthlyPostCountItem()
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    PostCount = g.Count(x => x.Published)
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<Post>> GetPagedPostsAsync(
        PostQuery condition,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Post.PostedDate), "DESC",
                cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedPostsAsync<T>(
        PostQuery condition,
        IPagingParams pagingParams,
        Func<IQueryable<Post>, IQueryable<T>> mapper)
        {
            var posts = FilterPosts(condition);
            var projectedPosts = mapper(posts);

            return await projectedPosts.ToPagedListAsync(pagingParams);
        }


        public IQueryable<Post> FilterPosts(PostQuery condition)
        {
            IQueryable<Post> posts = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);

            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.CategoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.AuthorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
                                         x.ShortDescription.Contains(condition.Keyword) ||
                                         x.Description.Contains(condition.Keyword) ||
                                         x.Category.Name.Contains(condition.Keyword) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
            }

            if (condition.Year > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.Year);
            }

            if (condition.Month > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.Month);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }

            return posts;

            //// Compact version
            //return _context.Set<Post>()
            //	.Include(x => x.Category)
            //	.Include(x => x.Author)
            //	.Include(x => x.Tags)
            //	.WhereIf(condition.PublishedOnly, x => x.Published)
            //	.WhereIf(condition.NotPublished, x => !x.Published)
            //	.WhereIf(condition.CategoryId > 0, x => x.CategoryId == condition.CategoryId)
            //	.WhereIf(!string.IsNullOrWhiteSpace(condition.CategorySlug), x => x.Category.UrlSlug == condition.CategorySlug)
            //	.WhereIf(condition.AuthorId > 0, x => x.AuthorId == condition.AuthorId)
            //	.WhereIf(!string.IsNullOrWhiteSpace(condition.AuthorSlug), x => x.Author.UrlSlug == condition.AuthorSlug)
            //	.WhereIf(!string.IsNullOrWhiteSpace(condition.TagSlug), x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug))
            //	.WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), x => x.Title.Contains(condition.Keyword) ||
            //	                                                             x.ShortDescription.Contains(condition.Keyword) ||
            //	                                                             x.Description.Contains(condition.Keyword) ||
            //	                                                             x.Category.Name.Contains(condition.Keyword) ||
            //	                                                             x.Tags.Any(t => t.Name.Contains(condition.Keyword)))
            //	.WhereIf(condition.Year > 0, x => x.PostedDate.Year == condition.Year)
            //	.WhereIf(condition.Month > 0, x => x.PostedDate.Month == condition.Month)
            //	.WhereIf(!string.IsNullOrWhiteSpace(condition.TitleSlug), x => x.UrlSlug == condition.TitleSlug);
        }
     
    }
}
