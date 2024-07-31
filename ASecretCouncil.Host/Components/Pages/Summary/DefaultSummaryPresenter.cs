using ASecretCouncil.Host.Components.Pages.PersonalInformation;
using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Models;
using ASecretCouncil.Host.Shared.Services;
using ASecretCouncil.Model.Application;
using AutoMapper;

namespace ASecretCouncil.Host.Components.Pages.Summary;

internal sealed class DefaultSummaryPresenter(ReturningApplicationProvider savedApplicationProvider,
    IApplicationService applicationService,
    FormStepRoutingPresenter formStepPresenterDelegate,
    IMapper mapper): ISummaryPresenter
{
    public SummaryViewModel ViewModel { get; private set; } = new();

    private IView? _view;
    public async Task InitializeAsync(IView view)
    {
        _view = view;
        await formStepPresenterDelegate.InitializeAsync(view);
    }

    public void Dispose()
    {
        _view = null;
    }

    public async Task OnNextClicked()
    {
        _view?.StartLoading();

        var savedApplicationId = await savedApplicationProvider.ApplicationId();

        if (savedApplicationId == Guid.Empty)
        {
            //todo: reset and route to start
            _view?.StopLoading();
            return;
        }

        await applicationService.SubmitApplication(savedApplicationId);

        await savedApplicationProvider.DeleteApplicationId();

        await formStepPresenterDelegate.OnNextClicked();
        _view?.StopLoading();
    }

    public async Task OnPreviousClicked()
    {
        await formStepPresenterDelegate.OnPreviousClicked();
    }

    public async Task PrefillOnReturnVisit()
    {
        _view?.StartLoading();

        var savedApplicationId = await savedApplicationProvider.ApplicationId();

        if (savedApplicationId == Guid.Empty)
        {
            //todo: route to start
            _view?.StopLoading();
            return;
        }

        var savedApplication = await applicationService.FindApplication(savedApplicationId);
        if (savedApplication != null)
        {
            ViewModel = mapper.Map<ApplicationDto, SummaryViewModel>(savedApplication);

            _view?.TriggerRender();
        }

        _view?.StopLoading();
    }
}
