using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ASecretCouncil.Host.Shared.Services;

public class LoadingService
{
    private readonly BehaviorSubject<bool> _loadingSubject = new(false);

    public void StartLoading()
    {
        _loadingSubject.OnNext(true);
    }

    public void StopLoading()
    {
        _loadingSubject.OnNext(false);
    }

    public IObservable<bool> IsLoading()
    {
        return _loadingSubject.AsObservable();
    }
}
