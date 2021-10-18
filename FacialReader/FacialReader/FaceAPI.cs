using System;
using System.IO;
using System.Drawing;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace FacialReader
{
    class FaceAPI
    {

        // Sets the read-only value of a file.
        public static void SetFileReadAccess(string FileName, bool SetReadOnly)
        {
            // Create a new FileInfo object.
            FileInfo fInfo = new FileInfo(FileName);

            // Set the IsReadOnly property.
            fInfo.IsReadOnly = SetReadOnly;
        }

        /*
         * Function: convert a video to frames(extract every second thumbnails)
         * Store all the pictures into the image array
         * Return Image array
         */
        public static Image[] ConvertVideoToFrames(Image[] mn, string videoDir, string outDir)
        {
            //SetFileReadAccess(videoDir, true);
            var inputFile = new MediaFile { Filename = @videoDir };
            Console.WriteLine("IM here!");

            using (var engine = new Engine())
            {
                Console.WriteLine("IM here!");
                engine.GetMetadata(inputFile);
                // get the total length of the video(seconds)
                double duration = inputFile.Metadata.Duration.TotalSeconds;

                // extract every second thumbnail, store all the thumbnails into specific dir
                // store all the thumbnails into the Image array.
                for (int i = 0; i < duration; i++)
                {
                    var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(i) };
                    var outputFile = new MediaFile { Filename = @outDir + i.ToString() + ".jpg" };
                    engine.GetThumbnail(inputFile, outputFile, options);
                    mn[i] = Image.FromFile(@outDir + i.ToString() + ".jpg");
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
            //Console.WriteLine("Enter Video Directory: ");
            //string videoDir = Console.ReadLine();
            var videoDir = "Users/kevinlin/Desktop/happy.mp4";
            //Console.WriteLine("Enter Output Directory: ");
            //string outDir = Console.ReadLine();
            var outDir = "Users/kevinlin/Desktop/FaceApi/";
            Image[] mn = new Image[1000];
            mn = ConvertVideoToFrames(mn, videoDir, outDir);
        }
    }
}
