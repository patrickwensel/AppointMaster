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

            var labStep1= new Label
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

            //var titleEntry = new ExtendedEntry
            //{
            //    VerticalOptions = LayoutOptions.Start,
            //    HorizontalOptions = LayoutOptions.Start,
            //    HeightRequest=40,
            //    WidthRequest = 150,
            //    TextColor = Color.Black,
            //    FontSize = 20,
            //    BackgroundColor=Color.White,
            //    HasBorder=false
            //};

            var titleEntry = new ExtendedEntry()
            {
                Text = "and From code",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                PlaceholderTextColor = Color.Red,
                HasBorder = false,
                XAlign = TextAlignment.Center
            };

            var grid1 = new Grid();
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid1.Children.Add(logoImage1, 0, 0);
            grid1.Children.Add(labStep1, 0, 0);

            grid1.Children.Add(labTitle, 0, 1);

            grid1.Children.Add(titleEntry, 0, 2);

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
