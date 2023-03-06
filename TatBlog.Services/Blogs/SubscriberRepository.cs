using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public class SubscriberRepository : ISubscriberRepository
    {
        public Task BlockSubscriberAsync(int id, string reason, string notes)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSubscriberAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Subscriber> GetSubscriberByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Subscriber> GetSubscriberByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IPagedList<Subscriber>> SearchSubscribersAsync(IPagingParams pagingParams, string keyword, bool unsubscribed, bool involuntary)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task UnsubscribeAsync(string email, string reason, bool voluntary)
        {
            throw new NotImplementedException();
        }
    }
}
