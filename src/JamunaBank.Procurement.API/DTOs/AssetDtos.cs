using System.ComponentModel.DataAnnotations;

namespace JamunaBank.Procurement.API.DTOs;

public sealed record AssetCategoryDto(long Id, string Code, string Name, string? Description, bool IsActive);

public sealed record AssetDto(
    long Id, long AssetCategoryId, string? CategoryName, string Code, string Name,
    string? Description, string UnitOfMeasurement, int? DefaultWarranty, int? UsefulLife,
    decimal? CapitalizationThreshold, string Criticality, bool IsInventoryItem,
    bool IsDepreciable, bool IsActive);

public sealed class CreateAssetRequest
{
    [Range(1, long.MaxValue)] public long AssetCategoryId { get; init; }
    [Required, StringLength(30)] public string AssetCode { get; init; } = string.Empty;
    [Required, StringLength(200)] public string AssetName { get; init; } = string.Empty;
    [StringLength(1000)] public string? Description { get; init; }
    [Required, StringLength(50)] public string UnitOfMeasurement { get; init; } = string.Empty;
    [Range(0, int.MaxValue)] public int? DefaultWarranty { get; init; }
    [Range(1, int.MaxValue)] public int? UsefulLife { get; init; }
    [Range(typeof(decimal), "0", "79228162514264337593543950335")] public decimal? CapitalizationThreshold { get; init; }
    [Required] public string Criticality { get; init; } = "MEDIUM";
    public bool IsInventoryItem { get; init; } = true;
    public bool IsDepreciable { get; init; } = true;
}

public sealed class UpdateAssetRequest
{
    [Range(1, long.MaxValue)] public long AssetCategoryId { get; init; }
    [Required, StringLength(30)] public string AssetCode { get; init; } = string.Empty;
    [Required, StringLength(200)] public string AssetName { get; init; } = string.Empty;
    [StringLength(1000)] public string? Description { get; init; }
    [Required, StringLength(50)] public string UnitOfMeasurement { get; init; } = string.Empty;
    [Range(0, int.MaxValue)] public int? DefaultWarranty { get; init; }
    [Range(1, int.MaxValue)] public int? UsefulLife { get; init; }
    [Range(typeof(decimal), "0", "79228162514264337593543950335")] public decimal? CapitalizationThreshold { get; init; }
    [Required] public string Criticality { get; init; } = "MEDIUM";
    public bool IsInventoryItem { get; init; } = true;
    public bool IsDepreciable { get; init; } = true;
    public bool IsActive { get; init; } = true;
}
