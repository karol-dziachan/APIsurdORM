namespace Pr0t0k07.APIsurdORM.Application.Shared
{
    public class ReplacePatterns
    {
        public static string ProjectNamePattern = "{{ProjectName}}";
        public static string EntityPattern = "{{Entity}}";
        public static string EntitiesPattern = "{{Entities}}";

        public static string ContentProjectNamePattern = "__ProjectName__";
        public static string ContentEntityPattern = "__Entity__";
        public static string ContentEntitiesPattern = "__Entities__";

        public static string NonAlphaNumericRegex = @"[^\p{L}-\s]+";

        public static string DdlPattern = @"
IF NOT EXISTS (SELECT * FROM sysobjects WHERE NAME = '__TABLE_NAME__' AND xtype='U')
	CREATE TABLE [dbo].[__TABLE_NAME__]
	(
        __COLUMNS__
        __COMPOSITE_KEY__
        __CONSTAINTS__
	)
GO";
    }

}
