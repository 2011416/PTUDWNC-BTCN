using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public class SubscriberRepository
    {
        public Task SubcribeAsync(string email)
        {
            return null;
        }

        public Task UnSubcribeAsync(string email,string reason, bool voluntary)
        {
            return null;
        }

        public Task BlockSubcribeAsync(int id, string reason, string notes)
        {
            return null;
        }

        public Task DeleteSubcribeAsync(int id)
        {
            return null;
        }

        public Task GetSubcribeByIdAsync(int id)
        {
            return null;
        }

        public Task GetSubcribeByEmailAsync(string email)
        {
            return null;
        }

        public Task<IPagedList<Subscriber>> SearchSubscribersAsync(IPagingParams pagingParams, string keyword, bool unsubscribed, bool involuntary)
        {
            return null;
        }
    }
}
