using AppointMaster.Resources;
using AppointMaster.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace AppointMaster.Pages
{
    public class CheckedInPage : ContentPage
    {
        public CheckedInPage()
        {
            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            Padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20);

            Image imgLogo = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "svc_logo.png",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };
            if (DataHelper.GetInstance().Clinic.Logo != null)
            {
                imgLogo.Source = ImageSource.FromStream(() => new System.IO.MemoryStream(DataHelper.GetInstance().Clinic.Logo));
            }

            Button btnMain = new Button
            {
                Text = AppResources.Main_Menu,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2,
                BackgroundColor = DataHelper.GetInstance().SecondaryColor,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions= LayoutOptions.End,
                WidthRequest = 350,
                HeightRequest = 50,
                TextColor=Color.Black
            };
            btnMain.SetBinding(Button.CommandProperty, new Binding("ShowMainCommand"));

            Button btnRegistration = new Button
            {
                Text = AppResources.Registration,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2,
                BackgroundColor = DataHelper.GetInstance().SecondaryColor,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End,
                WidthRequest = 350,
                HeightRequest=50,
                TextColor = Color.Black
            };
            btnRegistration.SetBinding(Button.CommandProperty, new Binding("ShowRegistertionCommand"));

            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height= 150});
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(imgLogo, 0, 0);

            grid.Children.Add(new StackLayout
            {
                Padding = new Thickness(0, 100, 0, 1),
                HorizontalOptions = LayoutOptions.Center,
                Children =
                        {
                            new Label
                            {
                                Text = AppResources.All_Checked_In,
                                TextColor=Color.Black,
                                FontSize=25,
                                HorizontalOptions=LayoutOptions.Center
                            },
                            new Label
                            {
                                Text = AppResources.Return_Tablet,
                                TextColor=Color.Black,
                                FontSize=25,
                                HorizontalOptions=LayoutOptions.Center
                            },
                        }
            }, 0, 1);

            grid.Children.Add(btnMain, 0, 2);
            grid.Children.Add(btnRegistration, 0, 2);

            Content = grid;
        }
    }
}
