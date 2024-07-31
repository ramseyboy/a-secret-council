using ASecretCouncil.Host.Components.Pages;
using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Models;
using ASecretCouncil.Host.Shared.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Components;
using Xunit.Abstractions;

namespace ASecretCouncil.Tests.Pages;

public sealed class DefaultFormRoutingPresenterTest: IAsyncLifetime
{
    private readonly IFormRoutingPresenter _presenter;
    private readonly IView _mockView;

    public DefaultFormRoutingPresenterTest(ITestOutputHelper testOutputHelper)
    {
        var mockNavigationManager = A.Fake<NavigationManager>();
        var mockSavedApplicationProvider = A.Fake<ReturningApplicationProvider>();
        var formSteps = new StaticFormSteps(mockSavedApplicationProvider);
        var mockStepService = A.Fake<IApplicationStepService>();

        _mockView = A.Fake<IView>();
        _presenter = new DefaultFormRoutingPresenter(mockNavigationManager, formSteps, mockSavedApplicationProvider, mockStepService);
    }

    [Fact]
    public async Task TestPresenterRouting()
    {
        await _presenter.InitializeAsync(_mockView);
        Assert.True(_presenter != null);
    }


    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _presenter.Dispose();
        return Task.CompletedTask;
    }
}
