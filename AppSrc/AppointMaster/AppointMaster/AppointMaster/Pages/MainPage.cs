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
            Padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20);

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

            PopupLayout popupLayout = new PopupLayout();
            logoImage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                NumberOfTapsRequired=2,
                Command = new Command(() => ShowSettingPopUp(popupLayout))
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
            grid.Children.Add(logoImage, 0, 0);

            grid.Children.Add(registrationGrid, 0, 1);

            grid.Children.Add(slDis, 0, 2);

            grid.Children.Add(popupLayout, 0, 0);
            Grid.SetRowSpan(popupLayout, 3);

            Content = grid;
        }

        private void ShowCheckInView()
        {
            MainViewModel.ShowCheckInCommand.Execute();
        }

        private void ShowSettingPopUp(PopupLayout popupLayout)
        {
            if (popupLayout.IsPopupActive)
            {
                popupLayout.DismissPopup();
            }
            else
            {
                var list = new ListView()
                {
                    BackgroundColor = Color.White,
                    ItemsSource = new[] { "1", "2", "3" },
                    HeightRequest = this.Height * .5,
                    WidthRequest = this.Width * .8
                };

                list.ItemSelected += (s, args) =>
                    popupLayout.DismissPopup();

                popupLayout.ShowPopup(list);
            }
        }
    }
}
