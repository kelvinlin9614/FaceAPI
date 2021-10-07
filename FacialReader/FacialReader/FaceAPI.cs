using System;
using System.Drawing;
using System.IO;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace FacialReader
{
    class FaceAPI
    {
        /*
         * Function: convert a video to frames(extract every second thumbnails)
         * Store all the pictures into the image array
         * Return Image array
         */
        public static Image[] convertVideoToFrames(string fileName)
        {
            Image[] mn = new Image[1000];
            var inputFile = new MediaFile { Filename = fileName };
            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                // get the total length of the video(seconds)
                double duration = inputFile.Metadata.Duration.TotalSeconds;

                // extract every second thumbnail, store all the thumbnails into C:\FaceAPI\pic\"
                // store all the thumbnails into the Image array.
                for (int i = 0; i < duration; i++)
                {
                    var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(i) };
                    var outputFile = new MediaFile { Filename = @"C:\FaceAPI\pic\" + i.ToString() + ".jpg" };
                    engine.GetThumbnail(inputFile, outputFile, options);
                    mn[i] = Image.FromFile(@"C:\FaceAPI\pic\" + i.ToString() + ".jpg");
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

        public static void Main(string[] args)
        {
            Image[] mn = new Image[1000];
            mn = convertVideoToFrames(@"C:\FaceAPI\habi.mp4");
        }
    }
}
