using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;


////Tạo đối tượng DbContext để quản lý phiên làm việc
//// Với CSDL và trạng thái của các đối tượng
var context = new BlogDbContext();

////Tạo đối tượng Blogrepository
IBlogRepository blogRepo = new BlogRepository(context);

//////Xuất 3 bài viết có lượt xem nhiều nhất
////var posts = await blogRepo.GetPopularArticlesAsync(3);

//// Tạo đối tượng chứa tham số phân trang
var pagingParams = new PagingParams
{
    PageNumber = 1,        //Lấy kết quả ở trang số 1
    PageSize = 5,          //Lấy 5 mẫu ti
    SortColumn = "Name",   // Sắp xếp theo tên
    SortOrder = "DESC"     //Theo chiều giảm dần
};


////// Lấy danh sách từ khóa
var tagslist = await blogRepo.GetPagedTagsAsync(pagingParams);

////// Xuất ra màn hình
////Console.WriteLine("{0,-5}{1,-50}{2,10}",
////    "ID", "Name", "Count");



////foreach (var item in tagsList)
////{
////    Console.WriteLine("{0,-5}{1,-50}{2,10}",
////        item.Id, item.Name, item.PostCount);
////}

////Console.WriteLine("".PadRight(80, '-'));

////// Đọc danh sách bài viết từ cơ sở dữ liệu
////// Lấy kèm tên tác giả và chuyên mục

//////var posts = context.Posts
//////    .Where(p => p.Published)
//////    .OrderBy(p => p.Title)
//////    .Select(p => new
//////    {
//////        Id = p.Id,
//////        Title = p.Title,
//////        ViewCount = p.ViewCount,
//////        PostedDate = p.PostedDate,
//////        Author = p.Author.FullName,
//////        Category = p.Category.Name,
//////    })
//////    .ToList();


////// Xuất danh sách bài viết ra màn hình
////foreach (var post in posts)
////{
////    Console.WriteLine("ID      : {0}", post.Id);
////    Console.WriteLine("Title   : {0}", post.Title);
////    Console.WriteLine("View    : {0}", post.ViewCount);
////    Console.WriteLine("Date    : {0:MM/dd/yyyy}", post.PostedDate);
////    Console.WriteLine("Author  : {0}", post.Author);
////    Console.WriteLine("Category: {0}", post.Category);
////    Console.WriteLine("".PadRight(80, '-'));
////}

////// Tạo đối tượng khởi tạo dữ liệu mẫu
////var seeder = new DataSeeder(context);

////// Gọi hàm Inititalize để nhập dữ liệu mẫu
////seeder.Initialize();

////// Đọc danh sách tác giả từ cơ sở dữ liệu
////var authors = context.Authors.ToList();

////// Xuất danh sách tác giả ra màn hình
////Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}",
////    "ID", "Full Name", "Email", "Joined Date");

////foreach (var author in authors)
////{
////    Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12:MM/dd/yyyy}",
////        author.Id, author.FullName, author.Email, author.JoinedDate);
////}
////Console.WriteLine("".PadRight(80, '-'));

////var categories = await blogRepo.GetCategoriesAsync();

////Console.WriteLine("{0,-5}{1,-50}{2,10}",
////    "ID", "Name", "Count");

////foreach (var item in categories)
////{
////    Console.WriteLine("{0,-5}{1,-50}{2,10}",
////        item.Id, item.Name, item.PostCount);
////}



////1a

//var taglist = await blogRepo.GetTagSlugAsync("google-applications");

//Console.WriteLine("{0, -5}{1, -10}{2, 20}{3, 20}", "ID", "Name", "Description", "Slug");
//Console.WriteLine("{0, -5}{1, -10}{2, 20}{3, 30}", taglist?.Id, taglist?.Name, taglist?.Description, taglist?.UrlSlug);

////var tagList = await blogRepo.GetTagSlugAsync("google-applications");

////Console.WriteLine("{0, -5}{1, -50}", "ID", "Name");
////foreach (var tag in tagList)
////{
////    Console.WriteLine("{0, -5}{1, -50}", tagList.Id, tagList.Name);
////}

//Console.WriteLine("".PadRight(80, '-'));

////1c
//var tags = await blogRepo.GetTagsAsync();
//Console.WriteLine("{0, -5}{1, -30}{2, -35}{3, -25}{4, 10}", "ID", "Name", "Description", "Slug", "Posts");
//foreach (var tag in tags)
//{
//    Console.WriteLine("{0, -5}{1, -30}{2, -35}{3, -25}{4, 10}", tag.Id, tag.Name, tag.Description, tag.UrlSlug, tag.PostCount);
//}


//Console.WriteLine("".PadRight(80, '-'));

////1d
////bool isSuccess = await blogRepo.DeleteTagByNameAsync(3);
////Console.WriteLine(isSuccess);

//Console.WriteLine("".PadRight(80, '-'));

////1e
////var category = await blogRepo.GetCategoryBySlugAsync("messaging");
////Console.WriteLine("{0, -5}{1, -10}{2, 20}{3, 20}", "ID", "Name", "Description", "Slug");
////Console.WriteLine("{0, -5}{1, -10}{2, 20}{3, 20}", category?.Id, category?.Name, category?.Description, category?.UrlSlug);

////Console.WriteLine("".PadRight(80, '-'));

////1f
//var category = await blogRepo.GetCategoryByIdAsync(5);
//Console.WriteLine("{0, -5}{1, -10}{2, 30}{3, 20}", "ID", "Name", "Description", "Slug");
//Console.WriteLine("{0, -5}{1, -10}{2, 25}{3, 20}", category?.Id, category?.Name, category?.Description, category?.UrlSlug);

//Console.WriteLine("".PadRight(80, '-'));

//// 1i
//var isExisted = await blogRepo.IsCategorySlugExistedAsync("architecture");
//Console.WriteLine(isExisted);

//Console.WriteLine("".PadRight(80, '-'));

//// 1j
//var pagingParams = new PagingParams
//{
//    PageNumber = 1,
//    PageSize = 5,
//    SortColumn = "Id"
//};

//var categories = await blogRepo.GetPagedCategoriesAsync(pagingParams);
//Console.WriteLine("{0, -5}{1, -30}{2, 40}{3, 20}{4, 25}", "ID", "Name", "Description", "Slug", "Posts");
//foreach (var categoryItem in categories)
//{
//    Console.WriteLine("{0, -5}{1, -30}{2, 40}{3, 30}{4, 15}", categoryItem?.Id, categoryItem?.Name, categoryItem?.Description, categoryItem?.UrlSlug, categoryItem?.PostCount);
//}

//Console.WriteLine("".PadRight(80, '-'));

////1l
//var postById = await blogRepo.GetPostByIdAsync(3);
//Console.WriteLine("{0, 20}{1, 50}{2, 30}", "Name", "Description", "Slug");
//Console.WriteLine("{0, 20}{1, 45}{2, 20}", postById.Title, postById.Description, postById.UrlSlug);

////1s


