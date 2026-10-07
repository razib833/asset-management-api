using JamunaBank.Procurement.API.DTOs;
namespace JamunaBank.Procurement.API.Services.Interfaces;
public interface IAssetCategoryService
{
    Task<IReadOnlyList<AssetCategoryDto>> GetAllAsync(bool? isActive, CancellationToken cancellationToken);
    Task<AssetCategoryDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
}
