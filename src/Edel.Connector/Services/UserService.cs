﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Autumn.Mvc.Data.Configurations;
using Edel.Connector.Entities;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Edel.Connector.Services
{
    public class UserService : AbstractCollectionService<User>, IUserService
    {
        private readonly AutumnDataSettings _dataSettings;
        private readonly IClaimsService _claimsService;

        public UserService(AutumnDataSettings dataSettings, IMongoDatabase database,
            IHttpContextAccessor contextAccessor, IClaimsService claimsService) : base(database, contextAccessor, "user")
        {
            _dataSettings = dataSettings;
            _claimsService = claimsService;
        }

        public async Task<User> AddAsync(User user, string password)
        {
            user.Salt = GetRandomSalt();
            user.Hash = GetHash(password, user.Salt);
            CheckClaims(user);
            user.Metadata = new Metadata
            {
                CreatedAt = Context()?.User?.Identity?.Name,
                CreatedDate = DateTime.UtcNow
            };
            await Collection().InsertOneAsync(user);
            return user;
        }

        private Dictionary<string, Type> GetEntityTypeByRoute()
        {
            
            var result = _dataSettings
                .Routes
                .ToDictionary(x => x.Value.Template.TrimStart('/').Replace('/', '_'), x => x.Key.GetGenericArguments()[0]);
            result.Add("v1_user", typeof(User));
            result.Add("v1_subscription",typeof(Subscription));
            return result;
        }

        private void CheckClaims(User user)
        {
            var claimsByEntity = _claimsService.GetClaimsByEntityType();
            var entityTypeByRoute = GetEntityTypeByRoute();
            foreach (var claim in user.Claims)
            {
                if (!entityTypeByRoute.ContainsKey(claim.Key))
                {
                    throw new Exception(string.Format("Claims {0} not exist", claim.Key));
                }

                if (_claimsService.TryParse(claim.Value, out var scopes))
                {
                   // TODO  check claims    
                }
                else
                {
                    throw new Exception(string.Format("Scopes {0} not exist for Claim {1}", claim.Value, claim.Key));
                }
            }
        }
        
        

        public async Task<User> FindByUserNameAsync(string userName)
        {
            return await Collection()
                .Find(u => u.Username == userName)
                .SingleOrDefaultAsync();
        }

        public async Task<User> UpdateAsync(User user, string password)
        {

            var userDb = await Collection()
                .Find(u => u.Username == user.Username)
                .SingleOrDefaultAsync();

            if (userDb == null) throw new Exception("User not found");
            userDb.Salt = GetRandomSalt();
            userDb.Hash = GetHash(password, userDb.Salt);
            userDb.Claims = user.Claims;
            user.Metadata.LastModifiedDate = DateTime.UtcNow;
            userDb.Metadata.LastModifiedAt = Context()?.User?.Identity?.Name;
            CheckClaims(userDb);
            await Collection().ReplaceOneAsync(u => u.Username == userDb.Username, userDb);
            return userDb;
        }

        /// <summary>
        /// authenticate user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> Authenticate(string userName, string password)
        {
            var user = await FindByUserNameAsync(userName);
            if (user == null) return null;
            var expected = GetHash(password, user.Salt);
            if (expected != user.Hash) return null;
            return user;
        }

        static string GetHash(string password, string salt)
        {
            var sha512 = SHA512Managed.Create();
            var bytes = Encoding.UTF8.GetBytes(string.Concat(password,salt));
            var hash = sha512.ComputeHash(bytes);
            var result = new StringBuilder();
            foreach (var t in hash)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }

        static string GetRandomSalt()
        {
            var bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public void TryAddAdminIfNotExistUsers(string adminPassword = "admin")
        {
            var count = Collection().Count(u => true);
            if (count != 0) return;
            var stringBuilder = new StringBuilder();
            foreach (var item in Enum.GetNames(typeof(ScopeType)))
            {
                stringBuilder.Append(string.Concat(item, " ,"));
            }

            var scope = stringBuilder.ToString().TrimEnd(',').Trim();
            var claims = new Dictionary<string, string>();
            claims.Add("v1_user",scope);
            claims.Add("v1_subscriptin",scope);
            foreach (var info in _dataSettings.EntitiesInfos.Values)
            {
                var claimKey = string.Format("{0}_{1}", info.ApiVersion, info.Name);
                stringBuilder = new StringBuilder();
                stringBuilder.Append(ScopeType.Read.ToString() + ",");
                if (!info.IgnoreOperations.Contains(HttpMethod.Post))
                    stringBuilder.Append(ScopeType.Create.ToString() + ",");
                if (!info.IgnoreOperations.Contains(HttpMethod.Put))
                    stringBuilder.Append(ScopeType.Update.ToString() + ",");
                if (!info.IgnoreOperations.Contains(HttpMethod.Delete))
                    stringBuilder.Append(ScopeType.Delete.ToString() + ",");
                var claimValue = stringBuilder.ToString().Trim().TrimEnd(',');
                claims.Add(claimKey, claimValue);
            }

            var user = new User();
            user.Username = "admin";
            user.Salt = GetRandomSalt();
            user.Hash = GetHash(adminPassword, user.Salt);
            user.Claims = claims;
            user.Metadata = new Metadata();
            user.Metadata.CreatedAt = "admin";
            user.Metadata.CreatedDate = DateTime.UtcNow;
            Collection().InsertOne(user);
        }
    }
}