using JamunaBank.Procurement.API.DTOs;
using JamunaBank.Procurement.API.Repositories.Interfaces;
using JamunaBank.Procurement.API.Services.Implementations;

namespace JamunaBank.Procurement.API.Tests.Services;

public static class CommonLookupServiceTests
{
    public static void CategoryIsNormalized()
    {
        var repository = new FakeCommonLookupRepository();
        var service = new CommonLookupService(repository);

        service.GetByCategoryAsync("  priority  ", CancellationToken.None).GetAwaiter().GetResult();

        TestAssert.Equal("PRIORITY", repository.LastCategory!);
    }

    public static void InvalidCategoryIsRejected()
    {
        var service = new CommonLookupService(new FakeCommonLookupRepository());

        TestAssert.Throws<ArgumentException>(() => service.GetByCategoryAsync(" ", CancellationToken.None).GetAwaiter().GetResult());
        TestAssert.Throws<ArgumentException>(() => service.GetByCategoryAsync(new string('X', 51), CancellationToken.None).GetAwaiter().GetResult());
    }

    private sealed class FakeCommonLookupRepository : ICommonLookupRepository
    {
        public string? LastCategory { get; private set; }

        public Task<IReadOnlyList<CommonLookupDto>> GetByCategoryAsync(string category, CancellationToken cancellationToken)
        {
            LastCategory = category;
            return Task.FromResult<IReadOnlyList<CommonLookupDto>>([]);
        }
    }
}
