using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {

        Task<Post> GetPostAsync(string slug, CancellationToken cancellationToken = default);

        Task<Post> GetPostByIdAsync(int id, bool published = false, CancellationToken cancellationToken = default);


        Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(int numMonths, CancellationToken cancellationToken = default);

        Task<IList<Post>> GetRandomPostsAsync(int randomOfPosts, CancellationToken cancellationToken = default);
        Task<Post> CreateOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default);

        // Tìm Top N Bài viết phổ được nhiều người xem nhất
        Task<IList<Post>> GetPopularArticlesAsync( int numPosts, CancellationToken cancellationToken= default);

        Task<bool> IsPostSlugExistedAsync( int postId, string slug, CancellationToken token = default);

        Task IncreaseViewCountAsync( int postId,CancellationToken cancellationToken = default);

        Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default);

        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);

        Task<Tag> GetTagSlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<bool> DeleteTagByNameAsync(int id, CancellationToken cancellationToken = default);

        Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default);

        Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> IsCategorySlugExistedAsync(string slug, CancellationToken cancellationToken = default);

        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<IList<Author>> GetAuthorsAsync(CancellationToken cancellationToken = default);

        Task<IPagedList<Post>> GetPagedPostsAsync(PostQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedPostsAsync<T>(PostQuery condition, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper);
    }
}
