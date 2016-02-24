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
using Xamarin.Forms.Platform.Android;

namespace AppointMaster.Droid.CustomRenderer
{
    public class MyProgressBarRenderer:ProgressBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //Control.Background = Resources.GetDrawable(Resource.Drawable.bg_entry);
                //Control.SetTextColor(Android.Graphics.Color.Black);
                //Control.Gravity = GravityFlags.CenterVertical;
            }
        }
    }
}