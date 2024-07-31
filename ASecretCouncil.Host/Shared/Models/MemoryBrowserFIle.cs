using Microsoft.AspNetCore.Components.Forms;
using ASecretCouncil.Model.Resume;

namespace ASecretCouncil.Host.Shared.Models;

internal class MemoryBrowserFile : IBrowserFile
{
    private readonly FormFile _delegateFile;

    public MemoryBrowserFile(FileDto document, byte[] file)
    {
        var stream = new MemoryStream(file);
        _delegateFile = new FormFile(
            stream,
            0,
            file.Length,
            document.OriginalName,
            $"{document.OriginalName}.{document.FileExt}");

        Name = _delegateFile.Name;
        Size = _delegateFile.Length;
        ContentType = document.ContentType;
        LastModified = DateTimeOffset.Now;
    }

    public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = new())
    {
        //todo: handle maxallowedsize and token?
        return _delegateFile.OpenReadStream();
    }

    public string Name { get; }
    public DateTimeOffset LastModified { get; }
    public long Size { get; }
    public string ContentType { get; }
}
