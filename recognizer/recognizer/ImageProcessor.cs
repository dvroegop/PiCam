// recognizer
// recognizer
// 
// ImageProcessor.cs
// (c)  Dennis Vroegop
// 
// Last edited: 2019-05-14 at 2:03 PM

using System;
using System.Threading.Tasks;
using MMALSharp;
using MMALSharp.Handlers;
using MMALSharp.Native;

namespace recognizer
{
    internal class ImageProcessor : IDisposable
    {
        private MMALCamera _cameraDevice;

        /// <summary>
        /// Takes a picture and saves that in the default location
        /// </summary>
        /// <returns>The filename of the picture,
        /// without path or extension</returns>
        public async Task<string> TakePicture()
        {
            // Instantiate the camera, if needed
            // In my demos I always have the cam upside down. Fix that (not needed
            // for analyzing, just for the user...)
            if (_cameraDevice == null)
            {
                MMALCameraConfig.Flips = MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_BOTH;
                _cameraDevice = MMALCamera.Instance;
            }

            // Create the handler that will receive the data
            using (var imgCaptureHandler = new ImageStreamCaptureHandler(
                "/home/pi/images/", 
                "jpg"))
            {
                // Call the camera and collect bytes
                await _cameraDevice.TakePicture(
                    imgCaptureHandler, 
                    MMALEncoding.JPEG, 
                    MMALEncoding.I420);

                // Get the last generated filename
                var fileName = imgCaptureHandler.GetFilename();

                return fileName;
            }
        }

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
                _cameraDevice.Cleanup();
                _cameraDevice = null;
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