using System.ComponentModel.DataAnnotations;
using System.Text;
using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Properties;
using ASecretCouncil.Host.Shared.Models;
using ASecretCouncil.Host.Shared.Services;
using ASecretCouncil.Model.Application;
using ASecretCouncil.Model.Person;
using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Exceptions_ApplicationException = ASecretCouncil.Host.Shared.Exceptions.ApplicationException;

namespace ASecretCouncil.Host.Components.Pages.PersonalInformation;

internal sealed class DefaultPersonalInformationPresenter(
    ReturningApplicationProvider savedApplicationProvider,
    IApplicationService applicationService,
    FormStepRoutingPresenter formStepPresenterDelegate,
    IMapper mapper)
    : IPersonalInformationPresenter
{
    public PersonalInformationViewModel ViewModel { get; private set; } = new();

    public Func<string, string?> EmailValidationFunc =>
        delegate(string email)
        {
            try
            {
                new EmailAddressAttribute().Validate(email, "");
                return null;
            }
            catch (ValidationException)
            {
                return Strings.Validation_InvalidEmailMessage;
            }
        };

    public Func<string, string?> PhoneValidationFunc =>
        delegate(string phone)
        {
            string digits = new(phone.Where(char.IsDigit).ToArray());
            if (digits.Length == 10)
            {
                return null;
            }

            return Strings.Validation_InvalidPhoneLengthMessage;
        };

    private IPersonalInformationView? _view;

    public async Task InitializeAsync(IPersonalInformationView view)
    {
        _view = view;
        await formStepPresenterDelegate.InitializeAsync(view);
    }

    public async Task PrefillOnReturnVisit()
    {
        if (_view == null)
        {
            throw new Exceptions_ApplicationException(
                "Presenter cannot be called with initializing view");
        }

        _view.StartLoading();

        var savedApplicationId = await savedApplicationProvider.ApplicationId();

        if (savedApplicationId == Guid.Empty)
        {
            //todo: route to start
            _view.StopLoading();
            return;
        }

        var savedApplication = await applicationService.FindApplication(savedApplicationId);
        if (savedApplication?.Person != null)
        {
            ViewModel = mapper.Map(savedApplication.Person, ViewModel);

            _view.TriggerRender();
            await _view.Validate();
        }

        _view.StopLoading();
    }

    public async Task OnNextClicked()
    {
        if (!ViewModel.IsValid)
        {
            return;
        }

        _view?.StartLoading();
        var savedApplicationId = await savedApplicationProvider.ApplicationId();
        if (savedApplicationId == Guid.Empty)
        {
            //todo: route to start
            _view?.StopLoading();
            return;
        }

        var savedApplication = await applicationService.FindApplication(savedApplicationId);

        var personDto = mapper.Map<PersonalInformationViewModel, PersonDto>(ViewModel);
        if (savedApplication?.Person is null)
        {
            await applicationService.CreatePerson(savedApplicationId, personDto);
        }
        else
        {
            await applicationService.UpdatePerson(savedApplicationId, personDto);
        }

        await savedApplicationProvider.SaveApplicationId(savedApplicationId);
        await formStepPresenterDelegate.OnNextClicked();

        _view?.StopLoading();
    }

    public async Task OnPreviousClicked()
    {
        await formStepPresenterDelegate.OnPreviousClicked();
    }

    public void Dispose()
    {
        _view = null;
    }
}
