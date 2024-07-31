using System.ComponentModel.DataAnnotations;
using ASecretCouncil.Model.Application;

namespace ASecretCouncil.Model.Person;

public class PersonEntity
{
    [Key]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Guid ApplicationId { get; set; }
    public ApplicationEntity Application { get; set; }
}
