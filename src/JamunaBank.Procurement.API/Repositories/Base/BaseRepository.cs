using JamunaBank.Procurement.API.Data;

namespace JamunaBank.Procurement.API.Repositories.Base;

public abstract class BaseRepository(IStoredProcedureExecutor storedProcedures)
{
    protected IStoredProcedureExecutor StoredProcedures { get; } = storedProcedures;
}
