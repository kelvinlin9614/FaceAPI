using System;
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
        public EmotionReader()
        {
            this.SUBSCRIPTION_KEY = "257176d0-727a-4831-8360-c43b80260aa1";
            this.ENDPOINT = "7a3a8212c72642b5a7b6156cdd13db1c";
            //this.ENDPOINT = "https://randomname.cognitiveservices.azure.com/";
            this.client = Authenticate(this.ENDPOINT, this.SUBSCRIPTION_KEY);
            this.RECOGNITION_MODEL4 = RecognitionModel.Recognition04;
        }

        public IFaceClient getFaceClient()
        {
            return this.client;
        }

        public string getRecognitionModel()
        {
            return this.RECOGNITION_MODEL4;
        }
        // </snippet_creds>
        /**
        public static void Main(string[] args)
        {

            const string RECOGNITION_MODEL4 = RecognitionModel.Recognition04;

            IFaceClient client = Authenticate(ENDPOINT, SUBSCRIPTION_KEY);

            byte[] imageBytes = File.ReadAllBytes(@"C:\FaceAPI\pic\imageName.jpg");
            Stream stream = new MemoryStream(imageBytes);

            //save the image as stream and pass it as an argument 
            DetectFaceExtract(client, stream, RECOGNITION_MODEL4).Wait();

        }*/

        public IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        public async Task DetectFaceExtract(IFaceClient client, Stream []mn, string recognitionModel)
        {
            IList<DetectedFace> detectedFaces;
            // Detect faces with all attributes from stream.
            for(int i = 0; i < mn.Length; i++)
            {
            //detectedFaces = await client.Face.DetectWithStreamAsync(mn[i],
            //returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Accessories, FaceAttributeType.Age,
            //            FaceAttributeType.Blur, FaceAttributeType.Emotion, FaceAttributeType.Exposure, FaceAttributeType.FacialHair,
            //            FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose,
            //            FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion, FaceAttributeType.Smile },
            //            // We specify detection model 1 because we are retrieving attributes.
            //            detectionModel: DetectionModel.Detection01,
            //            recognitionModel: recognitionModel);
                detectedFaces = await client.Face.DetectWithStreamAsync(mn[i],true,false, returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Accessories, FaceAttributeType.Age,
                            FaceAttributeType.Blur, FaceAttributeType.Emotion, FaceAttributeType.Exposure, FaceAttributeType.FacialHair,
                            FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose,
                            FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion, FaceAttributeType.Smile }, recognitionModel,true,DetectionModel.Detection01);
                Console.WriteLine($"{detectedFaces.Count} face(s) detected from image '{i}'.");

                foreach (var face in detectedFaces)
                {
                    // Get bounding box of the faces
                    Console.WriteLine($"Rectangle(Left/Top/Width/Height) : " +
                        $"{face.FaceRectangle.Left} {face.FaceRectangle.Top} {face.FaceRectangle.Width} {face.FaceRectangle.Height}");
                    // Get emotion on the face
                    string emotionType = string.Empty;
                    double emotionValue = 0.0;
                    Emotion emotion = face.FaceAttributes.Emotion;
                    if (emotion.Anger > emotionValue) { emotionValue = emotion.Anger; emotionType = "Anger"; }
                    if (emotion.Contempt > emotionValue) { emotionValue = emotion.Contempt; emotionType = "Contempt"; }
                    if (emotion.Disgust > emotionValue) { emotionValue = emotion.Disgust; emotionType = "Disgust"; }
                    if (emotion.Fear > emotionValue) { emotionValue = emotion.Fear; emotionType = "Fear"; }
                    if (emotion.Happiness > emotionValue) { emotionValue = emotion.Happiness; emotionType = "Happiness"; }
                    if (emotion.Neutral > emotionValue) { emotionValue = emotion.Neutral; emotionType = "Neutral"; }
                    if (emotion.Sadness > emotionValue) { emotionValue = emotion.Sadness; emotionType = "Sadness"; }
                    if (emotion.Surprise > emotionValue) { emotionType = "Surprise"; }
                    Console.WriteLine($"Emotion : {emotionType}");
                    Console.WriteLine();
                }
            }
        }


        public async Task DeletePersonGroup(IFaceClient client, string personGroupId)
        {
            await client.PersonGroup.DeleteAsync(personGroupId);
            Console.WriteLine($"Deleted the person group {personGroupId}.");
        }
        /*
		 * END - DELETE PERSON GROUP
		 */
    }
}