using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Pr0t0k07.APIsurdORM.Application.Shared.Interfaces;
using Pr0t0k07.APIsurdORM.Application.Shared.Models;

namespace Pr0t0k07.APIsurdORM.Infrastructure.Shared.Services
{
    public class SyntaxProvider : ISyntaxProvider
    {
        public List<ClassModel> MapSyntaxToClassModel(string filePath)
        {
            var classes = new List<ClassModel>();
            string code = File.ReadAllText(filePath);

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();

            var classesDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

            foreach (var classDeclaration in classesDeclarations)
            {
                var @class = new ClassModel()
                {
                    ClassName = classDeclaration.Identifier.Text,
                };

                var classAttributes = classDeclaration.AttributeLists.SelectMany(x => x.Attributes);
                foreach (var attribute in classAttributes)
                {
                    var arguments = attribute.ArgumentList?.Arguments;
                    var propertyValue = arguments.HasValue && arguments.Value.Count > 0 ? arguments.Value.Select(x => x.ToString()).ToList() : new List<string>();
                    @class.Attributes.Add(new AttributeModel() { AttributeName = attribute.Name.ToString(), AttributeValues = propertyValue });
                }

                var properties = classDeclaration.Members.OfType<PropertyDeclarationSyntax>();
                foreach (var property in properties)
                {
                    var prop = new PropertyModel()
                    {
                        PropertyName = property.Identifier.Text,
                    };
                    var propertyAttributes = property.AttributeLists.SelectMany(x => x.Attributes);
                    foreach (var attribute in propertyAttributes)
                    {
                        var arguments = attribute.ArgumentList?.Arguments;
                        var propertyValue = arguments.HasValue && arguments.Value.Count > 0 ? arguments.Value.Select(x => x.ToString()).ToList() : new List<string>();
                        prop.Attributes.Add(new() { AttributeName = attribute.Name.ToString(), AttributeValues = propertyValue });
                    }

                    @class.Properties.Add(prop);
                }
                classes.Add(@class);
            }

            return classes;
        }
    }
}
