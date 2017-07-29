using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
namespace Translator
{
    /// <summary>
    /// A manager class manage the data transfer between the app and the easy table hosted in azure. 
    /// </summary>
    public class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<VocabModel> vocabTable;
        /// <summary>
        /// Constructor method
        /// </summary>
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
        /// <summary>
        /// GET method
        /// </summary>
        /// <returns></returns>
        public async Task<List<VocabModel>> GetVocabModel()
        {
            return await this.vocabTable.ToListAsync();
        }
        /// <summary>
        /// POST method
        /// </summary>
        /// <param name="vocabModel"></param>
        /// <returns></returns>
        public async Task AddVocabModel(VocabModel vocabModel)
        {
            await this.vocabTable.InsertAsync(vocabModel);
        }
    }
}
