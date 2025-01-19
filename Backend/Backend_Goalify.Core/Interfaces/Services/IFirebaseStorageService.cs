public interface IFirebaseStorageService
{
    Task<string> UploadProfilePictureAsync(string userId, Stream fileStream, string contentType);
    Task DeleteProfilePictureAsync(string fileUrl);
} 