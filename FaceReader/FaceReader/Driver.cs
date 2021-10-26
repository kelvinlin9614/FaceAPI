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
            // obtain current directory
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("Please copy your video into this directory: " + currentDir);
            Console.WriteLine("Enter Video File Name: ");
            string videoName = Console.ReadLine();
            Console.WriteLine("Enter a directory name(Pictures will store in this directory)");
            string newDir = Console.ReadLine();
            Stream[] mn = new Stream[1000];
            VideoToImage obj = new VideoToImage();
            mn = obj.VideoToStreams(currentDir, videoName, newDir);
        }
    }
}
