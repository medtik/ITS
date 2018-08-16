using Service;

namespace DependencyResolver
{
    using Ninject.Modules;
    using Core.ApplicationService.Business.EntityService;
    using Core.ApplicationService.Business.IdentityService;
    using Core.ApplicationService.Business.LogService;
    using Core.ApplicationService.Business.PagingService;
    using Service.Implement.Entity;
    using Service.Implement.Pagination;
    using Infrastructure.Identity.Adapter;
    using Infrastructure.Logging.Service;
    using Core.ApplicationService.Business.Algorithm;
    using Service.Implement.Algorithm;
    using Service.Implement.Mail;
    using Core.ApplicationService.Business.MailService;

    public class ServiceModules : NinjectModule
    {
        public override void Load()
        {
            //entity
            Bind<IUserService>().To<UserService>();
            Bind<ITagService>().To<TagService>();
            Bind<ILocationService>().To<LocationService>();
            Bind<IAnswerService>().To<AnswerService>();
            Bind<IAreaService>().To<AreaService>();
            Bind<IQuestionService>().To<QuestionService>();
            Bind<IPlanService>().To<PlanService>();
            Bind<IGroupService>().To<GroupService>();

            //identity
            Bind<IIdentityService>().To<IdentityService>();

            //pagination
            Bind<IPagingService>().To<PagingService>();

            //log
            Bind<ILoggingService>().To<LoggingService>();

            //algorithm
            Bind<ISearchTreeService>().To<SearchTreeService>();

            //Mail
            Bind<IMailService>().To<MailService>();
        }
    }
}