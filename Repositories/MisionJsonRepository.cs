using System.Text.Json;
using DSW_3P_EJ01.Models;

namespace DSW_3P_EJ01.Repositories;

public sealed class MisionJsonRepository : IMisionRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _filePath;
    private readonly SemaphoreSlim _fileLock = new(1, 1);

    public MisionJsonRepository(IWebHostEnvironment environment)
    {
        _filePath = Path.Combine(environment.ContentRootPath, "misiones.json");
    }

    public async Task<IReadOnlyList<Mision>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            return await ReadAllAsync(cancellationToken);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<Mision?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var misiones = await GetAllAsync(cancellationToken);
        return misiones.FirstOrDefault(mision => mision.Id == id);
    }

    public async Task<Mision> AddAsync(Mision mision, CancellationToken cancellationToken = default)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            var misiones = await ReadAllAsync(cancellationToken);
            var nuevaMision = new Mision
            {
                Id = misiones.Count == 0 ? 1 : misiones.Max(existing => existing.Id) + 1,
                Nombre = mision.Nombre,
                Dificultad = mision.Dificultad,
                RecompensaXP = mision.RecompensaXP,
                Completada = false
            };

            misiones.Add(nuevaMision);
            await WriteAllAsync(misiones, cancellationToken);
            return nuevaMision;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<bool> UpdateAsync(Mision mision, CancellationToken cancellationToken = default)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            var misiones = await ReadAllAsync(cancellationToken);
            var index = misiones.FindIndex(existing => existing.Id == mision.Id);
            if (index < 0)
            {
                return false;
            }

            misiones[index] = mision;
            await WriteAllAsync(misiones, cancellationToken);
            return true;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            var misiones = await ReadAllAsync(cancellationToken);
            var mision = misiones.FirstOrDefault(existing => existing.Id == id);
            if (mision is null)
            {
                return false;
            }

            misiones.Remove(mision);
            await WriteAllAsync(misiones, cancellationToken);
            return true;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task<List<Mision>> ReadAllAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath))
        {
            await File.WriteAllTextAsync(_filePath, "[]", cancellationToken);
        }

        await using var stream = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<List<Mision>>(stream, JsonOptions, cancellationToken)
            ?? throw new InvalidDataException($"El archivo de misiones '{_filePath}' contiene JSON nulo.");
    }

    private async Task WriteAllAsync(List<Mision> misiones, CancellationToken cancellationToken)
    {
        var temporaryPath = $"{_filePath}.tmp";
        await using (var stream = File.Create(temporaryPath))
        {
            await JsonSerializer.SerializeAsync(stream, misiones, JsonOptions, cancellationToken);
        }

        File.Move(temporaryPath, _filePath, overwrite: true);
    }
}
