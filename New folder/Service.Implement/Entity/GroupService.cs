namespace Service.Implement.Entity
{
    using System;
    using System.Linq.Expressions;
    using System.Transactions;
    using Core.ObjectModels.Entities;
    using Core.ObjectService.Repositories;
    using Core.ObjectModels.Entities.Helper;
    using Core.ObjectModels.Entities.EnumType;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.LogService;
    using System.Linq;

    public class GroupService : _BaseService<Group>, IGroupService
    {
        private readonly IRepository<Member> _memberRepository;
        private readonly IRepository<Plan> _planRepository;
        private readonly IRepository<GroupInvitation> _groupInvitationRepository;

        public GroupService(ILoggingService loggingService, IUnitOfWork unitOfWork) : base(loggingService, unitOfWork)
        {
            _planRepository = unitOfWork.GetRepository<Plan>();
            _memberRepository = unitOfWork.GetRepository<Member>();
            _groupInvitationRepository = unitOfWork.GetRepository<GroupInvitation>();
        }

        public bool AddPlanToGroup(int planId, int groupId)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                Plan plan = _planRepository.Get(_ => _.Id == planId, _ => _.Notes, _ => _.PlanLocations);
                if (plan != null)
                {
                    
                    Plan clone = Clone.CloneObject(plan);

                    if (clone != null)
                    {
                        clone.IsPublic = false;//default when clone
                        clone.GroupId = groupId;
                        clone.MemberId = null;

                        _planRepository.Create(clone);
                        _unitOfWork.SaveChanges();

                        scope.Complete();
                        return true;
                    }//clone success
                }
                return false;
            }
        }

        public bool AddUser(int userId, int groupId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Member member = _memberRepository.Get(_ => _.Id == userId);
                    Group group = _repository.Get(_ => _.Id == groupId, _ => _.Members);

                    var temp = _groupInvitationRepository.Search(_ => _.GroupId == groupId && _.UserId == userId);

                    foreach (var item in temp)
                    {
                        item.Status = RequestStatus.Approved;
                        _groupInvitationRepository.Update(item);
                    }

                    group.Members.Add(member);

                    _repository.Update(group);
                    _unitOfWork.SaveChanges();

                    scope.Complete();
                    return true;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(UserRemoved), ex);
                return false;
            }
        }

        public Group Find(int id, params Expression<Func<Group, object>>[] includes)
        {
            return _repository.Get(_ => _.Id == id, includes);
        }

        public GroupInvitation GetGroupInvitation(int id)
        {
            return _groupInvitationRepository.Get(_ => _.Id == id);
        }

        public IQueryable<GroupInvitation> GetGroupInvitations(int userId)
        {
            return _groupInvitationRepository.SearchAsQueryable(_ => _.UserId == userId, _ => _.Group, _ => _.User);
        }

        public bool GroupInvitations(int userId, int groupId, string messsage)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    _groupInvitationRepository.Create(new GroupInvitation
                    {
                        GroupId = groupId,
                        UserId = userId,
                        Status = RequestStatus.NotYet,
                        Message = messsage
                    });
                    _unitOfWork.SaveChanges();

                    scope.Complete();
                    return true;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Create), ex);
                return false;
            }
        }

        public bool IsExisted(int userId, int groupId)
        {
            Group group = _repository.Get(_ => _.Id == groupId, _ => _.Members);
            Member member = _memberRepository.Get(_ => _.Id == userId);

            return group.Members.Contains(member);
        }

        public bool UpdateGroupInvitation(GroupInvitation groupInvitation)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    _groupInvitationRepository.Update(groupInvitation);
                    _unitOfWork.SaveChanges();

                    scope.Complete();
                    return true;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(UpdateGroupInvitation), ex);
                return false;
            }
        }

        public bool UserRemoved(int userId, int groupId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Member member = _memberRepository.Get(_ => _.Id == userId);
                    Group group = _repository.Get(_ => _.Id == groupId, _ => _.Members);

                    group.Members.Remove(member);

                    _repository.Update(group);
                    _unitOfWork.SaveChanges();

                    scope.Complete();
                    return true;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(UserRemoved), ex);
                return false;
            }
        }
    }
}