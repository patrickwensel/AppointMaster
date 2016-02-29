﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.iOS.Platform;
using XLabs.Forms.Controls;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Geolocation;
using XLabs.Ioc;
using XLabs.Forms;

namespace AppointMaster.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        UIWindow _window;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //global::Xamarin.Forms.Forms.Init();
            ////LoadApplication(new App());

            //return base.FinishedLaunching(app, options);

            //var container = new SimpleContainer();
            //container.Register<IDevice>(t => AppleDevice.CurrentDevice);
            //container.Register<IGeolocator, Geolocator>();
            //Resolver.SetResolver(container.GetResolver());
            var r = new GridViewRenderer();

            _window = new UIWindow(UIScreen.MainScreen.Bounds);

            var setup = new Setup(this, _window);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            _window.MakeKeyAndVisible();

            return true;
        }
    }
}
