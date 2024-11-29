using Pr0t0k07.APIsurdORM.Application.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pr0t0k07.APIsurdORM.Application.Shared.Models;
using Pr0t0k07.APIsurdORM.Application.Shared;

namespace Pr0t0k07.APIsurdORM.Application.Workers
{
    public class GenerateApplication
    {
        #region DEBUG
        private readonly string TEMPLATES_PATH = "C:\\source\\APIsurdORM\\Templates";
        private readonly string DESTINATION_PATH = @"C:\temp\generate";
        private readonly string ENTITIES_DIR_PATH = @"C:\source\APIsurdORM\Examples\Pr0t0k07.APIsurdORM.Examples\Entities\";
        private readonly string __PROJECT_NAME__ = "Pr0t0k07.GenerateExamples";
        #endregion

        private readonly ILogger<GenerateApplication> _logger;
        private readonly IFileService _fileService;

        public GenerateApplication(ILogger<GenerateApplication> logger, IFileService fileService)
        {
            _logger = logger ?? NullLogger<GenerateApplication>.Instance;
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));  
        }

        public async Task Handle()
        {
            try
            {
                _logger.LogInformation("Start generating.");
                _logger.LogInformation("Get files and directories from templates.");

                var templates = _fileService.GetDirectoryResources(TEMPLATES_PATH, new List<string>() { @"\bin", @"\obj" });
                var entities = _fileService.GetDirectoryResources(ENTITIES_DIR_PATH, new List<string>() { @"\bin", @"\obj" });

                await PrepareTemplatesFile(templates, entities);

                _logger.LogInformation("Succesfully get the templates.");

                //_fileService.CreateDirectories(templates.Directories);

                _logger.LogInformation("Succesfully creating the templates.");


            }
            catch(Exception ex)
            {
                _logger.LogError("There was any error durring generate application. Message: {ex}", ex.ToString());
                Rollback();
                _logger.LogInformation("Rollback was done");
            }
        }

        private async Task PrepareTemplatesFile(DirectoryContentModel templates, DirectoryContentModel entities)
        {
            var entityNames = entities.Files.Select(GetFileNameFromPath);

            /*
                Replaced project name
             */
            templates.Directories = templates.Directories.Select(x => x.Replace(ReplacePatterns.ProjectNamePattern, __PROJECT_NAME__)).ToList();
            templates.Files = templates.Files.Select(x => x.Replace(ReplacePatterns.ProjectNamePattern, __PROJECT_NAME__)).ToList();

            /*
                Replaced {{entity}} in directories
             */
            var entitiesDir = templates.Directories.Where(x => x.Contains(ReplacePatterns.EntityPattern));
            templates.Directories = templates.Directories.Where(x => !x.Contains(ReplacePatterns.EntityPattern)).ToList();

            var replacedDirs = entitiesDir.SelectMany(dir => entityNames.Select(entityName => dir.Replace(ReplacePatterns.EntityPattern, entityName)));
            templates.Directories = templates.Directories.Concat(replacedDirs).ToList();


            /*
                Replaced {{entity}} in files
             */
            var entitiesFile = templates.Files.Where(x => x.Contains(ReplacePatterns.EntityPattern));
            templates.Files = templates.Files.Where(x => !x.Contains(ReplacePatterns.EntityPattern)).ToList();

            var replacedfiles = entitiesFile.SelectMany(dir => entityNames.Select(entityName => dir.Replace(ReplacePatterns.EntityPattern, entityName)));
            templates.Files = templates.Files.Concat(replacedfiles).ToList();


            /*
             * Replaced path to dest
             * 
            */

            templates.Directories = templates.Directories.Select(x => x.Replace(TEMPLATES_PATH, DESTINATION_PATH)).ToList();
            templates.Files = templates.Files.Select(x => x.Replace(TEMPLATES_PATH, DESTINATION_PATH)).ToList();

            _fileService.CreateDirectories(templates.Directories);
            _fileService.CreateFiles(templates.Files);


            Console.WriteLine("Debuig");
        }

        private string GetFileNameFromPath(string filePath)
            => filePath.Split("\\").Last()[..^3]; //trim '.cs'

        private async Task Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
