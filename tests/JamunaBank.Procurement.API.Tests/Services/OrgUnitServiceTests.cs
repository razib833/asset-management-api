using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Implementations;

namespace JamunaBank.Procurement.API.Tests.Services;

public static class OrgUnitServiceTests
{
    public static void PagingAndTypeAreApplied()
    {
        var repository = new FakeOrgUnitRepository(CreateUnits());
        var service = new OrgUnitService(repository);

        var result = service.GetAsync("Branch", 2, 1, CancellationToken.None).GetAwaiter().GetResult();

        TestAssert.Equal("BRANCH", repository.LastType!);
        TestAssert.True(repository.LastActiveOnly);
        TestAssert.Equal(2, result.Page);
        TestAssert.Equal(1, result.PageSize);
        TestAssert.Equal(3, result.TotalCount);
        TestAssert.Equal(3, result.TotalPages);
        TestAssert.Equal(2L, result.Items.Single().Id);
    }

    public static void IdLookupUsesOrganizationProcedureResults()
    {
        var repository = new FakeOrgUnitRepository(CreateUnits());
        var service = new OrgUnitService(repository);

        var found = service.GetByIdAsync(2, CancellationToken.None).GetAwaiter().GetResult();
        var missing = service.GetByIdAsync(99, CancellationToken.None).GetAwaiter().GetResult();

        TestAssert.Equal(2L, found!.Id);
        TestAssert.True(missing is null);
        TestAssert.True(repository.LastActiveOnly);
    }

    public static void InvalidFiltersAndPagingAreRejected()
    {
        var service = new OrgUnitService(new FakeOrgUnitRepository(CreateUnits()));

        TestAssert.Throws<ArgumentException>(() => service.GetAsync("Region", 1, 50, CancellationToken.None).GetAwaiter().GetResult());
        TestAssert.Throws<ArgumentOutOfRangeException>(() => service.GetAsync(null, 0, 50, CancellationToken.None).GetAwaiter().GetResult());
        TestAssert.Throws<ArgumentOutOfRangeException>(() => service.GetAsync(null, 1, 201, CancellationToken.None).GetAwaiter().GetResult());
        TestAssert.Throws<ArgumentOutOfRangeException>(() => service.GetByIdAsync(0, CancellationToken.None).GetAwaiter().GetResult());
    }

    private static IReadOnlyList<OrgUnitDto> CreateUnits() =>
    [
        new(1, null, "B01", "Branch One", "BRANCH", true),
        new(2, null, "B02", "Branch Two", "BRANCH", true),
        new(3, null, "B03", "Branch Three", "BRANCH", true)
    ];

    private sealed class FakeOrgUnitRepository(IReadOnlyList<OrgUnitDto> units) : IOrgUnitRepository
    {
        public string? LastType { get; private set; }
        public bool LastActiveOnly { get; private set; }

        public Task<IReadOnlyList<OrgUnitDto>> GetAllAsync(string? type, bool activeOnly, CancellationToken cancellationToken)
        {
            LastType = type;
            LastActiveOnly = activeOnly;
            return Task.FromResult(units);
        }
    }
}
