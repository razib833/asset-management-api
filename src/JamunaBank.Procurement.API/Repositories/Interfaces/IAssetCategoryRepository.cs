using JamunaBank.Procurement.API.DTOs;
namespace JamunaBank.Procurement.API.Repositories.Interfaces;
public interface IAssetCategoryRepository { Task<IReadOnlyList<AssetCategoryDto>> GetAllAsync(bool activeOnly, CancellationToken cancellationToken); }
