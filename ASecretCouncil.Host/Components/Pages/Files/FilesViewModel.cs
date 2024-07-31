using Microsoft.AspNetCore.Components.Forms;

namespace ASecretCouncil.Host.Components.Pages.Files;

public class FilesViewModel
{
    public IBrowserFile? SelectedFile { get; set; }
    public bool IsValid { get; set; }
}
