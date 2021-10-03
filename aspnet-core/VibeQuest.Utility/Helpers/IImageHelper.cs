using VibeQuest.Utility.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Helpers
{
    public interface IImageHelper: ITransientDependency
    {
        Tuple<int, int> GetImageSize(Stream stream);
        void ResizeAndSaveImage(Stream stream, int newWidth, int newHeight, string imagePath);
        Stream ResizeAndReturnStream(Stream file, int newWidth, int newHeight);
    }
}
