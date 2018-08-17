using Core.ObjectModels.Entities;

namespace Core.ApplicationService.Business.EntityService
{
    public interface IPhotoService
    {
        string GetBase64(int photoId);

        bool Create(Photo photo);

        bool Update(Photo photo);
    }
}
