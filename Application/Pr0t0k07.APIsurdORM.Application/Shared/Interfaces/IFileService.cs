namespace Pr0t0k07.APIsurdORM.Application.Shared.Interfaces
{
    public interface IFileService
    {
        void WriteToFile(string filePath, string content);
        void DeleteContent(string filePath);
        bool EnsureIfFileExists(string filePath);
        void ReplaceContent(string filePath, string content);
    }
}
