using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Interfaces;
namespace JamunaBank.Procurement.API.Services.Implementations;
public sealed class AssetCategoryService(IAssetCategoryRepository repository) : IAssetCategoryService
{
    public async Task<IReadOnlyList<AssetCategoryDto>> GetAllAsync(bool? isActive, CancellationToken cancellationToken)
    {
        var rows = await repository.GetAllAsync(isActive == true, cancellationToken);
        return isActive.HasValue ? rows.Where(x => x.IsActive == isActive.Value).ToArray() : rows;
    }
    public async Task<AssetCategoryDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        return (await repository.GetAllAsync(false, cancellationToken)).SingleOrDefault(x => x.Id == id);
    }
}
