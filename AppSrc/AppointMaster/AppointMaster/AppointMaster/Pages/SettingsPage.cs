using AppointMaster.Controls;
using AppointMaster.Resources;
using AppointMaster.Services;
using AppointMaster.ViewModels;
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
        private SettingsViewModel SettingsViewModel
        {
            get { return BindingContext as SettingsViewModel; }
        }

        public SettingsPage()
        {
            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            Padding = new Thickness(100, Device.OnPlatform(40, 20, 20), 100, 20);

            Image imgLogo = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };
            if (DataHelper.GetInstance().Clinic.Logo != null)
            {
                imgLogo.Source = ImageSource.FromStream(() => new System.IO.MemoryStream(DataHelper.GetInstance().Clinic.Logo));
            }

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

            grid.Children.Add(imgLogo, 0, 0);

            grid.Children.Add(new Label { Text = AppResources.Settings, TextColor = Color.Black, FontSize = 30,FontFamily="Bold" },0,1);

            grid.Children.Add(apiSL, 0, 2);

            grid.Children.Add(btnSave, 0, 3);

            apiEntry.SetBinding(Entry.TextProperty, new Binding("BaseAPIAddress"));
            btnSave.SetBinding(Button.CommandProperty, new Binding("SaveCommand"));

            Content = grid;

            MessagingCenter.Subscribe<SettingsViewModel, string>(this, "DisplayAlert", (sender, value) =>
            {
                DisplayAlert(AppResources.Error, AppResources.Enter_BaseAPI, AppResources.OK);
            });
        }
    }
}
