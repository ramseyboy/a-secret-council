
namespace ASecretCouncil.Host.Components.Pages;

public class PrefillFormAfterRenderDelegate()
{
    public async Task OnAfterRenderAsync(IPrefillFormPresenterMixin mixin, bool firstRender)
    {
        if (firstRender)
        {
            await mixin.PrefillOnReturnVisit();
        }
    }
}
