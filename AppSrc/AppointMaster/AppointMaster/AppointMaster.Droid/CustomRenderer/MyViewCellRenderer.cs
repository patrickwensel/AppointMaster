using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using AppointMaster.Controls;

[assembly: ExportRenderer(typeof(MyViewCell), typeof(AppointMaster.Droid.CustomRenderer.MyViewCellRenderer))]
namespace AppointMaster.Droid.CustomRenderer
{
    public class MyViewCellRenderer : ViewCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            Android.Views.View cell = base.GetCellCore(item, convertView, parent, context);
            cell.SetBackgroundColor(Android.Graphics.Color.White);
            return cell;
        }
    }
}