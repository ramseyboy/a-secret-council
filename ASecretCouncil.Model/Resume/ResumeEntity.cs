using System.ComponentModel.DataAnnotations;
using ASecretCouncil.Model.Application;

namespace ASecretCouncil.Model.Resume
{
    public class ResumeEntity: IFileEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string ContentType { get; set; }
        public string FileExt { get; set; }
        public string Uri { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public long Size { get; set; }
        public Guid ApplicationId { get; set; }
        public ApplicationEntity Application { get; set; }
    }
}
