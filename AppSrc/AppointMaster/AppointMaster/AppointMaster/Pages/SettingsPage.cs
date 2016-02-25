using AppointMaster.Controls;
using AppointMaster.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace AppointMaster.Pages
{
    public class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            Padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20);

            var logoImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 100,
                WidthRequest = 207
            };

            var apiEntry = new MyEntry
            {
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20
            };

            Button btnSave = new Button
            {
                Text = AppResources.Save,
                WidthRequest = 470,
                HeightRequest = 50,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2,
                BackgroundColor = Color.Transparent,
                TextColor = Color.Black,
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(140) });

            grid.Children.Add(logoImage, 0, 0);
            grid.Children.Add(new Label { Text = AppResources.Settings, TextColor = Color.Black, FontSize = 25 });

            Content = grid;
        }
    }
}
