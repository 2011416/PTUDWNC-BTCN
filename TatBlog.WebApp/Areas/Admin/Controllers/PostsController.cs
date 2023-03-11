using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController :Controller
    {
        public IActionResult Index()
        {
            return View(); 
        }
    }
}
