namespace SalesSystem.Application.Interfaces
{
    public interface IFireBaseService
    {
        Task<string?> UploadStorage(Stream streamFile, string destinationFolder, string fileName);
        Task<bool> DeleteStorage(string destinationFolder, string fileName);
    }
}
