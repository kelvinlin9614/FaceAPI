using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace FaceReader
{
    class EmotionReader
    {
        private string SUBSCRIPTION_KEY;
        private string ENDPOINT;
        private IFaceClient client;
        private string RECOGNITION_MODEL4;
        private string emotion;
       
        public EmotionReader()
        {
            this.SUBSCRIPTION_KEY = "7a3a8212c72642b5a7b6156cdd13db1c";
            this.ENDPOINT = "https://randomname.cognitiveservices.azure.com/";
            this.client = Authenticate(this.ENDPOINT, this.SUBSCRIPTION_KEY);
            this.RECOGNITION_MODEL4 = RecognitionModel.Recognition04;
            this.emotion = "";
        }
        public String GetEmotion()
        {
            return emotion;
        }

        private IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        //get the most occuring element 
        private int FindMostOccuringElement(int[] array, Hashtable hs)
        {
            int mostCommon = array[0];
            int occurance = 0;
            foreach (int num in array)
            {
                if (!hs.ContainsKey(num))
                {
                    hs.Add(num, 1);
                }
                else
                {
                    int tempOccurance = (int)hs[num];
                    tempOccurance++;
                    hs.Remove(num);
                    hs.Add(num, tempOccurance);
                    if (occurance < tempOccurance)
                    {
                        occurance = tempOccurance;
                        mostCommon = num;
                    }
                }
            }
            return mostCommon;
        }

        //get the emotion of all the image of the video
        private string GetHeaviestEmotion(IList<DetectedFace> imageList)
        {
            if(imageList.Count() != 0)
            {
                return imageList.FirstOrDefault().FaceAttributes.Emotion.ToRankedList().FirstOrDefault().Key;
            }
            else
            {
                return "null";
            }
        }


        //get integer value for the given emotions 
        private int GetReplyText(string emotion)
        {
            if (emotion == "Surprise" || emotion == "Happiness" || emotion == "Neutral")
            {
                return 1;
            }
            else if (emotion == "Anger" || emotion == "Disgust")
            {
                return 2;
            }
            else if (emotion == "Fear")
            {
                return 3;
            }
            else if (emotion == "Sadness" || emotion == "Contempt")
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }

        //get the mood type 
        private string MoodType(int emotionLevel)
        {
            if (emotionLevel == 1)
            {
                return "Confident";
            }
            else if (emotionLevel == 2)
            {
                return "Angry";
            }
            else if (emotionLevel == 3)
            {
                return "Nervous";
            }
            else if (emotionLevel == 4)
            {
                return "Sad";
            }
            else
            {
                return " ";
            }
        }


        public async Task DetectFaceExtract(Stream[] imageStream)
        {
            //int anger, contempt, disgust, fear, happiness, neutral, sadness, surprise;
            //anger = contempt = disgust = fear = happiness = neutral = sadness = surprise = 0;
            Console.WriteLine("========DETECT FACES========");
            Console.WriteLine();

            //save the moods in the array
            int[] imageMoods = new int[imageStream.Length];

            for (int i = 0; i < imageStream.Length; i++)
            {
                IList<DetectedFace> detectedFaces;
                // Detect faces with all attributes from image stream.
                detectedFaces = await this.client.Face.DetectWithStreamAsync(imageStream[i],
                    returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Emotion },
                    // We specify detection model 1 because we are retrieving attributes.
                    detectionModel: DetectionModel.Detection01,
                    recognitionModel: this.RECOGNITION_MODEL4);
                Console.WriteLine($"{detectedFaces.Count} face(s) detected from {i}.jpg.");
                //save the highest score of the emotion for the given image 
                imageMoods[i] = GetReplyText(GetHeaviestEmotion(detectedFaces));
            }

            Hashtable hs = new Hashtable();
            //get the average emotion of all the images 
            int mood = FindMostOccuringElement(imageMoods, hs);
            this.emotion = MoodType(mood);
            Console.WriteLine();
            //print the average emotion of all the images 
            Console.WriteLine("===========================");
            Console.WriteLine("The most occuring emotion: " + MoodType(mood));
            Console.WriteLine("===========================");
            Console.WriteLine();
        }
    }
}
