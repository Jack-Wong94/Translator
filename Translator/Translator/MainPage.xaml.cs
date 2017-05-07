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
using System.Net.Http;
using Newtonsoft.Json;
namespace Translator
{
    /// <summary>
    /// The main page of the translator app.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// When the user click the button (defined in the xaml file), this method is called to take the photo and translate the text in the photo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TakePhoto(object sender, EventArgs e)
        {
            //Set up the camera permission.
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
            if (!this.IsBusy)
            {
                try
                {
                    this.IsBusy = true;
                    //initizlize the text message. It is a string of the recognized text from the computer vision api
                    string textMsg = "";

                    //Get the ocr result from the UploadAndAnalyzeImage();
                    OcrResults textResult = await UploadAndAnalyzeImage(file);
                    this.IsBusy = false;
                    //Loop through the text result and parse it in a single string.
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

                    //Display the text msg.
                    //In progress: change the ui to allow autocorrect or manually correct.
                    await DisplayAlert("Your text:", textMsg, "Cancel");

                    //In progress: Allow the user to discard the image.

                    //use the yandex translator api to get the string of response json object.
                    string result = await UploadAndTranslate(textMsg);

                    //Deserialize the json object and string the translated text msg into a string array.
                    var translatedTextModel = JsonConvert.DeserializeObject<TranslateTextModel>(result);

                    //initialize the translated text msg string. Ready to be parsed.
                    string translatedTextMsg = "";

                    //Loop through every string in the string array to parse the string.
                    foreach (string translatedText in translatedTextModel.TranslatedText)
                    {
                        translatedTextMsg += translatedText;
                    }

                    //Display the final result.
                    await DisplayAlert("Translated Text:", translatedTextMsg, "Cancel");

                }
                catch (Exception ex)
                {

                }
            }
            

        }
        /// <summary>
        /// Upload a text string to Yandex translator api to return a json string containing the translation;
        /// </summary>
        /// <param name="textMsg"></param>
        /// <returns></returns>
        private async Task<string> UploadAndTranslate(string textMsg)
        {
            try
            {
                //Create a http client
                var client = new HttpClient();

                //Set up the require parameter (i.e. text to translated, language to use, and the subscription key)
                string text = "text=" + textMsg;
                string lang = "lang=" + "en-zh";
                string translatorKey = "key=" + "trnsl.1.1.20170505T130736Z.9886d1e879de6303.6a534fd32397ecba37a3c120449d094ba135e6a1";

                //Parse it into a string of url 
                string url = "https://translate.yandex.net/api/v1.5/tr.json/translate?" + translatorKey + "&" + text + "&" + lang;

                //HttpResponseMessage response = await client.GetAsync(url);

                //retrieve the json object and store it as a single string
                string result = await client.GetStringAsync(url);
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Upload a media image file to microsoft computer vision api to get ocr result.
        /// Ocr result is a analysis of word recognition in an image.
        /// return the ocr result to get the text.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<OcrResults> UploadAndAnalyzeImage(MediaFile file)
        {
            try
            {
                //Set up the computer vision client.
                VisionServiceClient VisionServiceClient = new VisionServiceClient("3bd57c38ada74daeb2d11c859f1c36bb");
                //VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
                
                //Convert the image into file stream
                var stream = file.GetStream();
                //AnalysisResult analysisResult = await VisionServiceClient.AnalyzeImageAsync(stream, visualFeatures);
                //var captions = analysisResult.Description.Captions;

                //get the ocr result from the api
                OcrResults textResult = await VisionServiceClient.RecognizeTextAsync(stream, "en", true);
                return textResult;
            }
            catch (Exception ex)
            {
                return null;
            }

            //Not finished. It is used to recognize handwritten text.

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
        }
        
    }
}
