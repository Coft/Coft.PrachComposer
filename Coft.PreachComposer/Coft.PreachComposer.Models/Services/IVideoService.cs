using System;

namespace Coft.PreachComposer.Models.Services
{
    public interface IVideoService
    {
        void CreateVideo(string imagePath, string audioPath, string outputPath);
        void AttachProgressAction(Action<int> progressCallback);
    }
}