// recognizer
// recognizer
// 
// Program.cs
// (c) 2019 Dennis Vroegop
// 
// Last edited: 2019-05-14 at 2:02 PM

#region Using Statements

using System;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;

#endregion

namespace recognizer
{
    internal class Program
    {
        #region Keys and URLS
        private static readonly string PredictionKey = 
            "fc80a3233dc0404db9a05c7524bddea9";

        // When you want to use a trainer, use this key ->
        // private static string TrainingKey = "85bab12d10774075b9869840bff50c3e";

        private static readonly string ServiceEndpoint = 
            "https://westeurope.api.cognitive.microsoft.com";

        private static readonly string _projectId = 
            "aeeb0af8-9c49-45d3-8419-9c76485c1fdf";
        #endregion

        private static void Main(string[] args)
        {
            // Take a picture
            var fileName = TakePicture();

            // See if we can find something interesting
            DetectImage(fileName);
        }
        private static string TakePicture()
        {
            // Create an instance of the processor
            var imageProcessor = new ImageProcessor();

            // Take the picture, store the filename
            var fileName = imageProcessor.TakePicture().Result;

            Console.WriteLine($"Filename is: {fileName}");
            return fileName;
        }

        private static void DetectImage(string fileName)
        {
            // Create instance of the API wrapper
            var endPoint = new CustomVisionPredictionClient
            {
                ApiKey = PredictionKey,
                Endpoint = ServiceEndpoint
            };

            // Generate the GUID with the project ID from the strig
            var projectId = Guid.Parse(_projectId);

            // Generate the detector class instance
            var detector = new ItemDetector();

            // Analyze the picture
            detector.DetectImageCharacteristics(
                endPoint,
                projectId,
                "Iteration2",
                fileName);
        }
    }
}