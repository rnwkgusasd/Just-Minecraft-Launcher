using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            //miniImg.Source = new BitmapImage(new Uri(@"Resource\minimize.png", UriKind.RelativeOrAbsolute));

            login.BorderBrush = Brushes.DarkSeaGreen;
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

        private void DoMinimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SlideStart();

            mine = new Minecraft(Environment.GetEnvironmentVariable("APPDATA") + "\\.minecraft");
            mine.Downloader_ChangeFile = Downloader_ChangeFile;
            mine.Downloader_ChangeProgress = Downloader_ChangeProgress;

            LoadConfig();
        }

        private void Downloader_ChangeProgress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void Downloader_ChangeFile(CmlLib.Launcher.DownloadFileChangedEventArgs e)
        {

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
                tb.BorderBrush = Brushes.LightSeaGreen;
                tb.Text = "";
            }

            tb.BorderThickness = new Thickness(4);
        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox tb = (PasswordBox)sender;
            if (tb.Foreground != Brushes.Black)
            {
                tb.Background = Brushes.White;
                tb.Foreground = Brushes.Black;
                tb.BorderBrush = Brushes.LightSeaGreen;
                tb.Password = "";
            }

            tb.BorderThickness = new Thickness(4);
        }

        private void textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.BorderThickness = new Thickness(1);
        }

        private void passwordbox_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox tb = (PasswordBox)sender;
            tb.BorderThickness = new Thickness(1);
        }

        public string pwdText = "";

        private void textBox_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            pwdText = tb.Text;

            //string t = "";

            //for(int i = 0; i < tb.Text.Length; i++)
            //{
            //    t += "*";
            //}

            //tb.Text = t;
            //tb.CaretIndex = tb.Text.Length;
        }

        private Minecraft mine;
        private void login_Click(object sender, RoutedEventArgs e)
        {
            if(login.Content.ToString() == "LOGIN")
            {
                bool loginOK = true;

                if (id.Text == "" || id.Foreground != Brushes.Black)
                {
                    loginOK = false;
                    id.Background = Brushes.Tomato;
                    id.Foreground = Brushes.White;
                    id.BorderBrush = Brushes.Red;
                }
                if (pwd2.Password == "" || pwd2.Foreground != Brushes.Black)
                {
                    loginOK = false;
                    pwd2.Background = Brushes.Tomato;
                    pwd2.Foreground = Brushes.White;
                    pwd2.BorderBrush = Brushes.Red;
                }

                if (!mine.MinecraftLogin(id.Text, pwd2.Password))
                {
                    loginOK = false;
                    pwd2.Background = Brushes.Tomato;
                    pwd2.Foreground = Brushes.White;
                    pwd2.BorderBrush = Brushes.Red;

                    id.Background = Brushes.Tomato;
                    id.Foreground = Brushes.White;
                    id.BorderBrush = Brushes.Red;
                }

                if (!loginOK)
                {
                    return;
                }

                login.Content = "START";
            }
            else
            {
                mine.MinecraftStart("");
            }

            SaveConfig();
        }

        private string FILE_PATH = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\config.cfg";

        private string SERVER_IP = "";
        private bool AUTO_LOGIN = false;

        private void SaveConfig()
        {
            string txt = "";

            foreach(var c in pwd2.Password.Reverse())
            {
                txt += c;
            }

            File.WriteAllLines(FILE_PATH, new string[] { $"AUTO_LOGIN:{checkBox.IsChecked}", $"CONNECT_SERVER_IP:{SERVER_IP}",
                                                        $"LOGIN_ID:{id.Text}", $"LOGIN_PWD:{txt}"});
        }

        private void LoadConfig()
        {
            if(!File.Exists(FILE_PATH))
            {
                File.Create(FILE_PATH);
            }

            string[] rData = File.ReadAllLines(FILE_PATH);

            if(rData.Length > 0)
            {
                Boolean.TryParse(rData[0].Split(':')[1], out AUTO_LOGIN);
                SERVER_IP = rData[1].Split(':')[1];

                checkBox.IsChecked = AUTO_LOGIN;

                if (AUTO_LOGIN)
                {
                    id.Focus();
                    pwd2.Focus();

                    id.Text = rData[2].Split(':')[1];

                    string txt = "";

                    foreach (var c in rData[3].Split(':')[1].Reverse())
                    {
                        txt += c;
                    }

                    pwd2.Password = txt;
                    login_Click(null, null);
                }
            }
        }

        private void MouseLeftButtonDown_FrmMove(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        Brush mBrush;

        private void login_MouseEnter(object sender, MouseEventArgs e)
        {
            if(mBrush == null)
                mBrush = loginBorder.BorderBrush;

            loginBorder.BorderBrush = Brushes.LightCyan;
        }

        private void login_MouseLeave(object sender, MouseEventArgs e)
        {
            loginBorder.BorderBrush = mBrush;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
