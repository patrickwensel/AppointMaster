using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using AppointMaster.Droid.CustomRenderer;
using AppointMaster.Controls;

[assembly: ExportRenderer(typeof(MyPicker), typeof(MyPickerRenderer))]
namespace AppointMaster.Droid.CustomRenderer
{
    class MyPickerRenderer: PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = Resources.GetDrawable(Resource.Drawable.bg_entry);
                Control.SetTextColor(Android.Graphics.Color.Black);
            }
        }
    }
}