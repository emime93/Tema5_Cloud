using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace CloudWebMVC.Controllers
{
    public class FileController : Controller
    {
        public ActionResult Upload()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);

                        // Retrieve storage account from connection string.
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                            CloudConfigurationManager.GetSetting("StorageConnectionString"));

                        // Create the blob client.
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                        // Retrieve reference to a previously created container.
                        CloudBlobContainer container = blobClient.GetContainerReference("files");

                        container.CreateIfNotExists();

                        // Retrieve reference to a blob named "myblob".
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(Models.Profile.leUser.Username + "_" + file.FileName);
                        using (var context = new CloudModel.LeModelContainer())
                        {
                            CloudModel.Document doc = new CloudModel.Document { Name = blockBlob.Name };
                            Models.Profile.userBL.AddDocumentToUser(doc, Models.Profile.leUser);
                        }
                        blockBlob.Properties.ContentType = file.ContentType;

                        // Create or overwrite the "myblob" blob with contents from a local file.
                        using (var fileStream = System.IO.File.OpenRead(path))
                        {
                            blockBlob.UploadFromStream(fileStream);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }

        }

        public ActionResult GetFiles(string fileName)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("files");

            container.CreateIfNotExists();

            List<String> imageUrls = new List<string>();
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    if (blob.Name.Equals(fileName))
                    {
                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));
                        var path = Path.Combine(originalDirectory.ToString(), blob.Name);
                        blob.DownloadToFile(path, FileMode.Create);
                        Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);
                        return base.File(path, "image/jpeg");
                    }
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob pageBlob = (CloudPageBlob)item;

                    Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    Console.WriteLine("Directory: {0}", directory.Uri);
                }
            }
            return Json(imageUrls, JsonRequestBehavior.AllowGet);
        }
    }
}