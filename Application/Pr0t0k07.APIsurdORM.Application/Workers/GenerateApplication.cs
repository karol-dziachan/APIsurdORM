using Pr0t0k07.APIsurdORM.Application.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pr0t0k07.APIsurdORM.Application.Shared.Models;
using Pr0t0k07.APIsurdORM.Application.Shared;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Text;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;

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
        private readonly ISyntaxProvider _syntaxProvider;
        private Dictionary<string, string> _replaceDict;

        public GenerateApplication(ILogger<GenerateApplication> logger, IFileService fileService, ISyntaxProvider syntaxProvider)
        {
            _logger = logger ?? NullLogger<GenerateApplication>.Instance;
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _syntaxProvider = syntaxProvider ?? throw new ArgumentNullException(nameof(syntaxProvider));
            _replaceDict = new Dictionary<string, string>()
            {
                {"__ProjectName__" , __PROJECT_NAME__ },
                {"__Entity__" , "" },
                {"__Entities__" , ""},
            };
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

                var entitiesClasses = GetListOfEntites(entities.Files);

                _logger.LogInformation("Succesfully collecting infos about classes.");

                await PrepareTemplatesFile(templates, entities, entitiesClasses);

                _logger.LogInformation("Succesfully creating the templates.");

                WriteTemplates(templates.Files, entities.Files, templates.Directories.FirstOrDefault(x => x.Contains(".Domain") && x.EndsWith("\\Entities")), entitiesClasses);
            }
            catch (Exception ex)
            {
                _logger.LogError("There was any error durring generate application. Message: {ex}", ex.ToString());
                Rollback();
                _logger.LogInformation("Rollback was done");
            }
        }

        private string GetSourceFileFormTemplatePath(string path, string entityName, string entityNameInPlural)
        {
            Dictionary<string, string> replacePathsDict = new()
            {
                    {$"{__PROJECT_NAME__}" , "{{ProjectName}}" },
                    {DESTINATION_PATH, TEMPLATES_PATH }
            };

            if (!string.IsNullOrEmpty(entityName)) 
            {
                replacePathsDict.Add($"{entityName}", "{{Entity}}");
            }

            if (!string.IsNullOrEmpty(entityNameInPlural) && entityName != entityNameInPlural)
            {
                replacePathsDict.Add($"{entityNameInPlural}", "{{Entities}}" );
            }

            foreach (var replaceItem in replacePathsDict)
            {
                path = path.Replace(replaceItem.Key, replaceItem.Value);
            }

            return path;
        }

        private string ReplaceFileContent(string filePath, Dictionary<string, string> replaceDict)
        {
            var sourceContent = File.ReadAllText(filePath);

            foreach (var replaceItem in replaceDict)
            {
                sourceContent = sourceContent.Replace(replaceItem.Key, replaceItem.Value);
            }

            return sourceContent;
        }

        private void WriteTemplates(List<string> templatesPath, List<string> entitiesPath, string entitiesDir, List<ClassModel> entitiesClasses)
        {
            foreach (var path in templatesPath)
            {
                Dictionary<string, string> replaceDict = new()
                {
                    {"__ProjectName__" , __PROJECT_NAME__ },
                    {"{{ProjectName}}" , __PROJECT_NAME__ },
                    {"__Entity__" , $"" },
                    {"__Entities__" , $""},
                };
                var entityClassName = entitiesClasses.Where(entity => path.Contains($"{entity.ClassName}")).FirstOrDefault();

                if(entityClassName is not null)
                {
                    var primaryKeyAttr = entityClassName.Properties.FirstOrDefault(x => x.Attributes.Any(y => y.AttributeName == "PrimaryKey")).PropertyName;

                    replaceDict["__Entity__"] = entityClassName.ClassName;
                    var entityNameInPlural = entityClassName.Attributes.FirstOrDefault(x => x.AttributeName == "PluralNameEntity")?.AttributeValues.FirstOrDefault()
                            ?? entityClassName.ClassName;
                    replaceDict["__Entities__"] = Regex.Replace(entityNameInPlural, @"[^\p{L}-\s]+", "");

                    var properties = entityClassName.Properties.Where(x => !x.Attributes.Any(x => x.AttributeName == "AutoNumerated"));
                    replaceDict.Add("__ENTITY_PARAMETERS__", string.Join(", ", properties.Select(x => x.PropertyName)));
                    replaceDict.Add("__ENTITY_VALUES__", string.Join(", ", properties.Select(x => $"@{x.PropertyName}")));
                    
                    replaceDict.Add("//__ENTITY_PARAMETERS_MAPPING__", string.Join(", ", properties.Select(x => $"{x.PropertyName} = entity.{x.PropertyName}")));

                    var relatedProperties = entityClassName.Properties.Where(x => x.Attributes.Any(x => x.AttributeName == "ForeignKey"));
                    if (!relatedProperties.Any())
                    {
                        replaceDict.Add("__LEFT_JOIN__", String.Empty);
                    }
                    else
                    {
                        replaceDict.Add("__LEFT_JOIN__", string.Join("\n", relatedProperties.Select(x => $"LEFT JOIN [dbo].[{x.Attributes.First(x => x.AttributeName == "ForeignKey")!.AttributeValues[0].Replace("\"", String.Empty)}] ON [dbo].[{entityClassName.ClassName.Replace("\"", String.Empty)}].{primaryKeyAttr}=[dbo].[{x.Attributes.First(x => x.AttributeName == "ForeignKey")!.AttributeValues[0].Replace("\"", String.Empty)}].{x.Attributes.First(x => x.AttributeName == "ForeignKey")!.AttributeValues[1].Replace("\"", String.Empty)}")));
                    }
                    replaceDict.Add("__PRIMARY_KEY__", primaryKeyAttr);

                    replaceDict.Add("__SET_PARAMETERS__", string.Join(", ", properties.Select(x => $"{x.PropertyName} = @{x.PropertyName}")));
                }

                var sourceFilePath = GetSourceFileFormTemplatePath(path, replaceDict["__Entity__"], replaceDict["__Entities__"]);

                var sourceContent = ReplaceFileContent(sourceFilePath, replaceDict);

                _fileService.WriteToFile(path, sourceContent);
            }

            foreach(var path in entitiesPath)
            {
                var destPath = path.Replace(ENTITIES_DIR_PATH, $"{entitiesDir}\\");

                var sourceContennt = File.ReadAllText(path);

                _fileService.EnsureIfFileExists(destPath);
                _fileService.WriteToFile(destPath, sourceContennt);
            }
        }

        private List<ClassModel> GetListOfEntites(IEnumerable<string> entitesPaths)
        {
            var rst = new List<ClassModel>();   

            foreach(var path in entitesPaths)
            {
                rst.AddRange(_syntaxProvider.MapSyntaxToClassModel(path).Select(x => x));
            }

            return rst;
        } 

        private async Task PrepareTemplatesFile(DirectoryContentModel templates, DirectoryContentModel entities, List<ClassModel> entitiesClasses)
        {
            var entityNames = entities.Files.Select(GetFileNameFromPath).ToList();

            ReplaceProjectNameInFilePaths(templates);

            ReplaceEntityNameInFilePaths(templates, entityNames);

            ReplaceEntitiesNameInFilePaths(templates, entitiesClasses);
            
            ReplacesDestinationPath(templates);

            _fileService.CreateDirectories(templates.Directories);
            _fileService.CreateFiles(templates.Files);
        }

        //TODO: refactor and split
        private void ReplaceEntitiesNameInFilePaths(DirectoryContentModel templates, List<ClassModel> entitiesClasses)
        {
            for (int i = 0 ; i < templates.Directories.Count; i++)
            {
                var temp = templates.Directories[i];
                if (temp.Contains(ReplacePatterns.EntitiesPattern))
                {
                    if(entitiesClasses.Any(entity => temp.Contains($"\\{entity.ClassName}")))
                    {
                        var entityClassName = entitiesClasses.Where(entity => temp.Contains($"\\{entity.ClassName}"));
                        if(entityClassName.Count() > 1)
                        {
                            throw new Exception("Wrong definition of entities. Same name in a few entities.");
                        }

                        var entityPlural = entityClassName.FirstOrDefault().Attributes.FirstOrDefault(x => x.AttributeName == "PluralNameEntity")?.AttributeValues.FirstOrDefault() 
                            ?? entityClassName.FirstOrDefault().ClassName;

                        //TODO: abstract regex
                        templates.Directories[i] = temp.Replace(ReplacePatterns.EntitiesPattern, Regex.Replace(entityPlural, @"[^\p{L}-\s]+", ""));
                    }
                }
            }  
            
            for (int i = 0 ; i < templates.Files.Count; i++)
            {
                var temp = templates.Files[i];
                if (temp.Contains(ReplacePatterns.EntitiesPattern))
                {
                    if (entitiesClasses.Any(entity => temp.Contains($"\\{entity.ClassName}")))
                    {
                        var entityClassName = entitiesClasses.Where(entity => temp.Contains($"\\{entity.ClassName}"));
                        if (entityClassName.Count() > 1)
                        {
                            throw new Exception("Wrong definition of entities. Same name in a few entities.");
                        }

                        var entityPlural = entityClassName.FirstOrDefault().Attributes.FirstOrDefault(x => x.AttributeName == "PluralNameEntity")?.AttributeValues.FirstOrDefault()
                            ?? entityClassName.FirstOrDefault().ClassName;

                        templates.Files[i] = temp.Replace(ReplacePatterns.EntitiesPattern, Regex.Replace(entityPlural, @"[^\p{L}-\s]+", ""));
                    }
                }
            }
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
