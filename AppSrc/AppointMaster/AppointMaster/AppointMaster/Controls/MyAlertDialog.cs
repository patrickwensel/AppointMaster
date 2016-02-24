using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace AppointMaster.Controls
{
    public class MyAlertDialog : PopupLayout
    {
        public MyAlertDialog(string msg)
        {
            WidthRequest = 350;
            HeightRequest = 300;

            if (IsPopupActive)
            {
                DismissPopup();
            }
            else
            {
                Button btn = new Button
                {
                    Text = "OK"
                };
                StackLayout sl = new StackLayout
                {
                    Children =
                    {
                       new Label
                       {
                            Text=msg,
                       },
                       btn
                    }
                };

                ShowPopup(sl);
            }
        }
    }
}
