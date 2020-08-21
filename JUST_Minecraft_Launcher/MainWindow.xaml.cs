using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace JUST_Minecraft_Launcher
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            closeImg.Source = new BitmapImage(new Uri(@"Resource\close.png", UriKind.RelativeOrAbsolute));
        }

        private void DoClose(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;

            Modal m = new Modal();

            bool? ret = m.ShowDialog();

            if(ret == true)
            {
                this.Close();
            }
            else
            {
                this.Opacity = 1;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SlideStart();
        }

        private ManualResetEvent mr = new ManualResetEvent(false);
        private bool SlideIsRunning = true;
        private BitmapImage[] backgroundImg;

        private void SlideStart()
        {
            backgroundImg = new BitmapImage[] { new BitmapImage(new Uri(@"Resource\b1.jpg", UriKind.RelativeOrAbsolute)), new BitmapImage(new Uri(@"Resource\b2.jpg", UriKind.RelativeOrAbsolute)),
                                                new BitmapImage(new Uri(@"Resource\b3.jpg", UriKind.RelativeOrAbsolute)), new BitmapImage(new Uri(@"Resource\b4.jpg", UriKind.RelativeOrAbsolute)),
                                                new BitmapImage(new Uri(@"Resource\b5.jpg", UriKind.RelativeOrAbsolute)), new BitmapImage(new Uri(@"Resource\b6.jpg", UriKind.RelativeOrAbsolute)),
                                                new BitmapImage(new Uri(@"Resource\b7.jpg", UriKind.RelativeOrAbsolute)), new BitmapImage(new Uri(@"Resource\b8.jpg", UriKind.RelativeOrAbsolute)),
                                                new BitmapImage(new Uri(@"Resource\b9.jpg", UriKind.RelativeOrAbsolute))};

            Thread th = new Thread(SlideThread);
            th.Start();
        }

        private void SlideThread()
        {
            int idx = 0;

            while(SlideIsRunning)
            {
                if(!mr.WaitOne(1000))
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => slideShowIMG.Source = backgroundImg[idx]));
                                        
                    idx++;

                    if (idx >= backgroundImg.Length) idx = 0;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SlideIsRunning = false;
        }
    }
}
