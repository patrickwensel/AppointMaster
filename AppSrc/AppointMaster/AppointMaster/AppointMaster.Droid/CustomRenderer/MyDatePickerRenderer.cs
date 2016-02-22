using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using AppointMaster.Controls;
using AppointMaster.Droid.CustomRenderer;

[assembly: ExportRenderer(typeof(MyDatePicker), typeof(MyDatePickerRenderer))]
namespace AppointMaster.Droid.CustomRenderer
{
    class MyDatePickerRenderer: DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = Resources.GetDrawable(Resource.Drawable.bg_entry);
                Control.SetTextColor(Android.Graphics.Color.Black);
                Control.Gravity = GravityFlags.CenterVertical;
            }
        }
    }
}