using AppointMaster.Controls;
using AppointMaster.iOS.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]
namespace AppointMaster.iOS.CustomRenderer
{
    public class MyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //Control.BorderStyle = UIKit.UITextBorderStyle.Line;
                Control.Layer.BorderColor = UIKit.UIColor.Black.CGColor;
            }
        }
    }
}
