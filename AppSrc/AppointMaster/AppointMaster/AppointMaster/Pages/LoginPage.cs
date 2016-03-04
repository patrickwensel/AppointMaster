using AppointMaster.Controls;
using AppointMaster.Resources;
using AppointMaster.ViewModels;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Services;

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

            var logoImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 112,
                WidthRequest = 350
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
                IsPassword = true
            };

            StackLayout userSl = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = AppResources.Login,
                        TextColor = Color.Black,
                        FontSize = 20
                    },
                    userEntry
                }
            };

            StackLayout passwordSl = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
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
                WidthRequest = 470,
                HeightRequest = 50,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2,
                BackgroundColor = Color.Transparent,
                TextColor = Color.Black,
            };

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
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(180) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(130) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(new StackLayout { Padding = new Thickness(0, Device.OnPlatform(40, 20, 20), 0, 0), Children = { logoImage } }, 0, 0);

            grid.Children.Add(userSl, 0, 1);

            grid.Children.Add(passwordSl, 0, 2);

            grid.Children.Add(loadingGrid, 0, 0);
            Grid.SetRowSpan(loadingGrid, 4);

            grid.Children.Add(new StackLayout { HeightRequest = 50, HorizontalOptions = LayoutOptions.Center, Children = { btnLogin } }, 0, 3);

            userEntry.SetBinding(Entry.TextProperty, new Binding("UserName"));
            passwordEntry.SetBinding(Entry.TextProperty, new Binding("Password"));
            btnLogin.SetBinding(Button.CommandProperty, new Binding("LoginCommand"));

            MessagingCenter.Subscribe<LoginViewModel, string>(this, "DisplayAlert", (sender, value) =>
            {
                DisplayAlert(AppResources.Error, value, AppResources.OK);
            });

            Content = grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var secureStorage = Resolver.Resolve<ISecureStorage>();
                secureStorage.Retrieve("BaseAPI");
            }
            catch (System.Exception)
            {
                LoginViewModel.ShowSetting();
            }
        }
    }
}
