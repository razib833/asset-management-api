namespace JamunaBank.Procurement.API.Configuration;

public sealed class DatabaseOptions
{
    public const string ConnectionStringName = "ProcurementDb";
    public string ConnectionString { get; init; } = string.Empty;
    public int CommandTimeoutSeconds { get; init; } = 30;
}
