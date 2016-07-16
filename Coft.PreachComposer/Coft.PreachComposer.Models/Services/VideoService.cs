using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coft.PreachComposer.Models.Services
{
    public class VideoService : IVideoService
    {
        private Process ffmpegProcess { get; set; }
        private Action<int> progressCallback { get; set; }

        public void AttachProgressAction(Action<int> progressCallback)
        {
            this.progressCallback = progressCallback;
        }

        public void CreateVideo(string imagePath, string audioPath, string outputPath)
        {
            using (ffmpegProcess = new Process())
            {
                ffmpegProcess.StartInfo.FileName = "ffmpeg.exe";
                ffmpegProcess.StartInfo.Arguments = $"-loop 1 -i {imagePath} -i {audioPath} -c:v libx264 -tune stillimage -c:a aac -strict experimental -b:a 192k -pix_fmt yuv420p -shortest {outputPath}";
                ffmpegProcess.StartInfo.UseShellExecute = false;
                ffmpegProcess.StartInfo.RedirectStandardOutput = true;
                ffmpegProcess.OutputDataReceived += FfmpegProcess_OutputDataReceived;
                ffmpegProcess.Start();
                ffmpegProcess.BeginOutputReadLine();

                

                //ffmpegProcess.StandardOutput.ReadLine();

                ffmpegProcess.WaitForExit();
            }
        }

        private void FfmpegProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (progressCallback != null)
            {
                Console.WriteLine(e.Data);
            }
        }
    }
}
