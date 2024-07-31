namespace ASecretCouncil.Host.Shared.Models;

public interface IPresenter<in TVIew>: IDisposable where TVIew: IView
{
    public Task InitializeAsync(TVIew view);
}
