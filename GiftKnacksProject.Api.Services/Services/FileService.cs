using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GiftKnacksProject.Api.Services.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GiftKnacksProject.Api.Services.Services
{
    public class FileService : IFileService
    {
        private readonly UrlSettings _urlSettings;

        public FileService(UrlSettings urlSettings)
        {
            _urlSettings = urlSettings;
        }

        public string SaveBase64FileReturnUrl(FileType fileType,string mimeType, string base64File)
        {
            var split = base64File.Split(new char[] { ',' }); //Убираем ненужную инфу о файле
            var imagestr = split[1];
            
            var bytes = Convert.FromBase64String(imagestr);
            string name= null;
            if (mimeType == null)
            {
                //old firefox
                mimeType = base64File.Split(new char[] {';'})[0];
                 name = CreateFileNameWithType(mimeType, _urlSettings.ApiUrl);
            }
            else
            {
                 name = CreateFileNameWithType(mimeType, _urlSettings.ApiUrl);
            }

            return InsertToBlob(name, bytes, "avatars");
        }

        public  string InsertToBlob(string name, byte[] image,string containerName)
        {
            var storageAccount =
                CloudStorageAccount.Parse(_urlSettings.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
            container.SetPermissions(
                    new BlobContainerPermissions
                    {
                        PublicAccess =
                            BlobContainerPublicAccessType.Blob
                    });
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            using (var fileStream = new MemoryStream(image))
            {
                blockBlob.UploadFromStream(fileStream);
            }
           
            return blockBlob.Uri.ToString();
        }

        private string CreateFileNameWithType(string mimeType,string apiUrl)
        {
            var type = mimeType.Split(new char[] { '/' })[1];
            var guid = Guid.NewGuid();
            var uriBuilder = new StringBuilder();
            uriBuilder.Append(guid);
            uriBuilder.Append(".");
            uriBuilder.Append(type);

            return uriBuilder.ToString();
        }


       
    }

    public enum FileType
    {
        Image
    }
}
