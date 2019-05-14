using System;
using System.IO;
using System.Linq;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace cuporglass
{
    class Program
    {

        static string PredictionKey = "fc80a3233dc0404db9a05c7524bddea9";
        static string TrainingKey = "85bab12d10774075b9869840bff50c3e";
        static string TrainingEndPoint = "https://westeurope.api.cognitive.microsoft.com/customvision/v3.0/Training/";
        static string TrainingEndPointShort = "https://westeurope.api.cognitive.microsoft.com";
        static string PredictionEndpoint = "https://westeurope.api.cognitive.microsoft.com/customvision/v3.0/Prediction/";
        static string TrainingResourceId = "/subscriptions/279d1d3a-3490-47dd-85e5-ee752ad3da9d/resourceGroups/RGSpilberg/providers/Microsoft.CognitiveServices/accounts/RGSpilberg";
        static string PredictionResourceId = "/subscriptions/279d1d3a-3490-47dd-85e5-ee752ad3da9d/resourceGroups/RGSpilberg/providers/Microsoft.CognitiveServices/accounts/RGSpilberg_prediction";


        private static string _projectId = "aeeb0af8-9c49-45d3-8419-9c76485c1fdf";

        static void Main(string[] args)
        {
            string fileName = "testimage";

            string imageFile = Path.Combine("/home/pi/images/", $"{fileName}.jpg");
            
            Console.WriteLine(imageFile);
            return;

            var trainingClient = new CustomVisionTrainingClient()
                {ApiKey = TrainingKey, Endpoint = TrainingEndPointShort};

            var predictionClient = new CustomVisionPredictionClient()
            {
                ApiKey = PredictionKey, Endpoint = TrainingEndPointShort
            };

            var projectId = Guid.Parse(_projectId);
            MakePrediction(predictionClient, projectId, "Iteration2");
        }

        public static void MakePrediction(CustomVisionPredictionClient endpoint, Guid projectId, string publishedModelName)
        {

            Console.WriteLine("Making a prediction:");
            var imageFile = "c:\\temp\\image.jpg";
            //Path.Combine("Images", "test", "test_image.jpg");
            using (var stream = File.OpenRead(imageFile))
            {
                var result = endpoint.DetectImage(projectId, publishedModelName, File.OpenRead(imageFile));

                // Loop over each prediction and write out the results
                foreach (var c in result.Predictions)
                {
                    Console.WriteLine(
                        $"\t{c.TagName}: {c.Probability:P1} [ {c.BoundingBox.Left}, {c.BoundingBox.Top}, {c.BoundingBox.Width}, {c.BoundingBox.Height} ]");
                }
            }
        }
    }
}
