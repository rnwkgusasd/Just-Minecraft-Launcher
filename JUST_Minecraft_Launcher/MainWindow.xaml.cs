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
using System.Windows.Media.Animation;
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

            while (SlideIsRunning)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {

                    DoubleAnimation da = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = new Duration(TimeSpan.FromSeconds(4)),
                        AutoReverse = true
                    };

                    DoubleAnimation da2 = new DoubleAnimation
                    {
                        From = 1,
                        To = 2,
                        Duration = new Duration(TimeSpan.FromSeconds(4)),
                        AutoReverse = true
                    };

                    slideShowIMG.BeginAnimation(OpacityProperty, da2);
                    slideShowIMG.BeginAnimation(OpacityProperty, da);

                    slideShowIMG.Source = backgroundImg[idx];
                }));

                idx++;

                if (idx >= backgroundImg.Length) idx = 0;

                while(true)
                {
                    if (!mr.WaitOne(8000))
                    {
                        break;
                    }
                }
            }
        }

        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SlideIsRunning = false;
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Foreground != Brushes.Black)
            {
                tb.Background = Brushes.White;
                tb.Foreground = Brushes.Black;
                tb.Text = "";
            }
        }

        private string pwdText = "";

        private void textBox_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            string t = "";

            for(int i = 0; i < tb.Text.Length; i++)
            {
                t += "*";
            }

            tb.Text = t;
            tb.CaretIndex = tb.Text.Length;
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            bool loginOK = true;

            if(id.Text == "" || id.Foreground != Brushes.Black)
            {
                loginOK = false;
                id.Background = Brushes.Tomato;
                id.Foreground = Brushes.White;
            }
            if(pwd.Text == "" || pwd.Foreground != Brushes.Black)
            {
                loginOK = false;
                pwd.Background = Brushes.Tomato;
                pwd.Foreground = Brushes.White;
            }

            if(!loginOK)
            {
                return;
            }

            MessageBox.Show("");
        }
    }
}
