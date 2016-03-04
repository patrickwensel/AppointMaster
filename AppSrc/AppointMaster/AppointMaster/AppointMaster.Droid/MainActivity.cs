using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Platform;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Presenter.Droid;
using MvvmCross.Core.ViewModels;
using Xamarin.Forms.Platform.Android;
using XLabs.Platform.Device;
using XLabs.Ioc;
using XLabs.Platform.Services;

namespace AppointMaster.Droid
{
    [Activity(Label = "FormsApplicationActivity", ScreenOrientation = ScreenOrientation.Portrait)]//, Theme = "@style/Theme.NoTitle"
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            var mvxFormsApp = new MvxFormsApp();
            LoadApplication(mvxFormsApp);

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsDroidPagePresenter;
            presenter.MvxFormsApp = mvxFormsApp;

            var resolverContainer = new SimpleContainer();
            resolverContainer.Register<IDevice>(t => AndroidDevice.CurrentDevice)
                .Register<ISecureStorage>(t => new KeyVaultStorage(t.Resolve<IDevice>().Id.ToCharArray()));

            Resolver.SetResolver(resolverContainer.GetResolver());

            Mvx.Resolve<IMvxAppStart>().Start();
        }
    }
}

