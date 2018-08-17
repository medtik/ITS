using Core.ApplicationService.Business.EntityService;
using Core.ApplicationService.Business.LogService;
using Core.ObjectModels.Entities;
using Core.ObjectService.Repositories;

namespace Service.Implement.Entity
{
    public class PhotoService : _BaseService<Photo>, IPhotoService
    {
        public PhotoService(ILoggingService loggingService, IUnitOfWork unitOfWork) : base(loggingService, unitOfWork)
        {
        }

        public string GetBase64(int photoId)
        {
            return _repository.Get(_ => _.Id == photoId).Path;
        }
    }
}
