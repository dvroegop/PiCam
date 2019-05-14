// recognizer
// recognizer
// 
// Program.cs
// (c) 2019 Dennis Vroegop
// 
// Last edited: 2019-05-14 at 1:56 PM

#region Using Statements

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using MMALSharp;
using MMALSharp.Handlers;
using MMALSharp.Native;

#endregion

namespace recognizer
{
    internal class Program
    {
        private static readonly string PredictionKey = "fc80a3233dc0404db9a05c7524bddea9";
        private static string TrainingKey = "85bab12d10774075b9869840bff50c3e";
        private static readonly string TrainingEndPointShort = "https://westeurope.api.cognitive.microsoft.com";

        private static string TrainingEndPoint =
            "https://westeurope.api.cognitive.microsoft.com/customvision/v3.0/Training/";

        private static string PredictionEndpoint =
            "https://westeurope.api.cognitive.microsoft.com/customvision/v3.0/Prediction/";

        private static string TrainingResourceId =
            "/subscriptions/279d1d3a-3490-47dd-85e5-ee752ad3da9d/resourceGroups/RGSpilberg/providers/Microsoft.CognitiveServices/accounts/RGSpilberg";

        private static string PredictionResourceId =
            "/subscriptions/279d1d3a-3490-47dd-85e5-ee752ad3da9d/resourceGroups/RGSpilberg/providers/Microsoft.CognitiveServices/accounts/RGSpilberg_prediction";

        private static readonly string _projectId = "aeeb0af8-9c49-45d3-8419-9c76485c1fdf";


        private static void Main(string[] args)
        {
            // Take a picture
            var imageProcessor = new ImageProcessor();

            string fileName = imageProcessor.TakePicture().Result;
            Console.WriteLine($"Filename is: {fileName}");


            var predictionClient = new CustomVisionPredictionClient
            {
                ApiKey = PredictionKey,
                Endpoint = TrainingEndPointShort
            };

            Guid projectId = Guid.Parse(_projectId);
            imageProcessor.MakePrediction(predictionClient, projectId, "Iteration2", fileName);

            // Upload it to the cloud

            // Show the result

            // Shutdown
        }
    }

    internal class ImageProcessor : IDisposable
    {
        public void MakePrediction(CustomVisionPredictionClient endpoint, Guid projectId, string publishedModelName,
            string fileName)
        {
            Console.WriteLine("Making a prediction:");

            string imageFile = Path.Combine("/home/pi/images/", $"{fileName}.jpg");
            using (FileStream stream = File.OpenRead(imageFile))
            {
                ImagePrediction result = endpoint.DetectImage(projectId, publishedModelName, File.OpenRead(imageFile));

                // Loop over each prediction and write out the results
                foreach (PredictionModel c in result.Predictions)
                    Console.WriteLine(
                        $"\t{c.TagName}: {c.Probability:P1} [ {c.BoundingBox.Left}, {c.BoundingBox.Top}, {c.BoundingBox.Width}, {c.BoundingBox.Height} ]");
            }
        }

        private MMALCamera _mmalCamera;
        // private string UploadUrl = "https://westeurope.api.cognitive.microsoft.com/customvision/v3.0/Prediction/aeeb0af8-9c49-45d3-8419-9c76485c1fdf/detect/iterations/Iteration1/image";

        public async Task<string> TakePicture()
        {
            if (_mmalCamera == null)
            {
                MMALCameraConfig.Flips = MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_BOTH;
                _mmalCamera = MMALCamera.Instance;
            }

            using (var imgCaptureHandler = new ImageStreamCaptureHandler("/home/pi/images/", "jpg"))
            {
                await _mmalCamera.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
                string fileName = imgCaptureHandler.GetFilename();

                return fileName;
            }
        }
        /*
         * 
         * t.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };

            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            using (var client = new HttpClient())

            using (var formData = new MultipartFormDataContent())

        public async Task<bool> UploadImage(Stream image, string fileName, string predictionKey)
        {
            HttpContent fileStreamContent = new StreamContent(image);
            //fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            fileStreamContent.Headers.Add("Prediction-Key", predictionKey);
            using (var client = new HttpClient())
                using(var formData = new MultipartFormDataContent())
            {
                formData.Add(fileStreamContent);
                HttpResponseMessage response = await client.PostAsync(UploadUrl, formData);
                return response.IsSuccessStatusCode;
            }
        }


        //public async Task<bool> GetPrediction(string key, string endPoint, string fileName)
        //{
        //    var client = new CustomVisionPredictionClient() { ApiKey = key, Endpoint = endPoint };
        //    var imageFile = $"/home/pi/images/{fileName}.jpg";

        //    using(var stream = File.OpenRead(imageFile))
        //    {
        //        var result = await client.DetectImageAsync()
        //    }
        //}
                 * */


        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _mmalCamera.Cleanup();
                _mmalCamera = null;
            }

            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        ~ImageProcessor()
        {
            Dispose(false);
        }

        #endregion
    }
}