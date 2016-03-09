using AppointMaster.Controls;
using AppointMaster.Resources;
using AppointMaster.ViewModels;
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

        StackLayout blackSl;

        Style labStyle = new Style(typeof(Label))
        {
            Setters = {
                new Setter { Property = Label.FontSizeProperty, Value = 20 },
                new Setter { Property = Label.TextColorProperty, Value = Color.Black },
            }
        };

        public MainPage()
        {
            //Application.Current.Resources["backgroundColor"] = Color.White;
            //Application.Current.Resources["backgroundColor"] = Color.Black;

            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            Thickness padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20);

            Label labName = new Label
            {
                Style = labStyle
            };

            Label labAddress = new Label
            {
                Style = labStyle
            };

            Label labCity = new Label
            {
                Style = labStyle
            };
            labCity.SetBinding(Label.TextProperty, new Binding("City"));

            Label labState = new Label
            {
                Style = labStyle
            };
            labState.SetBinding(Label.TextProperty, new Binding("StateProvince", stringFormat: ("{0}, ")));

            Label labPostalCode = new Label
            {
                Style = labStyle
            };
            labPostalCode.SetBinding(Label.TextProperty, new Binding("PostalCode"));

            StackLayout cityAndZipSl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    labCity,
                    labState,
                    labPostalCode
                }
            };

            Image logoImage = new Image
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };
            if (DataHelper.GetInstance().Clinic.Logo != null)
            {
                logoImage.Source = ImageSource.FromStream(() => new System.IO.MemoryStream(DataHelper.GetInstance().Clinic.Logo));
            }

            StackLayout logoImageSl = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };

            var sl = new StackLayout
            {
                Padding = padding,
                Children = {
                    labName,
                    labAddress,
                    cityAndZipSl
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
            grid.Children.Add(new StackLayout { Padding = padding, Children = { logoImage } }, 0, 0);
            grid.Children.Add(logoImageSl, 0, 0);

            grid.Children.Add(registrationGrid, 0, 1);

            grid.Children.Add(slDis, 0, 2);

            //PopupLayout
            blackSl = new StackLayout { BackgroundColor = Color.Black, Opacity = 0.5 };
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
                //btnSettings.SetDynamicResource(Button.BackgroundColorProperty, "backgroundColor");

                Button btnBack = new Button
                {
                    Text = AppResources.Back,
                    BorderColor = Color.Black,
                    BorderRadius = 1,
                    BorderWidth = 2,
                    WidthRequest = 100,
                    HeightRequest = 50,
                    TextColor = Color.Black,
                    BackgroundColor = Color.Transparent,
                    VerticalOptions = LayoutOptions.End
                };
                btnBack.Clicked += delegate
                {
                    popupLayout.DismissPopup();
                    blackSl.IsVisible = false;
                    popupLayout.IsVisible = false;
                };

                btnLogout.SetBinding(Button.CommandProperty, new Binding("LogoutCommand"));
                btnSettings.SetBinding(Button.CommandProperty, new Binding("ShowSettingsCommand"));

                var padding = new Thickness(100, 0, 100, 20);
                StackLayout contentSl = new StackLayout
                {
                    Children =
                    {
                        new StackLayout {Padding=padding, Children={ btnLogout } },
                        new StackLayout {Padding=padding, Children={ btnSettings } },
                        new StackLayout {Padding=padding, Children={ btnBack } }
                    }
                };

                var grid = new Grid
                {
                    BackgroundColor = Color.White,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                grid.Padding = new Thickness(20, 20, 20, 0);
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
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
