namespace Core.ApplicationService.Business.EntityService
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Core.ObjectModels.Entities;

    public interface IGroupService
    {
        Group Find(int id, params Expression<Func<Group, object>>[] includes);

        bool Create(Group entity);

        bool Update(Group entity);

        bool Delete(Group group);

        bool AddPlanToGroup(int planId, int groupId);

        bool GroupInvitations(int userId, int groupId, string messsage);

        bool UserRemoved(int userId, int groupId);

        bool AddUser(int userId, int groupId);

        IQueryable<GroupInvitation> GetGroupInvitations(int userId);

        GroupInvitation GetGroupInvitation(int id);

        bool IsExisted(int userId, int groupId);

        bool UpdateGroupInvitation(GroupInvitation groupInvitation);
    }
}