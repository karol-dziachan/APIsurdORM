﻿using Pr0t0k07.APIsurdORM.Application.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Pr0t0k07.APIsurdORM.Application.Shared.Models;
using Pr0t0k07.APIsurdORM.Application.Shared;
using Microsoft.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace Pr0t0k07.APIsurdORM.Application.Workers
{
    public class GenerateApplication
    {
        private readonly ILogger<GenerateApplication> _logger;
        private readonly IFileService _fileService;
        private readonly ISyntaxProvider _syntaxProvider;
        private readonly IOptionsSnapshot<Settings> _settings; 

        public GenerateApplication(ILogger<GenerateApplication> logger, IFileService fileService, ISyntaxProvider syntaxProvider, IOptionsSnapshot<Settings> settings)
        {
            _logger = logger ?? NullLogger<GenerateApplication>.Instance;
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _syntaxProvider = syntaxProvider ?? throw new ArgumentNullException(nameof(syntaxProvider));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));  
        }

        public async Task Handle()
        {
            try
            {
                _logger.LogInformation("Start generating.");
                _logger.LogInformation("Get files and directories from templates.");

                var templates = _fileService.GetDirectoryResources(_settings.Value.TemplatesPath, new List<string>() { @"\bin", @"\obj" });
                var entities = _fileService.GetDirectoryResources(_settings.Value.EntitiesDirPath, new List<string>() { @"\bin", @"\obj" });
               
                _logger.LogInformation("Succesfully get the templates.");

                var entitiesClasses = GetListOfEntites(entities.Files);

                _logger.LogInformation("Succesfully collecting infos about classes.");

                await PrepareTemplatesFile(templates, entities, entitiesClasses);

                _logger.LogInformation("Succesfully creating the templates.");

                WriteTemplates(templates.Files, entities.Files, templates.Directories.FirstOrDefault(x => x.Contains(".Domain") && x.EndsWith("\\Entities")), entitiesClasses);

                _logger.LogInformation("Succesfully writing content to the templates.");
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
                    {$"{_settings.Value.ProjetName}" , "{{ProjectName}}" },
                    {_settings.Value.DestinationPath, _settings.Value.TemplatesPath }
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
                    {"__ProjectName__" , _settings.Value.ProjetName },
                    {"{{ProjectName}}" , _settings.Value.ProjetName },
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
                    replaceDict["__Entities__"] = Regex.Replace(entityNameInPlural, ReplacePatterns.NonAlphaNumericRegex, "");

                    var properties = entityClassName.Properties.Where(x => !x.Attributes.Any(x => x.AttributeName == "AutoNumerated"));
                    replaceDict.Add("__ENTITY_PARAMETERS__", string.Join(", ", properties.Where(x => !x.Attributes.Any(x => x.AttributeName == "DefaultValue")).Select(x => x.PropertyName)));
                    replaceDict.Add("__ENTITY_VALUES__", string.Join(", ", properties.Where(x => !x.Attributes.Any(x => x.AttributeName == "DefaultValue")).Select(x => $"@{x.PropertyName}")));
                    
                    replaceDict.Add("//__ENTITY_PARAMETERS_MAPPING__", string.Join(", ", properties.Where(x => !x.Attributes.Any(x => x.AttributeName == "DefaultValue")).Select(x => $"{x.PropertyName} = entity.{x.PropertyName}")));

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

                replaceDict.Add("//__INJECT_SERVICES__", string.Join("\n", entitiesClasses.Select(x => $"services.AddScoped<I{x.ClassName}Repository>(provider => new {x.ClassName}Repository(connectionString));")));

                replaceDict.Add("--__TABLES_DDL__", CreateDdlScript(entitiesClasses));


                var sourceFilePath = GetSourceFileFormTemplatePath(path, replaceDict["__Entity__"], replaceDict["__Entities__"]);

                var sourceContent = ReplaceFileContent(sourceFilePath, replaceDict);

                _fileService.WriteToFile(path, sourceContent);
            }

            foreach(var path in entitiesPath)
            {
                var destPath = path.Replace(_settings.Value.EntitiesDirPath, $"{entitiesDir}\\");

                var sourceContennt = File.ReadAllText(path);

                _fileService.EnsureIfFileExists(destPath);
                _fileService.WriteToFile(destPath, sourceContennt);
            }
        }

        private string CreateDdlScript(List<ClassModel> entitiesClasses)
        {
            StringBuilder res = new StringBuilder();

            foreach(var entity in entitiesClasses)
            {
                string sql = ReplacePatterns.DdlPattern.Replace("__TABLE_NAME__", entity.ClassName);
                StringBuilder columns = new StringBuilder();
                StringBuilder constrains = new StringBuilder(String.Empty);
                string composite = String.Empty;
                foreach(var property in entity.Properties)
                {
                    var identity = property.Attributes.Any(x => x.AttributeName == "AutoNumerated") ? "IDENTITY (1,1)" : String.Empty;
                    var required = property.Attributes.Any(x => x.AttributeName == "RequiredProperty") ? "NOT NULL" : String.Empty;
                    var primaryKey = property.Attributes.Any(x => x.AttributeName == "PrimaryKey") && !entity.Attributes.Any(x => x.AttributeName == "ManyToMany") ? "PRIMARY KEY" : String.Empty;
                    var unique = property.Attributes.Any(x => x.AttributeName == "Unique") ? "UNIQUE" : String.Empty;
                    var defaultValue = property.Attributes.FirstOrDefault(x => x.AttributeName == "DefaultValue")?.AttributeValues.FirstOrDefault() ?? String.Empty;
                    var sqlType = property.Attributes.First(x => x.AttributeName == "SqlType").AttributeValues.First();

                    var contraintsAttr = property.Attributes.FirstOrDefault(x => x.AttributeName == "ForeignKey")?.AttributeValues;


                    string columnCommand = $"[{property.PropertyName}] {sqlType} {primaryKey} {required} {unique} {defaultValue} {identity},\n";
                    if(contraintsAttr is not null && contraintsAttr.Any())
                    {
                        string constaintCommand = $"CONSTRAINT {contraintsAttr.First()}Fk FOREIGN KEY ([{property.PropertyName}]) REFERENCES [dbo].[{contraintsAttr.First()}] ([{contraintsAttr.Skip(1).First()}])\n";
                        constrains.Append(constaintCommand);
                    }
                    columns.Append(columnCommand);
                }

                if (entity.Attributes.Any(x => x.AttributeName == "ManyToMany"))
                {
                    composite = $"CONSTRAINT {string.Join("_", entity.Properties.Where(x => x.Attributes.Any(y => y.AttributeName == "PrimaryKey")).Select(x => x.PropertyName))}Pk PRIMARY KEY ({string.Join(", ", entity.Properties.Where(x => x.Attributes.Any(y => y.AttributeName == "PrimaryKey")).Select(x => $"[{x.PropertyName}]"))}),";
                }

                res.Append(sql.Replace("__COLUMNS__", columns.ToString()).Replace("__CONSTAINTS__", constrains.ToString()).Replace("__COMPOSITE_KEY__", composite));
            }

            return res.ToString().Replace("\"", String.Empty);
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

                        templates.Directories[i] = temp.Replace(ReplacePatterns.EntitiesPattern, Regex.Replace(entityPlural, ReplacePatterns.NonAlphaNumericRegex, ""));
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

                        templates.Files[i] = temp.Replace(ReplacePatterns.EntitiesPattern, Regex.Replace(entityPlural, ReplacePatterns.NonAlphaNumericRegex, ""));
                    }
                }
            }
        }          
        
        private void ReplaceProjectNameInFilePaths(DirectoryContentModel templates)
        {
            ReplaceInList(templates.Directories, ReplacePatterns.ProjectNamePattern, _settings.Value.ProjetName);
            ReplaceInList(templates.Files, ReplacePatterns.ProjectNamePattern, _settings.Value.ProjetName);
        }        
        
        private void ReplacesDestinationPath(DirectoryContentModel templates)
        {
            ReplaceInList(templates.Directories, _settings.Value.TemplatesPath, _settings.Value.DestinationPath);
            ReplaceInList(templates.Files, _settings.Value.TemplatesPath, _settings.Value.DestinationPath);
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

            var replacedItems = itemsWhichContainsPattern.SelectMany(dir => newValues.Select(newVal => dir.Replace(replacePattern, newVal))); 
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
