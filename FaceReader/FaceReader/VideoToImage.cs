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
        private int imageCount = 0;
        /*
         * Function: convert a video to frames(extract every second thumbnails)
         * Store all the pictures into the image array
         * Return Image array
         */
        public Stream[] VideoToStreams(string currentDir, string videoFileName, string newDir)
        {
            ConvertVideoToFrames(currentDir, videoFileName, newDir);
            return ConvertFrameToStreams(newDir);
        }
        public Image[] ConvertVideoToFrames(string currentDir, string videoFileName, string newDir)
        {
            // create a new directory(will store all the picture into it)
            Image[] mn = new Image[1000];
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
                    
                    //I think currentDir should be reomved 
                    var outputFile = new MediaFile { Filename = currentDir + newDir + @"\" + i.ToString() + ".jpg" };
                    Console.WriteLine(currentDir + newDir + @"\" + i.ToString() + ".jpg");
                    
                    engine.GetThumbnail(inputFile, outputFile, options);
                    
                    //I think currentDir should be removed 
                    mn[i] = Image.FromFile(currentDir + newDir + @"\" + i.ToString() + ".jpg");
                    
                    imageCount = i;
                }
            }
            return mn;
        }
        public Stream[] ConvertFrameToStreams(string currentDir)
        {
            // create a new directory(will store all the picture into it)
            Stream[] mn = new Stream[1000];
            Image[] inputFile = new Image[imageCount];
            for (int i = 0; i <= imageCount; i++)
            {
                inputFile[i] = Image.FromFile(currentDir + i.ToString() + ".jpg");
                mn[i].Write(ImageToByteArray(inputFile[i]), 0, ImageToByteArray(inputFile[i]).Length);
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
