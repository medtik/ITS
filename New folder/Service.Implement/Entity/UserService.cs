namespace Service.Implement.Entity
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using Core.ObjectModels.Entities;
    using Core.ObjectService.Repositories;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.LogService;
    using System.Transactions;

    public class UserService : _BaseService<User>, IUserService
    {
        private readonly IRepository<Member> _memberRepository;
        private readonly IRepository<Creator> _creatorRepository;
        private readonly IRepository<GroupInvitation> _groupInvitationRepository;

        public UserService(ILoggingService loggingService, IUnitOfWork unitOfWork) : base(loggingService, unitOfWork)
        {
            _memberRepository = unitOfWork.GetRepository<Member>();
            _creatorRepository = unitOfWork.GetRepository<Creator>();
            _groupInvitationRepository = unitOfWork.GetRepository<GroupInvitation>();
        }

        public int CreateUser(User entity)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    Creator creator = new Creator
                    {
                        FullName = entity.FullName,
                        Birthdate = entity.Birthdate,
                        Address = entity.Address
                    };
                    _creatorRepository.Create(creator);
                    _unitOfWork.SaveChanges();
                    scope.Complete();
                    return creator.Id;
                }//end scope
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Create), ex);
                return -1;
            }
        }

        public bool Delete(int userId)
        {
            return false;
        }

        public User Find(int userId, params Expression<Func<User, object>>[] includes)
        {
            User user = null;
            try
            {
                user = _repository.GetAsQueryable(_ => _.Id == userId, includes);
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Find), ex);
            }

            return user;
        }

        public IEnumerable<GroupInvitation> GetGroupInvitations(int userId, params Expression<Func<GroupInvitation, object>>[] includes)
        {
            return _groupInvitationRepository.Search(_ => _.UserId == userId, includes);
        }

        public IEnumerable<Group> GetGroups(int userId)
            => _memberRepository.Get(_ => _.Id == userId, 
                _ => _.Groups.Select(__ => __.Plans),
                _ => _.Groups.Select(__ => __.Members)).Groups;

        public IEnumerable<User> Search(Func<User, bool> searchValue, params Expression<Func<User, object>>[] includes)
        {
            return base._repository.Search(searchValue, includes);
        }

        public bool SetMobileToken(User user, string token)
        {
            user.MobileToken = token;
            return base.Update(user);
        }
    }
}
