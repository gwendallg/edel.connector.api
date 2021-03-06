﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autumn.Mvc.Models.Paginations;
using Edel.Connector.Api.Models;

namespace Edel.Connector.Api.Services
{
    public interface ISubscriptionService
    {
        /// <summary>
        /// add new subscription
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<Subscription> AddAsync(Subscription subscription, string password);

        Task<Subscription> FindOneAsync(string userId);

        Task<IPage<Subscription>> FindAsync(Expression<Func<Subscription, bool>> filter,
            IPageable<Subscription> pageable);
    }
}