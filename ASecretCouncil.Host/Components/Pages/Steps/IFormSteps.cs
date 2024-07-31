using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Shared.Services;

namespace ASecretCouncil.Host.Components.Pages.Steps;

public interface IFormSteps
{
    FormStepViewModel? FindStep(string href);
    FormStepViewModel Head();
    FormStepViewModel Tail();
}

public class StaticFormSteps: IFormSteps
{
    private FormStepViewModel AcknowledgementStep { get; }

    private FormStepViewModel PersonalInfoStep { get; }

    private FormStepViewModel ResumeStep { get; }

    private FormStepViewModel SummaryStep { get; }

    private FormStepViewModel ConfirmationStep { get; }

    public StaticFormSteps(ReturningApplicationProvider savedAppProvider)
    {
        AcknowledgementStep = new FormStepViewModel(savedAppProvider)
        {
            CurrentStep = ApplicationStep.Start,
            Display = "Start",
            Href = "/acknowledgement"
        };
        PersonalInfoStep = new FormStepViewModel(savedAppProvider)
        {
            CurrentStep = ApplicationStep.PersonalInformation,
            Display = "Step 1",
            Href = "/personalInfo"
        };
        ResumeStep = new FormStepViewModel(savedAppProvider)
        {
            CurrentStep = ApplicationStep.Files,
            Display = "Step 2",
            Href = "/files"
        };
        SummaryStep = new FormStepViewModel(savedAppProvider)
        {
            CurrentStep = ApplicationStep.Summary,
            Display = "Step 3",
            Href = "/summary"
        };
        ConfirmationStep = new FormStepViewModel(savedAppProvider)
        {
            CurrentStep = ApplicationStep.Complete,
            Display = "Complete",
            Href = "/confirmation"
        };

        AcknowledgementStep.Previous = null;
        AcknowledgementStep.Next = PersonalInfoStep;

        PersonalInfoStep.Previous = AcknowledgementStep;
        PersonalInfoStep.Next = ResumeStep;

        ResumeStep.Previous = PersonalInfoStep;
        ResumeStep.Next = SummaryStep;

        SummaryStep.Previous = ResumeStep;
        SummaryStep.Next = ConfirmationStep;

        ConfirmationStep.Previous = SummaryStep;
        ConfirmationStep.Next = null;
    }

    public FormStepViewModel? FindStep(string href)
    {
        var currentStep = Head();
        while (currentStep != null)
        {
            if (currentStep.Href.EndsWith(href))
            {
                return currentStep;
            }

            currentStep = currentStep.Next;
        }

        return null;
    }

    public FormStepViewModel Head()
    {
        return AcknowledgementStep;
    }

    public FormStepViewModel Tail()
    {
        return ConfirmationStep;
    }
}
