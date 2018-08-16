namespace DependencyResolver
{
    using Ninject.Modules;
    using Ninject.Web.Common;
    using Core.ApplicationService.Database.Entities;
    using Core.ApplicationService.Database.Identities;
    using Core.ApplicationService.Database.Log;
    using Core.ObjectService.Repositories;
    using Infrastructure.Entity.Database;
    using Infrastructure.Entity.Repositories;
    using Infrastructure.Logging.Database;
    using Infrastructure.Identity.Database;

    public class InfrastructureModules : NinjectModule
    {
        public override void Load()
        {
            //factory
            Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            //context
            Bind<IEntityContext>().To<EntityContext>().InRequestScope();
            Bind<ILogContext>().To<LogContext>().InRequestScope();
            Bind<IIdentityContext>().To<IdentityContext>().InRequestScope();
        }
    }
}