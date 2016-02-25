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
            Padding = new Thickness(100, Device.OnPlatform(40, 20, 20), 100, 20);

            var logoImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                VerticalOptions=LayoutOptions.Start,
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

            var btnCheckIn = new Button
            {
                WidthRequest = 200,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Start,
                Text = string.Format("< {0}", AppResources.Check_In),
                TextColor = Color.Black,
                FontSize = 20,
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2
            };

            StackLayout apiSL = new StackLayout
            {
                Children =
                {
                   new Label { Text=AppResources.Base_API_Address,TextColor=Color.Black,FontSize=20},
                   apiEntry
                }
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1,GridUnitType.Star) });

            grid.Children.Add(logoImage, 0, 0);

            grid.Children.Add(new Label { Text = AppResources.Settings, TextColor = Color.Black, FontSize = 30,FontFamily="Bold" },0,1);

            grid.Children.Add(apiSL, 0, 2);

            grid.Children.Add(btnSave, 0, 3);

            grid.Children.Add(btnCheckIn, 0, 4);

            apiEntry.SetBinding(Entry.TextProperty, new Binding("BaseAPIAddress"));
            btnSave.SetBinding(Button.CommandProperty, new Binding("SaveCommand"));
            btnCheckIn.SetBinding(Button.CommandProperty, new Binding("ShowCheckInCommand"));

            Content = grid;
        }
    }
}
