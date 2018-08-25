using Core.ApplicationService.Business.EntityService;
using Core.ApplicationService.Business.LogService;
using Core.ObjectModels.Entities;
using Core.ObjectService.Repositories;
using Firebase.Storage;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

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

        public async Task<bool> Create2(Photo entity)
        {
            try
            {
                var stream = ConvertToStream(entity.Path);
                var task = new FirebaseStorage("its-g8.appspot.com")
                 .Child("photo")
                 .Child($"{Guid.NewGuid()}.jpg")
                 .PutAsync(stream);

                entity.Path = await task;
                _repository.Create(entity);
                _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _loggingService.Write(GetType().Name, nameof(Create), ex);
                return false;
            }
        }

        protected MemoryStream ConvertToStream(string base64)
        {
            var imageParts = base64.Split(',').ToList<string>();
            byte[] bytes = Convert.FromBase64String(imageParts[1]);
            MemoryStream memoryStream = new MemoryStream(bytes);
            return memoryStream;
        }
    }
}
