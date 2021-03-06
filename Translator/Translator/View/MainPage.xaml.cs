﻿using System;
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
using Microsoft.WindowsAzure.MobileServices;
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
           //save the photo to a directory
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                Directory = "Translator",
                Name = $"{DateTime.UtcNow}.jpg",
                CompressionQuality = 92
            });

            if (file == null)
            {
                return;
            }
            //Process the photo
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

                    //use the FindWordType() to find out the word type of the text.
                    FindWordType(textMsg);

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
                    this.IsBusy = false;
                }
                catch (Exception)
                {

                }
            }
            

        }
        /// <summary>
        /// Upload a text string to Yandex translator api to return a json string containing the translation.
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


                //retrieve the json object and store it as a single string
                string result = await client.GetStringAsync(url);
                return result;
            }
            catch(Exception)
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
            

                //get the ocr result from the api
                OcrResults textResult = await VisionServiceClient.RecognizeTextAsync(stream, "en", true);
                return textResult;
            }
            catch (Exception)
            {
                return null;
            }

            
        }
        /// <summary>
        /// Get the learn vocab from the easy table hosted in azure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnGetVocab(object sender, EventArgs e)
        {
            this.IsBusy = true;
           //initialize the result;
            string result = null;
            //Get the list of content in json from azure.
            List<VocabModel> content = await AzureManager.AzureManagerInstance.GetVocabModel();

            //deserialize the json object and parse it into result string.
            foreach (VocabModel model in content)
            {
                result += model.SourceText+"\n";
            }
            this.IsBusy = false;
            await DisplayAlert("Do you remember these words?", result, "Ok");
        }

        /// <summary>
        /// Find out the each word type in a sentence.
        /// </summary>
        /// <param name="text"></param>
        private async void FindWordType(string text)
        {
            try
            {
                //set the loading indicator to true.
                //this.IsBusy = true;
                //initialize http client 
                var client = new HttpClient();
                string SourceText = text;
                //parse the uri string
                string uri = "https://api.textgain.com/1/tag?lang=en&q=" + SourceText;
                string result = await client.GetStringAsync(uri);

                //deserialize the json object returned from calling the api.
                var s = (Text)JsonConvert.DeserializeObject(result, typeof(Text));
                foreach (List<List<WordType>> section in s.data)
                {
                    foreach (List<WordType> sentence in section)
                    {
                        foreach (WordType wordType in sentence)
                        {
                            string word = wordType.word;
                            string tag = wordType.tag;
                            //if the word is a noun, then save it to the easy table else not.
                            if (tag == "NOUN")
                            {
                                //serialize the data into a json object.
                                VocabModel vocabModel = new VocabModel()
                                {
                                    SourceText = word
                                    
                                };
                                //use POST method to send the json object to the easy table
                                await AzureManager.AzureManagerInstance.AddVocabModel(vocabModel);
                            }
                        }
                    }
                }
                //turn the loading indicator off.
               // this.IsBusy = false;
            }
            catch (Exception) { }
        }
    }
    
    
}
