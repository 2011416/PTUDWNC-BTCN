using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class CategoryEditModel
    {
        public int Id { get; set; }

        [DisplayName("Tên")]
        public string? Name { get; set; }

        [DisplayName("Giới thiệu")]
        public string? Description { get; set; }

        [DisplayName("Slug")]
        public string? UrlSlug { get; set; }

        [DisplayName("Hiển thị trên menu")]
        public bool ShowOnMenu { get; set; }
    }
}
