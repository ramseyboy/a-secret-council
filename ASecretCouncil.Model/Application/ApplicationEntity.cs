using System.ComponentModel.DataAnnotations;
using ASecretCouncil.Model.Person;
using ASecretCouncil.Model.Resume;

namespace ASecretCouncil.Model.Application;

public class ApplicationEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime SubmitDate { get; set; }

    public PersonEntity? Person { get; set; }

    public ResumeEntity? Resume { get; set; }
}
