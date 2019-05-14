// recognizer
// recognizer
// 
// ItemDetector.cs
// (c)  Dennis Vroegop
// 
// Last edited: 2019-05-14 at 2:05 PM

using System;
using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace recognizer
{
    public class ItemDetector
    {
        /// <summary>
        /// Analyzes the image to see if we have
        /// something we're interested in.
        /// </summary>
        /// <param name="endPoint">The endpoint of our CustomVision project</param>
        /// <param name="projectId">The <see cref="Guid"/>of the project</param>
        /// <param name="publishedModelName">The name of the published model</param>
        /// <param name="fileName">The raw image name we're analyzing</param>
        public void DetectImageCharacteristics(
            CustomVisionPredictionClient endPoint, 
            Guid projectId,
            string publishedModelName,
            string fileName)
        {
            Console.WriteLine("Making a prediction:");

            // Generate the full path
            var imageFile = Path.Combine("/home/pi/images/", $"{fileName}.jpg");

            // Open the image stream for uploading
            using (FileStream stream = File.OpenRead(imageFile))
            {
                // Call the API with the image stream
                ImagePrediction result = endPoint.DetectImage(
                    projectId, 
                    publishedModelName, 
                    File.OpenRead(imageFile));

                // Loop over each prediction and write out the results
                foreach (PredictionModel c in result.Predictions)
                    Console.WriteLine(
                        $"\t{c.TagName}: {c.Probability:P1} [ {c.BoundingBox.Left}, {c.BoundingBox.Top}, {c.BoundingBox.Width}, {c.BoundingBox.Height} ]");
            }
        }
    }
}