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
    public class RegistrationPage : CarouselPage
    {
        public RegistrationPage()
        {
            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            var padding = new Thickness(20, 20, 20, 20);

            Padding = padding;

            //Step1
            var logoImage1 = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };

            var labStep1 = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Registration_Step1,
                TextColor = Color.Black,
                FontSize = 20
            };

            var labTitle = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Title,
                TextColor = Color.Black,
                FontSize = 20
            };

            var labFirst = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.FirstName,
                TextColor = Color.Black,
                FontSize = 20
            };

            var labLast = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.LastName,
                TextColor = Color.Black,
                FontSize = 20
            };

            var titleEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 460,
                TextColor = Color.Black,
                FontSize = 20,
            };

            var firstEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 460,
                TextColor = Color.Black,
                FontSize = 20,
            };

            var lastEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 460,
                TextColor = Color.Black,
                FontSize = 20,
            };

            var titleSl = new StackLayout
            {
                Children =
                {
                    labTitle,
                    titleEntry
                }
            };

            var firstSl = new StackLayout
            {
                Children =
                {
                    labFirst,
                    firstEntry
                }
            };

            var lastSl = new StackLayout
            {
                Children =
                {
                    labLast,
                    lastEntry
                }
            };

            Button btnBack = new Button
            {
                Text = AppResources.Back,
                TextColor = Color.Black,
                BackgroundColor = Color.Transparent,
                WidthRequest = 120,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Start,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2
            };
            Button btnCancel = new Button
            {
                Text = AppResources.Cancel,
                TextColor = Color.Black,
                BackgroundColor = Color.Transparent,
                WidthRequest = 120,
                HorizontalOptions = LayoutOptions.Center,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2
            };
            Button btnNext = new Button
            {
                Text = AppResources.Next,
                TextColor = Color.Black,
                BackgroundColor = Color.Transparent,
                WidthRequest = 120,
                HorizontalOptions = LayoutOptions.End,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2
            };

            var btnSl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnBack } },
                     new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnCancel } },
                      new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnNext } }
                }
            };

            var grid1 = new Grid();
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            grid1.Children.Add(logoImage1, 0, 0);
            grid1.Children.Add(labStep1, 0, 0);

            grid1.Children.Add(titleSl, 0, 1);

            grid1.Children.Add(firstSl, 0, 2);

            grid1.Children.Add(lastSl, 0, 3);

            grid1.Children.Add(btnSl, 0, 4);
            //grid1.Children.Add(btnBack, 0, 4);
            //grid1.Children.Add(btnCancel, 0, 4);
            //grid1.Children.Add(btnNext, 0, 4);

            //Step2
            var logoImage2 = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };
            var labStep2 = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Registration_Step2,
                TextColor = Color.Black,
                FontSize = 20
            };
            var grid2 = new Grid();
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid2.Children.Add(logoImage2, 0, 0);
            grid2.Children.Add(labStep2, 0, 0);

            var step1ContentPage = new ContentPage
            {
                Content = grid1
            };

            var step2ContentPage = new ContentPage
            {
                Content = grid2
            };

            Children.Add(step1ContentPage);
            Children.Add(step2ContentPage);

            //Content = grid;
        }
    }
}
