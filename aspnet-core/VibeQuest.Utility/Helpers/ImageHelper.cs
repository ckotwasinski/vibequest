using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VibeQuest.Utility.Helpers
{
    public class ImageHelper : IImageHelper
    {
        public Tuple<int, int> GetImageSize(Stream stream)
        {
            Image<Rgba32> img = Image.Load<Rgba32>(stream);
            return new Tuple<int, int>(img.Width, img.Height);
        }

        public void ResizeAndSaveImage(Stream stream, int newWidth, int newHeight, string imagePath)
        {
            Image<Rgba32> image = Image.Load<Rgba32>(stream);

            if (image.Width < image.Height)
            {
                newHeight = image.Height * newWidth / image.Width;
            }
            else
            {
                newWidth = image.Width * newHeight / image.Height;
            }

            image.Mutate(x => x
                     .Resize(newWidth, newHeight)
                 );

            image.Save(imagePath);
        }

        public Stream ResizeAndReturnStream(Stream file, int newWidth, int newHeight)
        {
            Image<Rgba32> image = Image.Load<Rgba32>(file);

            if (image.Width < image.Height)
            {
                newHeight = image.Height * newWidth / image.Width;
            }
            else
            {
                newWidth = image.Width * newHeight / image.Height;
            }

            var outputStream = new MemoryStream();
            image.Mutate(x => x
                     .Resize(newWidth, newHeight)
                 );

            image.SaveAsJpeg(outputStream);
            return outputStream;
        }
    }
}
