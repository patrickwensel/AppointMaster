using AppointMaster.Controls;
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
    public class RegistrationPage : IndexedPages
    {
        private RegistrationViewModel RegistrationViewModel
        {
            get { return BindingContext as RegistrationViewModel; }
        }

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
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20,
            };

            var firstEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20,
            };

            var lastEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20,
            };

            var titleSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    labTitle,
                    titleEntry
                }
            };

            var firstSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    labFirst,
                    firstEntry
                }
            };

            var lastSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    labLast,
                    lastEntry
                }
            };

            Button btnStep1Back = new Button
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
            Button btnStep1Cancel = new Button
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
            Button btnStep1Next = new Button
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

            var btnStep1Sl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep1Back } },
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep1Cancel } },
                    new StackLayout {Children= { btnStep1Next } }
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

            grid1.Children.Add(btnStep1Sl, 0, 4);

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

            var labStreet = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Street_Adress,
                TextColor = Color.Black,
                FontSize = 20
            };

            var labCity = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.City,
                TextColor = Color.Black,
                FontSize = 20
            };

            var labState = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.State,
                TextColor = Color.Black,
                FontSize = 20
            };

            var labZip = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Zip,
                TextColor = Color.Black,
                FontSize = 20
            };

            var streetEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20,
            };

            var cityEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20,
            };

            var stateEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 200,
                TextColor = Color.Black,
                FontSize = 20
            };

            var zipEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 200,
                TextColor = Color.Black,
                FontSize = 20
            };

            var streetSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    labStreet,
                    streetEntry
                }
            };

            var citySl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                   labCity,
                   cityEntry
                }
            };

            var stateSl = new StackLayout
            {
                Padding = new Thickness(0, 0, 60, 0),
                Children =
                {
                   labState,
                   stateEntry
                }
            };

            var zipSl = new StackLayout
            {
                Children =
                {
                   labZip,
                   zipEntry
                }
            };

            var stateAndZipSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                  stateSl,
                  zipSl
                }
            };

            Button btnStep2Back = new Button
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
            Button btnStep2Cancel = new Button
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
            Button btnStep2Next = new Button
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

            var btnStep2Sl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep2Back } },
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep2Cancel } },
                    new StackLayout { Children= { btnStep2Next } }
                }
            };

            var grid2 = new Grid();
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            grid2.Children.Add(logoImage2, 0, 0);
            grid2.Children.Add(labStep2, 0, 0);

            grid2.Children.Add(streetSl, 0, 1);

            grid2.Children.Add(citySl, 0, 2);

            grid2.Children.Add(stateAndZipSl, 0, 3);

            grid2.Children.Add(btnStep2Sl, 0, 4);

            //Step3
            var logoImage3 = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };
            var labStep3 = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Registration_Step2,
                TextColor = Color.Black,
                FontSize = 20
            };

            var labPhone = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Phone,
                TextColor = Color.Black,
                FontSize = 20
            };

            var labEmial = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Email,
                TextColor = Color.Black,
                FontSize = 20
            };

            var phoneEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20
            };

            var emailEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20
            };

            var phoneSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    labPhone,
                    phoneEntry
                }
            };

            var emailSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                      labEmial,
                      emailEntry
                }
            };

            Button btnStep3Back = new Button
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
            Button btnStep3Cancel = new Button
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
            Button btnStep3Next = new Button
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

            var btnStep3Sl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep3Back } },
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep3Cancel } },
                    new StackLayout {Children= { btnStep3Next } }
                }
            };

            var grid3 = new Grid();
            grid3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            grid3.Children.Add(logoImage3, 0, 0);
            grid3.Children.Add(labStep3, 0, 0);

            grid3.Children.Add(phoneSl, 0, 1);

            grid3.Children.Add(emailSl, 0, 2);

            grid3.Children.Add(btnStep3Sl, 0, 3);

            //Step4

            var step1ContentPage = new ContentPage
            {
                Content = grid1
            };

            var step2ContentPage = new ContentPage
            {
                Content = grid2
            };

            var step3ContentPage = new ContentPage
            {
                Content = grid3
            };

            Children.Add(step1ContentPage);
            Children.Add(step2ContentPage);
            Children.Add(step3ContentPage);

            btnStep1Next.Clicked += delegate { /*CurrentPage = step2ContentPage;*/RegistrationViewModel.SelectedPageIndex += 1; };
            InputTransparent = true;
            SetBinding(IndexedPages.SelectedIndexProperty, new Binding("SelectedPageIndex"));
            //Content = grid;
        }
    }
}
