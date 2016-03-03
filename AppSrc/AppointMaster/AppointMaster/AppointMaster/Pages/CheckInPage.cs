﻿using AppointMaster.Controls;
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
    public class CheckInPage : ContentPage
    {
        private CheckInViewModel CheckInViewModel
        {
            get { return BindingContext as CheckInViewModel; }
        }
        public CheckInPage()
        {
            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);

            //Padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20);
            var padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20);

            var resImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "clipbaord.png",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 100,
                WidthRequest = 74
            };

            var logoImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };

            var labCheckIn = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Check_In,
                TextColor = Color.Black,
                FontSize = 20
            };

            var btnNewReg = new Button
            {
                WidthRequest = 300,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                Text = AppResources.Walk_In,
                TextColor = Color.Black,
                FontSize = 20,
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2
            };

            var btnMainMenu = new Button
            {
                WidthRequest = 200,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Start,
                Text = string.Format("< {0}", AppResources.Main_Menu),
                TextColor = Color.Black,
                FontSize = 20,
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2
            };

            btnMainMenu.SetBinding(Button.CommandProperty, new Binding("ShowMainCommand"));
            btnNewReg.SetBinding(Button.CommandProperty, new Binding("ShowRegistrationCommand"));

            var boxView = new BoxView { WidthRequest = 1, HeightRequest = 1, BackgroundColor = Color.Black, VerticalOptions = LayoutOptions.Start };

            MyListView listView = new MyListView()
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    Label dateLabel = new Label();
                    dateLabel.SetBinding(Label.TextProperty, new Binding("Time", stringFormat: "{0:HH:mm}"));
                    dateLabel.TextColor = Color.Black;
                    dateLabel.FontSize = 20;
                    dateLabel.VerticalOptions = LayoutOptions.Center;

                    Label firstLable = new Label();
                    firstLable.SetBinding(Label.TextProperty, "Client.FirstName");
                    firstLable.TextColor = Color.Black;
                    firstLable.FontSize = 20;
                    firstLable.VerticalOptions = LayoutOptions.Center;

                    Label lastLable = new Label();
                    lastLable.SetBinding(Label.TextProperty, "Client.LastName");
                    lastLable.TextColor = Color.Black;
                    lastLable.FontSize = 20;
                    lastLable.VerticalOptions = LayoutOptions.Center;

                    Label patientNameLable = new Label();
                    patientNameLable.SetBinding(Label.TextProperty, "PatientName");
                    patientNameLable.TextColor = Color.Black;
                    patientNameLable.FontSize = 20;
                    patientNameLable.VerticalOptions = LayoutOptions.Center;

                    Label lineLable = new Label();
                    lineLable.Text = "-";
                    lineLable.TextColor = Color.Black;
                    lineLable.FontSize = 20;
                    lineLable.VerticalOptions = LayoutOptions.Center;

                    Button btn = new Button
                    {
                        BorderWidth = 2,
                        BorderColor = Color.Black,
                        WidthRequest = 100,
                        HeightRequest = 40,
                        BackgroundColor = Color.Transparent,
                        BorderRadius = 1,
                        Text = AppResources.Check_In,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Color.Black
                    };

                    return new MyViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5, 0, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                               btn,
                               dateLabel,
                               lineLable,
                               firstLable,
                               lastLable,
                               patientNameLable
                            }
                        }
                    };
                })
            };

            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("Items"));

            var loadingGrid = new Grid();
            loadingGrid.BackgroundColor = Color.Black;
            loadingGrid.Opacity = 0.5;
            loadingGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            loadingGrid.Children.Add(new ActivityIndicator()
            {
                Color = Color.Blue,
                IsRunning = true,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            });

            loadingGrid.SetBinding(Grid.IsVisibleProperty, new Binding("IsBusy"));

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(new StackLayout { Padding = padding, Children = { resImage } }, 0, 0);
            grid.Children.Add(new StackLayout { Padding = padding, Children = { logoImage } }, 0, 0);

            grid.Children.Add(new StackLayout { Padding = new Thickness(0, 0, 20, 0), Children = { btnNewReg } }, 0, 1);

            grid.Children.Add(new StackLayout { Padding = new Thickness(20, 0, 0, 0), Children = { labCheckIn } }, 0, 2);

            grid.Children.Add(new StackLayout { Padding = new Thickness(20, 0, 20, 0), Children = { boxView } }, 0, 3);

            grid.Children.Add(new StackLayout { Padding = new Thickness(20, 0, 20, 0), Children = { listView } }, 0, 4);

            grid.Children.Add(new StackLayout { VerticalOptions = LayoutOptions.End, Padding = new Thickness(20, 0, 0, 20), Children = { btnMainMenu } }, 0, 4);

            grid.Children.Add(loadingGrid, 0, 0);
            Grid.SetRowSpan(loadingGrid, 5);

            Content = grid;

            //Device.StartTimer(new TimeSpan(0,0,15), () => { CheckInViewModel.GetAppointments(); return true; });
        }

    }
}
