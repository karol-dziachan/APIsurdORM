namespace Pr0t0k07.APIsurdORM.Application.Shared.Models
{
    public class ClassModel
    {
        public string ClassName { get; set; }
        public List<AttributeModel> Attributes { get; set; } = new List<AttributeModel>();
        public List<PropertyModel> Properties { get; set; } = new List<PropertyModel>();
    }
}
