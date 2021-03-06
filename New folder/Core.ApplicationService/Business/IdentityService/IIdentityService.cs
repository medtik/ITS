﻿namespace Core.ApplicationService.Business.IdentityService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.ObjectModels.Entities;
    using Core.ObjectModels.Identity;

    public interface IIdentityService
    {
        void LoginAvaiable(string userId);

        bool IsInRole(string roleName, string accountId);

        Task ChangeRole(string userId);

        Task<_IdentityData> FindByUsername(string username);

        Task<_IdentityData> AddLoginAsync(string userId, string a, string b);

        Task<User> Find(string accountId);

        Task<_IdentityData> FindAccount(string accountId);

        _IdentityData FindAccount(int userId);

        Task<_IdentityData> LoginExternalAsync(string providerName, string providerKey);

        Task<_IdentityData> Find(string username, string password);

        Task<string> GetFullName(string username);

        Task<string> GetRole(string userName);

        Task<_IdentityData> Create(string usernameOrEmail, string password, string fullName, string address, string phoneNumber, DateTimeOffset birthdate, params string[] roles);

        Client FindClient(string clientId);

        Task<bool> AddRefreshToken(RefreshToken token);

        Task<bool> RemoveRefreshToken(string refreshTokenId);

        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);

        Task<RefreshToken> FindRefreshToken(string refreshTokenId);

        IEnumerable<RefreshToken> GetAllRefreshTokens();

        Task<bool> ChangePassword(string username, string newPassword);

        void SaveChanges();

        Task<_IdentityData> FindAsync(string provider, string userId);

        Task<string> CreateAsync(string UserName);
    }
}