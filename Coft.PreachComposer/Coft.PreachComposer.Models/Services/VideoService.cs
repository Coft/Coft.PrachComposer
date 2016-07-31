using NLog;
using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private StreamReader reader;
        private Logger logger = LogManager.GetCurrentClassLogger();


        public VideoService()
        {
            
        }

        public void AttachProgressAction(Action<int> progressCallback)
        {
            this.progressCallback = progressCallback;
        }

        public void CreateVideo(string imagePath, string audioPath, string outputPath)
        {
            var ffmpeg = new FFMpegConverter();

            FFMpegInput imageInput = new FFMpegInput(imagePath);
            imageInput.CustomInputArgs = "-loop 1";
            FFMpegInput audioInput = new FFMpegInput(audioPath);

            FFMpegInput[] inputs = new FFMpegInput[] {
                imageInput,
                audioInput
            };

            OutputSettings settings = new OutputSettings();
            settings.CustomOutputArgs = "-c:v libx264 -tune stillimage -c:a aac -strict experimental -b:a 192k -pix_fmt yuv420p -shortest";

            ffmpeg.ConvertProgress += Ffmpeg_ConvertProgress;
            ffmpeg.ConvertMedia(inputs, outputPath, Format.mp4, settings);

            //using (ffmpegProcess = new Process())
            //{
            //    ffmpegProcess.StartInfo.FileName = "ffmpeg.exe";
            //    ffmpegProcess.StartInfo.Arguments = $"-loop 1 -i {imagePath} -i {audioPath} -c:v libx264 -tune stillimage -c:a aac -strict experimental -b:a 192k -pix_fmt yuv420p -shortest {outputPath}";
            //    ffmpegProcess.StartInfo.UseShellExecute = false;
            //    ffmpegProcess.StartInfo.RedirectStandardOutput = true;
            //    ffmpegProcess.StartInfo.CreateNoWindow = true;
            //    //ffmpegProcess.StartInfo.RedirectStandardError = true;
            //    ffmpegProcess.OutputDataReceived += FfmpegProcess_OutputDataReceived;
            //    ffmpegProcess.Start();

            //    ffmpegProcess.BeginOutputReadLine();
            //    //Task.Run(() => {


            //    //    reader = ffmpegProcess.StandardOutput;
            //    //    while (progressCallback != null)
            //    //    {
            //    //        Thread.Sleep(500);
            //    //        string line = reader.ReadLine();

            //    //        logger.Debug(line);
            //    //    }
            //    //});

            //    ffmpegProcess.WaitForExit();

            //    ffmpegProcess.OutputDataReceived -= FfmpegProcess_OutputDataReceived;

            //    progressCallback = null;
            //}
        }

        private void Ffmpeg_ConvertProgress(object sender, ConvertProgressEventArgs e)
        {
            logger.Debug($"{e.Processed}/{e.TotalDuration}");
        }

        private void FfmpegProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //if (progressCallback != null)
            {
                logger.Debug(e.Data);
            }
        }
    }
}
