using System.ComponentModel.DataAnnotations;
namespace JamunaBank.Procurement.API.DTOs;
public sealed record AssetSpecificationSetDto(long AssetId,IReadOnlyList<AssetSpecificationDto> Specifications);
public sealed record AssetSpecificationDto(long SpecId,string Name,string Type,bool IsMandatory,int DisplayOrder,bool IsActive,IReadOnlyList<AssetSpecificationOptionDto> Options);
public sealed record AssetSpecificationOptionDto(long OptionId,string Value,int DisplayOrder,bool IsActive);
public sealed class SaveAssetSpecificationRequest
{
 [Required,StringLength(150)] public string Name{get;init;}=string.Empty;
 [Required] public string Type{get;init;}=string.Empty;
 public bool IsMandatory{get;init;} public int DisplayOrder{get;init;} public bool IsActive{get;init;}=true;
}
public sealed class CreateAssetSpecificationOptionRequest
{
 [Required,StringLength(300)] public string Value{get;init;}=string.Empty;
 public int DisplayOrder{get;init;} public bool IsActive{get;init;}=true;
}
public sealed class UpdateAssetSpecificationOptionRequest
{
 [Range(1,long.MaxValue)] public long OptionId{get;init;}
 [Required,StringLength(300)] public string Value{get;init;}=string.Empty;
 public int DisplayOrder{get;init;} public bool IsActive{get;init;}=true;
}
