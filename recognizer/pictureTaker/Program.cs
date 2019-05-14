using System;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using CamHelpers;
using Sense.Stick;

namespace pictureTaker
{
    class Program
    {
        private static ImageProcessor _processor;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            _processor = new ImageProcessor();

            IObservable<JoystickEvent> d = Joystick.Events.DistinctUntilChanged(p => new JoystickData() {Key = p.Key, State = p.State});
            var observer= new JoystickObserver();

            observer.OnClick += ObserverOnOnClick;
            d.Subscribe(observer);

            Console.WriteLine("Waiting for the joystick");
            Console.ReadLine();
            Console.WriteLine("Bye!");
            _processor.Dispose();
        }

        private static async void ObserverOnOnClick(object sender, EventArgs e)
        {
            Console.WriteLine("Click!!");
            var fileName = await _processor.TakePicture();
            Console.WriteLine($"Captured {fileName}");

        }
    }


    class JoystickObserver : IObserver<JoystickEvent>
    {

        private bool _isDown;
        public event EventHandler OnClick;

        #region Implementation of IObserver<in JoystickData>

        /// <inheritdoc />
        public void OnCompleted()
        {
            Console.WriteLine("Completed");
        }

        /// <inheritdoc />
        public void OnError(Exception error)
        {
            Console.WriteLine($"Error: {error.ToString()}");
        }

        /// <inheritdoc />
        public void OnNext(JoystickEvent value)
        {
            if (value.Key == JoystickKey.Enter)
            {
                if (value.State == JoystickKeyState.Press &&  !_isDown)
                {
                    _isDown = true;
                    OnClick?.Invoke(this, EventArgs.Empty);
                }

                if (value.State == JoystickKeyState.Release)
                {
                    _isDown = false;
                }
            }
            Console.WriteLine($"Data {value.ToString()}");
        }

        #endregion
    }

    class JoystickData
    {
        public JoystickKey Key { get; set; }
        public JoystickKeyState State { get; set; }

        #region Overrides of Object

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Key: {Key}{Environment.NewLine}State: {State}";
        }

        #endregion
    }
}
