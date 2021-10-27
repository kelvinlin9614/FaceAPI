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
        private int imageCount = 148;
        /*
         * Function: convert a video to frames(extract every second thumbnails)
         * Store all the pictures into the image array
         * Return Image array
         */
        public Stream[] VideoToStreams(string baseDir, string videoName, string picDirName)
        {
            // ConvertVideoToFrames(baseDir, videoName, picDirName);
            return ConvertFrameToStreams(baseDir + picDirName);
        }
        public void ConvertVideoToFrames(string baseDir, string videoName, string picDirName)
        {
            // create a new directory(will store all the picture into it)
            // Image[] mn = new Image[1000];
            DirectoryInfo di = Directory.CreateDirectory(baseDir + picDirName);
            var inputFile = new MediaFile { Filename = baseDir + videoName};
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
                    var outputFile = new MediaFile { Filename = baseDir + picDirName + @"\" + i.ToString() + ".jpg" };
                    Console.WriteLine("Generating " + baseDir + picDirName + @"\" + i.ToString() + ".jpg");
                    engine.GetThumbnail(inputFile, outputFile, options);
                    //mn[i] = Image.FromFile(baseDir + picDirName + @"\" + i.ToString() + ".jpg");
                    
                    imageCount = i;
                }
            }
        }
        // Function: convert a image to byte array
        public byte[] ImageToByteArray(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }
        public Stream[] ConvertFrameToStreams(string picDir)
        {
            // create a new directory(will store all the picture into it)
            Stream[] mn = new Stream[1000];
            var inputFile = new Image[imageCount];
            Console.WriteLine(imageCount);
            for (int i = 0; i < imageCount; i++)
            {
                inputFile[i] = Image.FromFile(picDir + @"\" +  i.ToString() + ".jpg");
                mn[i] = new MemoryStream();
                mn[i].Write(ImageToByteArray(inputFile[i]), 0, ImageToByteArray(inputFile[i]).Length);
            }
            return mn;
        }


    }
}
