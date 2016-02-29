using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Xamarin.Forms;
using AppointMaster.Controls;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(MyGridView), typeof(AppointMaster.Droid.CustomRenderer.MyGridViewRenderer))]
namespace AppointMaster.Droid.CustomRenderer
{
    public class MyGridViewRenderer: XLabs.Forms.Controls.GridViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<GridView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.White);
                Control.SetBackground(Resources.GetDrawable( Resource.Color.white));
            }
        }
    }
}