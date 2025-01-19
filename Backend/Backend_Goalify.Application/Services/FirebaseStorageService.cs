using FirebaseAdmin;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System.Web;
using Microsoft.Extensions.Logging;

public class FirebaseStorageService : IFirebaseStorageService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;
    private readonly ILogger<FirebaseStorageService> _logger;

    public FirebaseStorageService(IConfiguration configuration, ILogger<FirebaseStorageService> logger)
    {
        var credential = GoogleCredential.FromFile(configuration["Firebase:CredentialsPath"]);
        _storageClient = StorageClient.Create(credential);
        _bucketName = configuration["Firebase:StorageBucket"];
        _logger = logger;
    }

    public async Task<string> UploadProfilePictureAsync(string userId, Stream fileStream, string contentType)
    {
        var objectName = $"profile-pictures/{userId}/{Guid.NewGuid()}";
        var obj = await _storageClient.UploadObjectAsync(
            _bucketName,
            objectName,
            contentType,
            fileStream
        );

        return $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(objectName)}?alt=media";
    }

    public async Task DeleteProfilePictureAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl)) return;

        try
        {
            _logger.LogInformation($"Starting deletion for URL: {fileUrl}");

            // Parse the URL to get the object name
            var uri = new Uri(fileUrl);
            var pathSegments = uri.AbsolutePath.Split('/');
            
            // The path format is /v0/b/BUCKET/o/OBJECT
            if (pathSegments.Length >= 5 && pathSegments[4] == "o")
            {
                // Get everything after "/o/" from the URL
                var startIndex = fileUrl.IndexOf("/o/") + 3;
                var endIndex = fileUrl.IndexOf("?alt=media");
                if (endIndex == -1) endIndex = fileUrl.Length;
                
                var encodedObjectName = fileUrl.Substring(startIndex, endIndex - startIndex);
                var objectName = Uri.UnescapeDataString(encodedObjectName);
                
                _logger.LogInformation($"Attempting to delete object: {objectName} from bucket: {_bucketName}");
                
                try
                {
                    await _storageClient.DeleteObjectAsync(_bucketName, objectName);
                    _logger.LogInformation($"Successfully deleted object: {objectName}");
                }
                catch (Exception deleteEx)
                {
                    _logger.LogError(deleteEx, $"Error deleting object {objectName} from bucket {_bucketName}");
                    throw;
                }
            }
            else
            {
                throw new InvalidOperationException($"Invalid Firebase Storage URL format: {fileUrl}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to delete file: {fileUrl}");
            throw new InvalidOperationException($"Failed to delete file: {ex.Message}", ex);
        }
    }
} 