using System;
using System.IO;
using System.Reflection;


namespace KingVonPasswordManager
{
    internal class MusicPlayer
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream stream;
        string tempfile;
        WMPLib.WindowsMediaPlayer WindowsMediaPlayer = new WMPLib.WindowsMediaPlayer();
        public MusicPlayer()
        {
            try
            {
                stream = assembly.GetManifestResourceStream("Notifications_host_process.Resources.king.wav");
                tempfile = Path.Combine(Path.GetTempPath(), "king.wav");

                using (FileStream filestream = new FileStream(tempfile, FileMode.Create, FileAccess.Write))
                {

                    stream.CopyTo(filestream);

                }

                WindowsMediaPlayer.URL = tempfile;
                WindowsMediaPlayer.settings.setMode("loop", true);
                WindowsMediaPlayer.settings.volume = 10;
                WindowsMediaPlayer.controls.play();

            }
            catch (FileNotFoundException e) { Console.WriteLine("ERROR: File Not Found! "); }
            catch (IOException e) { Console.WriteLine("ERROR: I/O error!"); }
            catch (Exception e) { Console.WriteLine("ERROR: something unexpected happened!"); }
        }

        public void StartPlayer() { WindowsMediaPlayer.controls.play(); }

        public void StopPlayer()
        {
            WindowsMediaPlayer.controls.stop();
        }
    }
}
