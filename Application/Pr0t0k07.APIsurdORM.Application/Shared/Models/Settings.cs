namespace Pr0t0k07.APIsurdORM.Application.Shared.Models
{
    public record Settings
    {
        public string TemplatesPath { get; set; }
        public string DestinationPath { get; set; }
        public string EntitiesDirPath { get; set; }
        public string ProjetName { get; set; }

    }
}
