using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
namespace Translator
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        async void TakePhoto(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.Text = "You click it";

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                Directory = "Moodify",
                Name = $"{DateTime.UtcNow}.jpg",
                CompressionQuality = 92
            });

            if (file == null)
            {
                return;
            }
            else
            {
                //await DisplayAlert("Name", file.Path, "djfadsklj");
                //var result = await UploadAndAnalyzeImage("https://en.wikipedia.org/wiki/Shogi#/media/File:Shogi_board_pieces_and_komadai.jpg");
            }
            try
            {
                VisionServiceClient VisionServiceClient = new VisionServiceClient("3bd57c38ada74daeb2d11c859f1c36bb");
                //VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
                string textMsg="";
                var stream = file.GetStream();
                //AnalysisResult analysisResult = await VisionServiceClient.AnalyzeImageAsync(stream, visualFeatures);
                //var captions = analysisResult.Description.Captions;
                OcrResults textResult = await VisionServiceClient.RecognizeTextAsync(stream, "en", true);
                
                foreach (Region region in textResult.Regions)
                {
                    foreach (Line line in region.Lines)
                    {
                        foreach (Word word in line.Words)
                        {
                            textMsg += word.Text + " ";
                        }
                    }
                }
                /*if (textMsg == "")
                {
                    HandwritingRecognitionOperation handWriteOp = await VisionServiceClient.CreateHandwritingRecognitionOperationAsync(stream);
                    HandwritingRecognitionOperationResult handWriteResult = await VisionServiceClient.GetHandwritingRecognitionOperationResultAsync(handWriteOp);
                    //handWriteResult.RecognitionResult.Lines[0].Words[0].Text;
                    
                    foreach(HandwritingTextLine line in handWriteResult.RecognitionResult.Lines)
                    {
                        foreach(HandwritingTextWord word in line.Words)
                        {
                            textMsg += word.Text + " ";
                        }
                    }

                }*/

                await DisplayAlert("Your text:", textMsg,"Cancel");
                var client = new HttpClient();
                string text = "text=" + textMsg;
                string lang = "lang=" + "en-zh";
                string translatorKey = "key=" + "trnsl.1.1.20170505T130736Z.9886d1e879de6303.6a534fd32397ecba37a3c120449d094ba135e6a1";
                string uri = "https://translate.yandex.net/api/v1.5/tr.json/translate?" + translatorKey + "&" + text + "&" + lang;
                
                //HttpResponseMessage response = await client.GetAsync(uri);
                string result = await client.GetStringAsync(uri);

                var translatedTextModel = JsonConvert.DeserializeObject<TranslateTextModel>(result);
                string translatedTextMsg = "";
                //string[] translatedText = translatedTextModel.TranslatedText;
                foreach (string translatedText in translatedTextModel.TranslatedText)
                {
                    translatedTextMsg += translatedText;
                }
                await DisplayAlert("Translated Text:", translatedTextMsg, "Cancel");
                /*while (analysisResult.Description.Captions[i] != null)
                {
                   string textmsg = analysisResult.Description.Captions[i].Text;
                    i++;
                }*/



            }
            catch (Exception ex)
            {

            }

        }
        
        /*private async Task<AnalysisResult> UploadAndAnalyzeImage(string imageFilePath)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------  
            //
            // Create Project Oxford Computer Vision API Service client
            //
            VisionServiceClient VisionServiceClient = new VisionServiceClient("3bd57c38ada74daeb2d11c859f1c36bb");
            VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
            AnalysisResult analysisResult = await VisionServiceClient.AnalyzeImageAsync(imageFilePath, visualFeatures);
            return analysisResult;



        }*/
    }
}
