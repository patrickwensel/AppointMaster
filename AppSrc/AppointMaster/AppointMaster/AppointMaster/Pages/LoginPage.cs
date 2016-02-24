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
    public class LoginPage : ContentPage
    {
        private LoginViewModel LoginViewModel
        {
            get { return BindingContext as LoginViewModel; }
        }

        public LoginPage()
        {
            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            Padding = Padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20);

            var logoImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 100,
                WidthRequest = 207
            };

            var userEntry = new MyEntry
            {
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20
            };

            var passwordEntry = new MyEntry
            {
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20,
            };

            StackLayout userSl = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        Text = AppResources.Login,
                        TextColor = Color.Black,
                        FontSize = 20
                    },
                    userEntry
                }
            };

            StackLayout passwordSl = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        Text = AppResources.Password,
                        TextColor = Color.Black,
                        FontSize = 20
                    },
                    passwordEntry
                }
            };

            Button btnLogin = new Button
            {
                Text = AppResources.Login,
                WidthRequest = 150,
                HeightRequest = 50,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2,
                BackgroundColor = Color.Transparent,
                TextColor = Color.Black,
            };

            var grid = new Grid();
            grid.HorizontalOptions = LayoutOptions.Center;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(140) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            grid.Children.Add(logoImage, 0, 0);

            grid.Children.Add(userSl, 0, 1);

            grid.Children.Add(passwordSl, 0, 2);

            grid.Children.Add(btnLogin, 0, 3);

            userEntry.SetBinding(Entry.TextProperty, new Binding("UserName"));
            passwordEntry.SetBinding(Entry.TextProperty, new Binding("Password"));
            btnLogin.SetBinding(Button.CommandProperty, new Binding("LoginCommand"));

            MessagingCenter.Subscribe<LoginViewModel, string>(this, "DisplayAlert", (sender, value) => {
                DisplayAlert(AppResources.Error, value, AppResources.OK);
            });

            Content = grid;
        }


    }
}
