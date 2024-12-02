using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pr0t0k07.APIsurdORM.Application.Shared.Exceptions;
using Pr0t0k07.APIsurdORM.Application.Shared.Interfaces;
using Pr0t0k07.APIsurdORM.Application.Shared.Models;
using Pr0t0k07.APIsurdORM.Application.Workers;
using System.IO;

namespace Pr0t0k07.APIsurdORM.Infrastructure.Shared.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<IFileService> _logger;

        public FileService(ILogger<IFileService> logger)
        {
            _logger = logger ?? NullLogger<IFileService>.Instance;
        }

        public void DeleteContent(string filePath)
        {
            if(!File.Exists(filePath))
            {
                _logger.LogError("Cannot delete file {filePath}, because it's not exists", filePath);
                throw new FileServiceException($"Cannot delete file {filePath}, because it's not exists");        
            }

            File.WriteAllText(filePath, String.Empty);
            _logger.LogDebug("All content is written to file");
        }

        public void CreateDirectories(IEnumerable<string> paths)
        {
            if(!paths.Any())
            {
                _logger.LogInformation("Paths to create directories was empty");
                return;
            }

            foreach (string path in paths.OrderBy(x => x.Split("\\").Length))
            {
                if (Directory.Exists(path))
                {
                    ClearDirectory(path);
                    _logger.LogInformation("Directory {filePath} already exists, so it was cleared.", path);
                    continue; 
                }

                Directory.CreateDirectory(path);
            }

            _logger.LogInformation("Whole directories was created");
        }

        public void ClearDirectory(string filePath)
        {
            _logger.LogInformation("In directory was {count} items", 
                GetSubItems(Directory.GetDirectories(filePath, "*", SearchOption.AllDirectories)).Count() 
                + GetSubItems(Directory.GetFiles(filePath, "*", SearchOption.AllDirectories)).Count());

            Directory.Delete(filePath, true);
            Directory.CreateDirectory(filePath);
        }

        public DirectoryContentModel GetDirectoryResources(string filePath, IEnumerable<string> exclusions = null)
        {
            if (!Directory.Exists(filePath))
            {
                _logger.LogError("File {filePath} does not exist", filePath);
                throw new ArgumentException($"Directory {filePath} does not exist");
            }

            _logger.LogDebug("Getting directories from {filePath}", filePath);
            var directories = GetSubItems(Directory.GetDirectories(filePath, "*", SearchOption.AllDirectories));
            _logger.LogDebug("Get {count} directories", directories.Count());
            _logger.LogDebug("Getting files from {filePath}", filePath);
            var files = GetSubItems(Directory.GetFiles(filePath, "*", SearchOption.AllDirectories));
            _logger.LogDebug("Get {count} files", files.Count());

            return new() 
            { 
                Directories = directories.Where(dir => !exclusions.Any(exclusion => dir.Contains(exclusion, StringComparison.OrdinalIgnoreCase))).ToList(),
                Files = files.Where(dir => !exclusions.Any(exclusion => dir.Contains(exclusion, StringComparison.OrdinalIgnoreCase))).ToList(), 
            };
        }

        private IEnumerable<string> GetSubItems(string[] items)
        {
            foreach (var item in items)
            {
                yield return item; 
            }
        }

        public void CreateFiles(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                EnsureIfFileExists(file);
            }
        }

        public bool EnsureIfFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                var fStr = File.Create(filePath);
                fStr.Close();
                _logger.LogInformation("File {filePath} wasn't exists, so the service create it.", filePath);
                return false;
            }

            _logger.LogDebug("File {filePath} exists", filePath);
            return true;
        }

        public void ReplaceContent(string filePath, string content)
        {
            if (!File.Exists(filePath))
            {
                _logger.LogError("Cannot replace content in file {filePath}, because it's not exists", filePath);
                throw new FileServiceException($"Cannot delete file {filePath}, because it's not exists");
            }

            File.Delete(filePath);
            var fStr = File.Create(filePath);
            File.WriteAllText(filePath, content);
            fStr.Close();
            _logger.LogInformation("Successed replace content in file {filePath}", filePath);
        }

        public void WriteToFile(string filePath, string content)
        {
            if (!File.Exists(filePath))
            {
                _logger.LogError("Cannot write content to file {filePath}, because it's not exists", filePath);
                throw new FileServiceException($"Cannot delete file {filePath}, because it's not exists");
            }

            File.WriteAllText(filePath, content);
            _logger.LogInformation("Successed write all content to file {filePath}", filePath);
        }
    }
}
