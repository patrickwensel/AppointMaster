using AppointMaster.Controls;
using AppointMaster.Resources;
using AppointMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using XLabs.Forms.Controls;

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
            var padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20);

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

            Image logoImage = new Image
            {
                Source = "logo.png",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };

            StackLayout logoImageSl = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };

            var sl = new StackLayout
            {
                Padding=padding,
                Children = {
                    label1,
                    label2,
                    label3,
                }
            };

            var slReg = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = {
                   new Image { Source="clipbaord.png",WidthRequest=50,HeightRequest=68},
                   new Label { Text=AppResources.Registration,VerticalOptions=LayoutOptions.Center,TextColor=Color.Black,FontSize=22}
                }
            };

            slReg.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ShowCheckInView())
            });

            var registrationGrid = new Grid();
            registrationGrid.HorizontalOptions = LayoutOptions.Center;
            registrationGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
            registrationGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
            registrationGrid.Children.Add(new Button
            {
                BorderColor = Color.Black,
                BorderWidth = 2,
                BorderRadius = 1,
                BackgroundColor = Color.Transparent,
            }, 0, 0);
            registrationGrid.Children.Add(slReg, 0, 0);

            var slDis = new StackLayout
            {
                Padding = padding,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.End,
                Children = {
                   new Label { Text=AppResources.Disclaimer, TextColor = Color.Black,FontSize=20},
                   new Label { Text=AppResources.Disclaimer_Details, TextColor = Color.Black}
                }
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(sl, 0, 0);
            grid.Children.Add(new StackLayout { Padding=padding, Children = { logoImage} }, 0, 0);
            grid.Children.Add(logoImageSl, 0, 0);

            grid.Children.Add(registrationGrid, 0, 1);

            grid.Children.Add(slDis, 0, 2);

            //PopupLayout
            StackLayout blackSl = new StackLayout { BackgroundColor = Color.Black, Opacity = 0.5 };
            blackSl.IsVisible = false;
            grid.Children.Add(blackSl, 0, 0);
            Grid.SetRowSpan(blackSl, 3);

            PopupLayout popupLayout = new PopupLayout
            {
                Content = logoImageSl
            };
            logoImage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired = 2,
                Command = new Command(() => ShowSettingPopUp(popupLayout, blackSl))
            });
            popupLayout.IsVisible = false;
            grid.Children.Add(popupLayout, 0, 0);
            Grid.SetRowSpan(popupLayout, 3);
            
            var tr = new TapGestureRecognizer();
            tr.Tapped += delegate
            {
                popupLayout.DismissPopup();
                blackSl.IsVisible = false;
                popupLayout.IsVisible = false;
            };
            popupLayout.GestureRecognizers.Add(tr);

            Content = grid;

            MessagingCenter.Subscribe<MainViewModel, string>(this, "DisplayAlert", (sender, value) =>
            {
                DisplayAlert(AppResources.Error, value, AppResources.OK);
            });
        }

        private void ShowCheckInView()
        {
            MainViewModel.ShowCheckInCommand.Execute();
        }

        private void ShowSettingPopUp(PopupLayout popupLayout, StackLayout sl)
        {
            sl.IsVisible = true;
            popupLayout.IsVisible = true;
            if (popupLayout.IsPopupActive)
            {
                popupLayout.DismissPopup();
            }
            else
            {
                //var list = new ListView()
                //{
                //    BackgroundColor = Color.White,
                //    ItemsSource = new[] { "1", "2", "3" },
                //    HeightRequest = this.Height * .5,
                //    WidthRequest = this.Width * .8
                //};

                //list.ItemSelected += (s, args) =>
                //    popupLayout.DismissPopup();

                Button btnLogout = new Button
                {
                    Text = AppResources.Logout,
                    BorderColor = Color.Black,
                    BorderRadius = 1,
                    BorderWidth = 2,
                    WidthRequest = 200,
                    HeightRequest = 50,
                    TextColor = Color.Black,
                    BackgroundColor = Color.Transparent
                };

                Button btnSettings = new Button
                {
                    Text = AppResources.Settings,
                    BorderColor = Color.Black,
                    BorderRadius = 1,
                    BorderWidth = 2,
                    WidthRequest = 200,
                    HeightRequest = 50,
                    TextColor = Color.Black,
                    BackgroundColor = Color.Transparent
                };

                MyEntry passEntry = new MyEntry
                {
                    WidthRequest = 300,
                    HeightRequest = 50,
                    TextColor = Color.Black,
                    IsPassword=true
                };

                passEntry.SetBinding(Entry.TextProperty, new Binding("Password"));
                btnLogout.SetBinding(Button.CommandProperty, new Binding("ShowLoginCommand"));
                btnSettings.SetBinding(Button.CommandProperty, new Binding("ShowSettingsCommand"));

                var padding20 = new Thickness(0, 0, 0, 20);
                var padding10 = new Thickness(0, 0, 0, 5);
                StackLayout contentSl = new StackLayout
                {
                    Children =
                    {
                        new StackLayout {Padding=padding20, Children={ btnLogout } },
                        new StackLayout {Padding=padding10,Children= {new BoxView { WidthRequest = 1, HeightRequest = 1, BackgroundColor = Color.Black}} },
                        new StackLayout {Children={new Label { Text=AppResources.Settings,TextColor=Color.Black,HorizontalOptions=LayoutOptions.Center} } },
                        new StackLayout {Padding=padding10, Children={ new Label { Text=AppResources.Enter_Settings_Section,TextColor=Color.Black, HorizontalOptions = LayoutOptions.Center } } },
                        new StackLayout {Padding=padding20, Children={ passEntry } },
                        btnSettings
                    }
                };

                var grid = new Grid
                {
                    Padding = new Thickness(30, 30, 30, 20),
                    BackgroundColor = Color.White,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                grid.RowDefinitions.Add(new RowDefinition { Height = 300 });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = Width * 0.5 });
                grid.Children.Add(contentSl, 0, 0);

                Grid settingGrid = new Grid
                {
                    WidthRequest = Width,
                    HeightRequest = Height,
                };
                settingGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                settingGrid.Children.Add(grid);

                popupLayout.ShowPopup(settingGrid);
            }
        }
    }
}
