using WalkProject.DataModels.Entities;

namespace WalkProject.API.RestFul.Repositories.Interfaces
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
