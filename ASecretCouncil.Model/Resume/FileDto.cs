namespace ASecretCouncil.Model.Resume;

public class FileDto
{
    public string Name { get; set; }
    public string OriginalName { get; set; }
    public string ContentType { get; set; }
    public string FileExt { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public long Size { get; set; }
}
