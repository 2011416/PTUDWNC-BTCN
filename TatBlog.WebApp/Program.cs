using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

var builder = WebApplication.CreateBuilder(args);
{
    // Thêm các dịch vụ được yêu cầu bởi MVC Framework
    builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IBlogRepository, BlogRepository>();
    builder.Services.AddScoped<IDataSeeder, DataSeeder>();

}


var app = builder.Build();
{
    // Cấu hình HTTP Request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Blog/Error");

        app.UseHsts();
    }



    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.MapControllerRoute(
        name: "posts-by-category",
        pattern: "blog/category/{slug}",
        defaults: new { controller = "Blog", Action = "Category" });

    app.MapControllerRoute(
     name: "posts-by-tag",
     pattern: "blog/tag/{slug}",
     defaults: new { controller = "Blog", Action = "Tag" });

    app.MapControllerRoute(
     name: "single-post",
     pattern: "blog/post{year:int}/{month:int}/{day:int}/{slug}",
     defaults: new { controller = "Blog", Action = "Post" });

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Blog}/{action=Index}/{id?}");

}

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    seeder.Initialize();
}

    app.Run();
