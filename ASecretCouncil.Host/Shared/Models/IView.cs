namespace ASecretCouncil.Host.Shared.Models;

public interface IView
{
    public void TriggerRender();

    public void StartLoading();

    public void StopLoading();
}
