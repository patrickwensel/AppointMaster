using AppointMaster.Controls;
using AppointMaster.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyListView), typeof(MyListViewRenderer))]
namespace AppointMaster.iOS.CustomRenderer
{
    public class MyListViewRenderer: ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                var tableView = Control as UITableView;
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            }
        }
    }
}
