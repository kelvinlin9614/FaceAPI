using System;
using System.IO;
using System.Drawing;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace FaceReader
{
    class VideoToImage
    {
        /*
         * Function: convert a video to frames(extract every second thumbnails)
         * Store all the pictures into the image array
         * Return Image array
         */
        public static Image[] ConvertVideoToFrames(Image[] mn, string currentDir, string videoFileName, string newDir)
        {
            // create a new directory(will store all the picture into it)
            DirectoryInfo di = Directory.CreateDirectory(currentDir + newDir);
            var inputFile = new MediaFile { Filename = currentDir + videoFileName};
            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                // get the total length of the video(seconds)
                double duration = inputFile.Metadata.Duration.TotalSeconds;
                // extract every second thumbnail, store all the thumbnails into specific dir
                // store all the thumbnails into the Image array.
                for (int i = 0; i < duration; i++)
                {
                    var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(i) };
                    var outputFile = new MediaFile { Filename = currentDir + newDir + @"\" + i.ToString() + ".jpg" };
                    Console.WriteLine("Generating " + currentDir + newDir + @"\" + i.ToString() + ".jpg");
                    engine.GetThumbnail(inputFile, outputFile, options);
                    mn[i] = Image.FromFile(currentDir + newDir + @"\" + i.ToString() + ".jpg");
                }
            }
            return mn;
        }

        // Function: convert a image to byte array
        public static byte[] ImageToByteArray(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }

    }
}
