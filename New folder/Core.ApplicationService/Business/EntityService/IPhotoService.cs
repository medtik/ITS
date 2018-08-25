using Core.ObjectModels.Entities;
using System.Threading.Tasks;

namespace Core.ApplicationService.Business.EntityService
{
    public interface IPhotoService
    {
        string GetBase64(int photoId);

        Task<bool> Create2(Photo photo);

        bool Update(Photo photo);
    }
}
