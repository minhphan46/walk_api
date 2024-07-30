using WalkProject.DataModels.Entities;

namespace WalkProject.API.RestFul.Repositories.Interfaces
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk, Guid categoryId);
        Task<List<Walk>> GetAllAsync(string filterOn = null, string filterQuery = null,
            string sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10);
        Task<Walk> GetByIdAsync(Guid id);
        Task<Walk> UpdateAsync(Guid id, Walk walk);
        Task<Walk> DeleteAsync(Guid id);
    }
}
