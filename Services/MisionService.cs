using DSW_3P_EJ01.Models;
using DSW_3P_EJ01.Repositories;

namespace DSW_3P_EJ01.Services;

public sealed class MisionService(IMisionRepository repository) : IMisionService
{
    public Task<IReadOnlyList<Mision>> GetAllAsync(CancellationToken cancellationToken = default) =>
        repository.GetAllAsync(cancellationToken);

    public Task<Mision?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        repository.GetByIdAsync(id, cancellationToken);

    public Task<Mision> CreateAsync(Mision mision, CancellationToken cancellationToken = default)
    {
        mision.Nombre = mision.Nombre.Trim();
        return repository.AddAsync(mision, cancellationToken);
    }

    public Task<bool> UpdateAsync(int id, Mision mision, CancellationToken cancellationToken = default)
    {
        mision.Id = id;
        mision.Nombre = mision.Nombre.Trim();
        return repository.UpdateAsync(mision, cancellationToken);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) =>
        repository.DeleteAsync(id, cancellationToken);
}
