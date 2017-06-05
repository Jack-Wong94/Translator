using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
namespace Translator
{
    public class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<VocabModel> vocabTable;
        private AzureManager()
        {
            this.client = new MobileServiceClient("http://translatorjw.azurewebsites.net");
            this.vocabTable = this.client.GetTable<VocabModel>();
        }
        public MobileServiceClient AzureClient
        {
            get { return client; }
        }
        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }
                return instance;
            }
        }
        public async Task<List<VocabModel>> GetVocabModel()
        {
            return await this.vocabTable.ToListAsync();
        }
        public async Task AddVocabModel(VocabModel vocabModel)
        {
            await this.vocabTable.InsertAsync(vocabModel);
        }
    }
}
