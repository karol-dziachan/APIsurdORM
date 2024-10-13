using Microsoft.Extensions.Logging;
using Pr0t0k07.APIsurdORM.Application.Shared.Exceptions;
using Pr0t0k07.APIsurdORM.Application.Shared.Interfaces;

namespace Pr0t0k07.APIsurdORM.Infrastructure.Shared.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<IFileService> _logger;

        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
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
