using JamunaBank.Procurement.API.DTOs;namespace JamunaBank.Procurement.API.Repositories.Interfaces;
public interface IRequisitionFormRepository{Task<ReadyRequisitionPageDto>GetReadyAsync(string?search,string?type,long?divisionId,DateOnly?from,DateOnly?to,int page,int size,CancellationToken ct);Task<RequisitionFormDto?>GetFormAsync(long id,string actor,CancellationToken ct);}
