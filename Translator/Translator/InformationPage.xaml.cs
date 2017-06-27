using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Translator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InformationPage : ContentPage
    {
        public InformationPage()
        {
            InitializeComponent();
            
        }
        private async void BtnGetVocab(object sender, EventArgs e)
        {

            /*MobileServiceClient client = new MobileServiceClient("http://translatorjw.azurewebsites.net");
            //IMobileServiceTable<FaceBookModel> model = client.GetTable<FaceBookModel>();
            IMobileServiceTable<VocabModel> vocabTable = client.GetTable<VocabModel>();
            var content = await vocabTable.ToListAsync();*/
            var content = await AzureManager.AzureManagerInstance.GetVocabModel();
        }

        private async void BtnAddVocab(object sender, EventArgs e)
        {
            VocabModel vocabModel = new VocabModel()
            {
                SourceText = "orange",
                TranslateText = "書"
            };
            await AzureManager.AzureManagerInstance.AddVocabModel(vocabModel);
        }
    }
    
}