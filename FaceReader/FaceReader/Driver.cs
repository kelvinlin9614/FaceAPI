using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Text;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace FaceReader
{
    class Driver
    {
        public static void Main(string[] args)
        {
            // obtain current directory(base directory)

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            //Console.WriteLine("Please copy your video into this directory: " + baseDir);
            //Console.WriteLine("Enter Video File Name: ");
            //string videoName = Console.ReadLine();
            //Console.WriteLine("Enter a directory name(Pictures will store in this directory)");
            //string picDirName = Console.ReadLine();

            //Video to Stream

            Stream[] mn = new Stream[1];
            var obj = new VideoToImage();
            //mn = obj.VideoToStreams(baseDir, videoName, picDirName);
            Image test = Image.FromFile("happyFace.jpg");
            mn[0] = obj.ImageToStream(test);

            ////Emotion Reader
            EmotionReader emotion = new EmotionReader();
            emotion.DetectFaceExtract(emotion.getFaceClient(), mn, emotion.getRecognitionModel()).Wait();


        }
    }
}
