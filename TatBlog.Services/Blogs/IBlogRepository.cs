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
        // Tìm bài viết có tên định danh là 'slug'
        // và được đăng vào tháng 'month' năm 'year'

        Task<Post> GetPostAsync(int year, int month,string slug, CancellationToken cancellationToken = default);

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

        Task<IPagedList<Post>> GetPostByQueryAsync(IPagingParams pagingParams, PostQuery query, CancellationToken cancellationToken = default);
    }
}
