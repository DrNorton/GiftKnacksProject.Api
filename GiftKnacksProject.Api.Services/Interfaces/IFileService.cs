using GiftKnacksProject.Api.Services.Services;

namespace GiftKnacksProject.Api.Services.Interfaces
{
    public interface IFileService
    {
        string SaveBase64FileReturnUrl(FileType fileType,string mimeType, string base64File);
    }
}