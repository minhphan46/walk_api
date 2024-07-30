using WalkProject.DataModels.Entities;

namespace WalkProject.API.RestFul.Repositories.Interfaces
{
    public interface IDifficultyRepository
    {
        Task<List<Difficulty>> GetAllAsync();

        Task<Difficulty> GetByIdAsync(Guid id);

        Task<Difficulty> CreateAsync(Difficulty difficulty);

        Task<Difficulty> UpdateAsync(Guid id, Difficulty difficulty);

        Task<Difficulty> DeleteAsync(Guid id);
    }
}
