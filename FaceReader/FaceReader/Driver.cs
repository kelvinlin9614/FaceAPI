using System;
using System.IO;

namespace FaceReader
{
    class Driver
    {
        public static void Main(string[] args)
        {
            // obtain user's current directory(base directory)
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("Please copy your video into this directory: " + baseDir);
            Console.WriteLine("Enter Video File Name: ");
            string videoName = Console.ReadLine();
            Console.WriteLine("Enter a directory name(Pictures will store in this directory)");
            string picDirName = Console.ReadLine();

            // initialize a stream array
            Stream[] imageStreamArray;
            VideoToImage obj = new VideoToImage();
            // convert video to stream array, store stream array to imageStreamArray
            imageStreamArray = obj.VideoToStreams(baseDir, videoName, picDirName);

            // detect face
            EmotionReader emotion = new EmotionReader();
            emotion.DetectFaceExtract(imageStreamArray).Wait();
            Console.WriteLine("End of Detection.");
        }
    }
}
