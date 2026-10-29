using DSW_3P_EJ01.Models;

namespace DSW_3P_EJ01.Services;

public interface IMisionService
{
    Task<IReadOnlyList<Mision>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Mision?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Mision> CreateAsync(Mision mision, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, Mision mision, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
