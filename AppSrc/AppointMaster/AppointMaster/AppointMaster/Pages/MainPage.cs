using AppointMaster.Resources;
using AppointMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace AppointMaster.Pages
{
    public class MainPage : ContentPage
    {
        private MainViewModel MainViewModel
        {
            get { return BindingContext as MainViewModel; }
        }

        public MainPage()
        {
            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            //var padding = new Thickness(0, Device.OnPlatform(40, 40, 0), 0, 0);
            var padding = new Thickness(20, 20, 20, 20);

            var label1 = new Label
            {
                Text = "Some Veterinary Clinic",
                FontSize = 20,
                TextColor = Color.Black
            };

            var label2 = new Label
            {
                Text = "123 Some Road",
                FontSize = 20,
                TextColor = Color.Black
            };

            var label3 = new Label
            {
                Text = "Somewhereville, US 12345",
                FontSize = 20,
                TextColor = Color.Black
            };

            var image = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var sl = new StackLayout
            {
                Children = {
                    label1,
                    label2,
                    label3,
                }
            };

            var slReg = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Children = {
                   new Image { Source="clipbaord.png",WidthRequest=50,HeightRequest=68,VerticalOptions=LayoutOptions.Center},
                   new Label { Text=AppResources.Registration,VerticalOptions=LayoutOptions.Center,TextColor=Color.Black,FontSize=22}
                }
            };

            slReg.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ShowCheckInView())
            });

            var slDis = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.End,
                Children = {
                   new Label { Text=AppResources.Disclaimer, TextColor = Color.Black,FontSize=20},
                   new Label { Text=AppResources.Disclaimer_Details, TextColor = Color.Black}
                }
            };

            //var parentReg = new StackLayout
            //{
            //    BackgroundColor = Color.Black,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //};

            //parentReg.Children.Add(slReg);

            Padding = padding;

            grid.Children.Add(sl, 0, 0);
            grid.Children.Add(image, 0, 0);
            grid.Children.Add(slReg, 0, 1);
            grid.Children.Add(slDis, 0, 2);

            Content = grid;
        }

        private void ShowCheckInView()
        {
            MainViewModel.ShowCheckInCommand.Execute();
        }
    }
}
