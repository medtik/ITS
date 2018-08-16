namespace Core.ApplicationService.Business.EntityService
{
    using System;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using Core.ObjectModels.Entities;

    public interface IUserService
    {
        int CreateUser(User entity);

        bool Update(User user);

        bool Delete(int userId);

        User Find(int userId, params Expression<Func<User, object>>[] includes);

        IEnumerable<Group> GetGroups(int userId);

        IEnumerable<User> Search(Func<User, bool> searchValue, params Expression<Func<User, object>>[] includes);

        bool SetMobileToken(User user, string token);

        IEnumerable<GroupInvitation> GetGroupInvitations(int userId, params Expression<Func<GroupInvitation, object>>[] includes);
    }
}