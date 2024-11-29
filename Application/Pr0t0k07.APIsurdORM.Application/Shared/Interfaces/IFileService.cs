using Pr0t0k07.APIsurdORM.Application.Shared.Models;

namespace Pr0t0k07.APIsurdORM.Application.Shared.Interfaces
{
    public interface IFileService
    {
        void WriteToFile(string filePath, string content);
        void DeleteContent(string filePath);
        bool EnsureIfFileExists(string filePath);
        void CreateFiles(IEnumerable<string> files);
        void ReplaceContent(string filePath, string content);
        DirectoryContentModel GetDirectoryResources(string filePath, IEnumerable<string> exclusions = null);
        void CreateDirectories(IEnumerable<string> paths); 
        void ClearDirectory(string filePath);
    }
}
