using ThunderServer.API.DTOs;

namespace ThunderServer.API.Endpoints.Files
{
    public sealed record FileUploadResponse
    {
        public List<ThunderFileDto>? ThunderFiles { get; set; }
    }
}