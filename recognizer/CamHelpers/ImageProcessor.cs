using System;
using System.Threading.Tasks;
using MMALSharp;
using MMALSharp.Handlers;
using MMALSharp.Native;

namespace CamHelpers
{
    public class ImageProcessor : IDisposable
    {
        private MMALCamera _mmalCamera;
        //   private string UploadUrl = "https://westeurope.api.cognitive.microsoft.com/customvision/v3.0/Prediction/aeeb0af8-9c49-45d3-8419-9c76485c1fdf/detect/iterations/Iteration1/image";

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
                var fileName = imgCaptureHandler.GetFilename();

                return fileName;
            }
        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _mmalCamera.Cleanup();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
