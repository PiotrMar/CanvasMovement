using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace CanvasMovement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Accelerometer accelerometer { get; set; }
        private uint _interval;
        private DispatcherTimer _timer;
        public MainPage()
        {
            this.InitializeComponent();
            accelerometer = Accelerometer.GetDefault();

            uint minInterval = accelerometer.MinimumReportInterval;
            _interval = minInterval > 8 ? minInterval : 8;

            _timer = new DispatcherTimer();
            _timer.Tick += DisplayReading;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, (int)_interval);

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Window.Current.VisibilityChanged -= new WindowVisibilityChangedEventHandler(VisibilityChanged);
        }

        private void BtnLeft_Click(object sender, RoutedEventArgs e)
        {
            double position = Canvas.GetLeft(TestImage);
            Canvas.SetLeft(TestImage, position - 10);
        }

        private void BtnRight_Click(object sender, RoutedEventArgs e)
        {
            double position = Canvas.GetLeft(TestImage);
            Canvas.SetLeft(TestImage, position + 10);
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            double position = Canvas.GetTop(TestImage);
            Canvas.SetTop(TestImage, position - 10);
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            double position = Canvas.GetTop(TestImage);
            Canvas.SetTop(TestImage, position + 10);
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            accelerometer.ReportInterval = _interval;
            Window.Current.VisibilityChanged += new WindowVisibilityChangedEventHandler(VisibilityChanged);
            _timer.Start();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void VisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            _timer.Start();
        }

        private void DisplayReading(object sender, object args)
        {
            AccelerometerReading reading = accelerometer.GetCurrentReading();

            if(reading != null)
            {
                double x = reading.AccelerationX;
                double z = reading.AccelerationZ;

                if (x > 0.05)
                {
                    if (Canvas.GetLeft(TestImage) < 310)
                    {
                        double position = Canvas.GetLeft(TestImage);
                        Canvas.SetLeft(TestImage, position + 10);
                    }
                }
                else if (x < -0.05)
                {
                    if (Canvas.GetLeft(TestImage) > 0)
                    {
                        double position = Canvas.GetLeft(TestImage);
                        Canvas.SetLeft(TestImage, position - 10);
                    }
                }
                if (z > 0.05)
                {
                    if (Canvas.GetTop(TestImage) < 310)
                    {
                        double position = Canvas.GetTop(TestImage);
                        Canvas.SetTop(TestImage, position + 10);
                    }
                }
                else if (z < -0.05)
                {
                    if (Canvas.GetTop(TestImage) > 0)
                    {
                        double position = Canvas.GetTop(TestImage);
                        Canvas.SetTop(TestImage, position - 10);
                    }
                }
            }
        }
    }
}
