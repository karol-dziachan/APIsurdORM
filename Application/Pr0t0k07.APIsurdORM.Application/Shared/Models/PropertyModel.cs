namespace Pr0t0k07.APIsurdORM.Application.Shared.Models
{
    public class PropertyModel
    {
        public string PropertyName { get; set; }
        public List<AttributeModel> Attributes { get; set; } = new List<AttributeModel>();
    }
}
