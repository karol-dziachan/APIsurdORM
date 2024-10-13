namespace Pr0t0k07.APIsurdORM.Application.Shared.Exceptions
{
    [Serializable]
    public class FileServiceException : Exception
    {
        public FileServiceException(string message) : base(message)
        { }
    }
}
