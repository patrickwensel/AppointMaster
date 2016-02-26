using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AppointMaster.Controls.MyViewCell), typeof(AppointMaster.iOS.CustomRenderer.MyViewCellRenderer))]
namespace AppointMaster.iOS.CustomRenderer
{
    public class MyViewCellRenderer:ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }
    }
}
