using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FileUploader.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FileUploader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlobServiceClient _blobServiceClient;

        public HomeController(ILogger<HomeController> logger, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _blobServiceClient = blobServiceClient;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> Index(IFormFile files)
        {
            try
            {

                if (files != null)
                {

                    HttpClient httpClient = new HttpClient();

                    // Create the container and return a container client object
                    BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("demo");
                    await containerClient.CreateIfNotExistsAsync();

                    // Get a reference to a blob
                    BlobClient blobClient = containerClient.GetBlobClient(files.FileName);
                    BlobHttpHeaders httpHeaders = new BlobHttpHeaders()
                    {
                        ContentType = files.ContentType
                    };

                    // Upload data from the local file
                    await blobClient.UploadAsync(files.OpenReadStream(), httpHeaders);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}