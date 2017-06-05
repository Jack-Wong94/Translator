using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;

using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using static Translator.App;
using Android.Webkit;

namespace Translator.Droid
{
    [Activity(Label = "Translator", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,IAuthenticate
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            App.Init((IAuthenticate)this);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private MobileServiceUser user;
        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                user = await AzureManager.AzureManagerInstance.AzureClient.LoginAsync(this, MobileServiceAuthenticationProvider.Facebook);
                if (user != null)
                {
                    message = string.Format("You are now signed-in as {0}", user.UserId);
                    success = true;
                }
            }catch(Exception ex)
            {
                message = ex.Message;
            }
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();
            return success;
        }

        public async Task<bool> LogoutAsync()
        {
            var message = string.Empty;
            var success = false;
            try
            {
                CookieManager.Instance.RemoveAllCookie();
                await AzureManager.AzureManagerInstance.AzureClient.LogoutAsync();
                success = true;
            }catch(Exception ex)
            {
                message = ex.Message;
            }
            return success;
        }
    }
}

