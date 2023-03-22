using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class AuthorEditModel
    {
        public int Id { get; set; }

        [DisplayName("Tên")]
        public string FullName { get; set; }

        [DisplayName("Slug")]
        public string UrlSlug { get; set; }

        [DisplayName("Chọn hình ảnh")]
        public IFormFile ImageFile { get; set; }

        [DisplayName("Ảnh đại diện")]
        public string ImageUrl { get; set; }
      
        [DisplayName("Ghi Chú")]
        public string Notes { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }
   


    }
}
