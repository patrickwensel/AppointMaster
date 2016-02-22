using AppointMaster.Resources;
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
        public CheckInPage()
        {
            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            var padding = new Thickness(20, 20, 20, 20);

            Padding = padding;

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
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                Text = AppResources.Walk_In,
                TextColor = Color.Black,
                FontSize = 20,
                BackgroundColor = Color.Transparent
            };

            var btnMainMenu = new Button
            {
                WidthRequest=200,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Main_Menu,
                TextColor = Color.Black,
                FontSize = 20,
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2
            };

            btnNewReg.SetBinding(Button.CommandProperty, new Binding("ShowRegistrationCommand"));

            var boxView = new BoxView { WidthRequest = 1, HeightRequest = 1, BackgroundColor = Color.Black, VerticalOptions = LayoutOptions.Start };

            ListView listView = new ListView()
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    Label dateLabel = new Label();
                    dateLabel.SetBinding(Label.TextProperty, "Date");
                    dateLabel.TextColor = Color.Black;
                    dateLabel.FontSize = 20;

                    Label infoLable = new Label();
                    infoLable.SetBinding(Label.TextProperty, "Info");
                    infoLable.TextColor = Color.Black;
                    infoLable.FontSize = 20;

                    Label lineLable = new Label();
                    lineLable.Text = "-";
                    lineLable.TextColor = Color.Black;
                    lineLable.FontSize = 20;

                    Button btn = new Button
                    {
                        BorderWidth = 2,
                        BorderColor = Color.Black,
                        WidthRequest = 25,
                        HeightRequest = 5,
                        BackgroundColor = Color.Transparent,
                        BorderRadius = 1
                    };

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                new StackLayout
                                {
                                    VerticalOptions=LayoutOptions.Center,
                                    Orientation=StackOrientation.Horizontal,
                                    Children=
                                    {
                                        btn,
                                        dateLabel,
                                        lineLable,
                                        infoLable
                                    }
                                }
                            }
                        }
                    };
                })
            };

            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("Items"));

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(resImage, 0, 0);
            grid.Children.Add(logoImage, 0, 0);

            grid.Children.Add(labCheckIn, 0, 1);
            grid.Children.Add(btnNewReg, 0, 1);

            grid.Children.Add(boxView, 0, 2);

            grid.Children.Add(listView, 0, 3);
            grid.Children.Add(btnMainMenu, 0, 3);

            Content = grid;

        }
    }
}
