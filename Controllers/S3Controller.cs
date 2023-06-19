using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

using Grpc.Core;

using Microsoft.AspNetCore.Mvc;

namespace ElearningMVC.Controllers
{
    public class S3Controller : Controller
    {
        private static IAmazonS3 client;
        private const string bucketName = "elearnings3storage";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1;
        [HttpPost]
        public async Task<ActionResult> Index(IFormFile file)
        {
            client = new AmazonS3Client("AKIAR4FED76S6ZSRLMGJ", "nyWqKNMpFWDonbE2lGNjYtrefz5XtQrf9J4TNq8E", bucketRegion);
            WritingAnObjectAsync(file).Wait();
            return RedirectToAction("UploadSuccess");
        }

        static async Task WritingAnObjectAsync(IFormFile file)
        {
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = file.FileName,
                    ContentBody = "sample text"
                };

                PutObjectResponse response1 = await client.PutObjectAsync(putRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult> UploadSuccess()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}
