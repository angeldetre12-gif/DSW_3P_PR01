using DSW_3P_EJ01.Models;

namespace DSW_3P_EJ01.Repositories;

public interface IMisionRepository
{
    Task<IReadOnlyList<Mision>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Mision?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Mision> AddAsync(Mision mision, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Mision mision, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
