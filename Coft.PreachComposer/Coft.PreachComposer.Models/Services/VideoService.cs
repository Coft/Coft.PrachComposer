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

        public void AttachProgressAction(Action<int> progressCallback)
        {
            this.progressCallback = progressCallback;
        }

        public bool CreateVideo(string imagePath, string audioPath, string outputPath)
        {
            bool isSucess = false;

            try
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

                isSucess = true;
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            return isSucess;
        }

        private void Ffmpeg_ConvertProgress(object sender, ConvertProgressEventArgs e)
        {
            int percentage = e.TotalDuration.Seconds * 100 / e.Processed.Seconds;

            progressCallback?.Invoke(percentage);
        }
    }
}
