using System;
using System.Threading.Tasks;
using MMALSharp;
using MMALSharp.Handlers;
using MMALSharp.Native;

namespace recognizer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Take a picture
            var imageProcessor = new ImageProcessor();

            imageProcessor.TakePicture().Wait();

            // Upload it to the cloud

            // Show the result

            // Shutdown

        }
    }

    class ImageProcessor : IDisposable
    {
        private MMALCamera _mmalCamera;

        public async Task TakePicture()
        {
            if(_mmalCamera == null)
                _mmalCamera = MMALCamera.Instance;

            using (var imgCaptureHandler = new ImageStreamCaptureHandler("/home/pi/images/", "jpg"))
            {
                await _mmalCamera.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);

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
