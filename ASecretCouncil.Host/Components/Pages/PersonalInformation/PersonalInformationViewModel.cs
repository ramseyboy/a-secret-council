using Microsoft.AspNetCore.Components.Forms;

namespace ASecretCouncil.Host.Components.Pages.PersonalInformation;

public class PersonalInformationViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsValid { get; set; }
}
