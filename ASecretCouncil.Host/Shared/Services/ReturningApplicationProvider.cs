using ASecretCouncil.Model.Application;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ASecretCouncil.Host.Shared.Services;

public class ReturningApplicationProvider
{
    public Func<Task<Guid>> ApplicationId { get; }
    private Func<Task<DateTime?>> Expires { get; }

    private const string ApplicationIdKey = "ApplicationId";
    private const string ExpiresKey = "Expires";
    private const double Ttl = 24;

    private readonly ProtectedLocalStorage _localStorage;
    private readonly IApplicationService _applicationService;

    public ReturningApplicationProvider(ProtectedLocalStorage localStorage, IApplicationService applicationService)
    {
        _localStorage = localStorage;
        _applicationService = applicationService;

        ApplicationId = GetAsyncApplicationId;
        Expires = GetAsyncExpires;
    }

    public async Task SaveApplicationId(Guid applicationId)
    {
        await _localStorage.SetAsync(ApplicationIdKey, applicationId);
        await _localStorage.SetAsync(ExpiresKey, DateTime.Now.AddHours(Ttl));
    }

    public async Task DeleteApplicationId()
    {
        await _localStorage.DeleteAsync(ApplicationIdKey);
        await _localStorage.DeleteAsync(ExpiresKey);
    }

    private Func<Task<Guid>> GetAsyncApplicationId =>
        async delegate
        {
            var result = await _localStorage.GetAsync<Guid>(ApplicationIdKey);

            var expires = await Expires();

            var savedApplication = await _applicationService.ApplicationExists(result.Success ? result.Value : Guid.Empty);

            if (!savedApplication || expires == null || expires < DateTime.Now.AddSeconds(-1))
            {
                await _localStorage.DeleteAsync(ApplicationIdKey);
                await _localStorage.DeleteAsync(ExpiresKey);
                return Guid.Empty;
            }

            return result.Value;
        };

    private Func<Task<DateTime?>> GetAsyncExpires =>
        async delegate
        {
            var result = await _localStorage.GetAsync<DateTime?>(ExpiresKey);
            return result is {Success: true, Value: not null} ? result.Value : null;
        };
}
