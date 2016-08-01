using System;

namespace Coft.PreachComposer.Models.Services
{
    public interface IVideoService
    {
        bool CreateVideo(string imagePath, string audioPath, string outputPath);
        void AttachProgressAction(Action<int> progressCallback);
    }
}