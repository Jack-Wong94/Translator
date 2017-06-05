using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Translator
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Translator.MainPage();
        }
        public interface IAuthenticate
        {
            Task<bool> Authenticate();
            Task<bool> LogoutAsync();
        }
        public static IAuthenticate Authenticator { get; private set; }
        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
