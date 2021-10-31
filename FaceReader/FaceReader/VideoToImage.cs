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
        private int imageCount = 0;       // store how many images we have
        /*
         * Function: convert a video to frames(extract every second thumbnails)
         * Store all the pictures into the image array
         * Return Image array
         */
        public Stream[] VideoToStreams(string baseDir, string videoName, string picDirName)
        {
            ConvertVideoToFrames(baseDir, videoName, picDirName);
            return ConvertFramesToStreams(baseDir + picDirName);
        }

        // convert video to frames, store all the frames to local directory, return the number of total frames
        private void ConvertVideoToFrames(string baseDir, string videoName, string picDirName)
        {
            MediaFile inputFile = new MediaFile { Filename = baseDir + videoName };
            
            // if the directory does not exist, then create a new directory
            if (!Directory.Exists(baseDir + picDirName))
            {
                // create a new directory(will store all the picture into it)
                Directory.CreateDirectory(baseDir + picDirName);
            }
            using (Engine engine = new Engine())
            {
                // get meta data from the video
                engine.GetMetadata(inputFile);
                // get the total length of the video(seconds)
                double duration = inputFile.Metadata.Duration.TotalSeconds;
                // extract every 4 second thumbnail, store all the thumbnails into specific directory
                for (int i = 0; i < duration; i+=4)
                { 
                    var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(i) };
                    var outputFile = new MediaFile { Filename = baseDir + picDirName + @"\" + imageCount.ToString() + ".jpg" };
                    Console.WriteLine("Generating " + baseDir + picDirName + @"\" + imageCount.ToString() + ".jpg");
                    engine.GetThumbnail(inputFile, outputFile, options);
                    imageCount++;
                }
            }
        }

        // use filestream to read each frame and store them to stream array, return stream array
        private Stream[] ConvertFramesToStreams(string picDir)
        {
            FileStream[] fs = new FileStream[this.imageCount];
            for (int i = 0; i < imageCount; i++)
            {
                fs[i] = File.OpenRead(picDir + @"\" + i.ToString() + ".jpg");
            }
            return fs;
        }

        //// Function: convert a image to byte array
        //public byte[] ImageToByteArray(Image x)
        //{
        //    ImageConverter _imageConverter = new ImageConverter();
        //    byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
        //    return xByte;
        //}
    }
}
