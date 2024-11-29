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

                _logger.LogInformation("Succesfully get the templates.");

                await PrepareTemplatesFile(templates, entities);

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
            var entityNames = entities.Files.Select(GetFileNameFromPath).ToList();

            ReplaceProjectNameInFilePaths(templates);

            ReplaceEntityNameInFilePaths(templates, entityNames);
            
            ReplacesDestinationPath(templates);

            _fileService.CreateDirectories(templates.Directories);
            _fileService.CreateFiles(templates.Files);
        }

        private void ReplaceProjectNameInFilePaths(DirectoryContentModel templates)
        {
            ReplaceInList(templates.Directories, ReplacePatterns.ProjectNamePattern, __PROJECT_NAME__);
            ReplaceInList(templates.Files, ReplacePatterns.ProjectNamePattern, __PROJECT_NAME__);
        }        
        
        private void ReplacesDestinationPath(DirectoryContentModel templates)
        {
            ReplaceInList(templates.Directories, TEMPLATES_PATH, DESTINATION_PATH);
            ReplaceInList(templates.Files, TEMPLATES_PATH, DESTINATION_PATH);
        }

        private void ReplaceEntityNameInFilePaths(DirectoryContentModel templates, List<string> entityNames)
        {
            ReplaceInListAFewValues(templates.Directories, ReplacePatterns.EntityPattern, entityNames);
            ReplaceInListAFewValues(templates.Files, ReplacePatterns.EntityPattern, entityNames);
        }

        private static void ReplaceInListAFewValues(List<string> toReplaceList, string replacePattern, List<string> newValues)
        {
            var itemsWhichContainsPattern = toReplaceList.Where(x => x.Contains(replacePattern)); 
            var tempList = toReplaceList.Where(x => !x.Contains(replacePattern)).ToList(); 

            var replacedItems = itemsWhichContainsPattern.SelectMany(dir => newValues.Select(newVal => dir.Replace(replacePattern, newVal))); //n^2
            tempList = tempList.Concat(replacedItems).ToList(); 

            toReplaceList.Clear();
            toReplaceList.AddRange(tempList);
        }

        private static void ReplaceInList(List<string> toReplaceList, string replacePattern, string newValue)
        {
            for (int i = 0; i < toReplaceList.Count; i++)
            {
                toReplaceList[i] = toReplaceList[i].Replace(replacePattern, newValue);
            }
        }

        private string GetFileNameFromPath(string filePath)
            => filePath.Split("\\").Last()[..^3]; //trim '.cs'

        private async Task Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
