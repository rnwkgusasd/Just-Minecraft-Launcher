using CmlLib.Launcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace JUST_Minecraft_Launcher
{
    public class Minecraft
    {
        private string _path = "";
        private string _startVersion = "1.12.2-forge-14.23.5.2854";

        private MSession session;

        public Minecraft(string path)
        {
            _path = path;
        }

        public bool MinecraftLogin(string id, string pwd)
        {
            CmlLib.Launcher.Minecraft.Initialize(_path);

            MLogin login = new MLogin();

            session = login.Authenticate(id, pwd);

            if (session.Result != MLoginResult.Success)
            {
                session = login.Authenticate(id, pwd);

                if (session.Result != MLoginResult.Success)
                    return false;
            }

            return true;
        }

        public void MinecraftStart(string serverIP)
        {
            MProfile profile = MProfile.FindProfile(MProfileInfo.GetProfiles(), _startVersion);

            DownloadGame(profile);

            string ip = "34.64.69.9"; //"34.64.225.34"; //serverType == SERVER_TYPE.BOSUNG ? "183.105.245.179" : "49.246.50.208";

            MLaunchOption option = new MLaunchOption()
            {
                StartProfile = profile,
                JavaPath = "java.exe",
                MaximumRamMb = 8192,
                Session = session,
                ServerIp = ip
            };

            DirectoryInfo d = new DirectoryInfo(_path + "\\mods");
            DirectoryInfo d2 = new DirectoryInfo(_path + "\\resourcepacks");

            //Directory.Delete(d.FullName);
            //Directory.Delete(d2.FullName);

            //Directory.CreateDirectory(d.FullName);
            //Directory.CreateDirectory(d2.FullName);

            string modePath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\MODS";
            string resourcePath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\RESOURCE";

            if (!Directory.Exists(modePath)) Directory.CreateDirectory(modePath);
            if (!Directory.Exists(resourcePath)) Directory.CreateDirectory(resourcePath);
            
            FileInfo[] files = Directory.GetFiles(modePath).ToList().ConvertAll(x => new FileInfo(x)).ToArray();

            foreach (var item in files)
            {
                if (!File.Exists(d.FullName + "\\" + item.Name))
                    item.CopyTo(d.FullName + "\\" + item.Name);
            }

            FileInfo[] files2 = Directory.GetFiles(resourcePath).ToList().ConvertAll(x => new FileInfo(x)).ToArray();

            foreach (var item in files2)
            {
                if (!File.Exists(d2.FullName + "\\" + item.Name))
                    item.CopyTo(d2.FullName + "\\" + item.Name);
            }

            MLaunch launch = new MLaunch(option);
            launch.GetProcess().Start();

            System.Windows.Application.Current.Shutdown();
        }

        public System.ComponentModel.ProgressChangedEventHandler Downloader_ChangeProgress;
        public DownloadFileChangedHandler Downloader_ChangeFile;

        public void DownloadGame(MProfile profile)
        {
            MDownloader downloader = new MDownloader(profile);

            downloader.ChangeFile += Downloader_ChangeFile;
            downloader.ChangeProgress += Downloader_ChangeProgress;
            downloader.DownloadAll();
        }
    }
}
