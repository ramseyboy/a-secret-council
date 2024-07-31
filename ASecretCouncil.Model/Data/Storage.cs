namespace ASecretCouncil.Model.Data;

internal static class Storage
{
    private const Environment.SpecialFolder UserCacheFolder = Environment.SpecialFolder.LocalApplicationData;

    private const string AppPathSegment = "ASecretCouncil";

    internal static string LocalSqlitePath =  Path.Join(Environment.GetFolderPath(UserCacheFolder), AppPathSegment, "Database");
    internal static string LocalResumeFilePath =  Path.Join(Environment.GetFolderPath(UserCacheFolder), AppPathSegment, "Resumes");

}
