using ASecretCouncil.Model.Person;
using ASecretCouncil.Model.Resume;
using Microsoft.AspNetCore.Components.Forms;

namespace ASecretCouncil.Model.Application;

public class ApplicationDto
{
    public Guid Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime SubmitDate { get; set; }

    public PersonDto? Person { get; set; }

    public FileDto? Resume { get; set; }
}
