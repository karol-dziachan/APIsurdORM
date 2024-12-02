using Pr0t0k07.APIsurdORM.Application.Shared.Models;

namespace Pr0t0k07.APIsurdORM.Application.Shared.Interfaces
{
    public interface ISyntaxProvider
    {
        List<ClassModel> MapSyntaxToClassModel(string filePath); 
    }
}
