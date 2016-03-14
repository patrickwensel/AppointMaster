using AppointMaster.Controls;
using AppointMaster.Converters;
using AppointMaster.Resources;
using AppointMaster.Services;
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
    public class SettingsPage : ContentPage
    {
        private SettingsViewModel SettingsViewModel
        {
            get { return BindingContext as SettingsViewModel; }
        }

        public SettingsPage()
        {
            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            Padding = new Thickness(100, Device.OnPlatform(40, 20, 20), 100, 20);

            MyEntry apiEntry = new MyEntry
            {
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20
            };
            apiEntry.SetBinding(MyEntry.TextProperty, new Binding("BaseAPIAddress"));

            Button btnSave = new Button
            {
                Text = AppResources.Save,
                WidthRequest = 470,
                HeightRequest = 50,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2,
                BackgroundColor = DataHelper.GetInstance().BaseAPI == null ? Color.Transparent : DataHelper.GetInstance().SecondaryColor,
                TextColor = Color.Black,
            };
            btnSave.SetBinding(Button.CommandProperty, new Binding("SaveCommand"));

            Image imgChecked = new Image();
            imgChecked.VerticalOptions = LayoutOptions.Center;
            imgChecked.Source = "checked_checkbox.png";
            imgChecked.SetBinding(Image.IsVisibleProperty, new Binding("IsChecked"));

            Image imgUnChecked = new Image();
            imgUnChecked.VerticalOptions = LayoutOptions.Center;
            imgUnChecked.Source = "unchecked_checkbox.png";
            imgUnChecked.SetBinding(Image.IsVisibleProperty, "IsChecked", BindingMode.Default, new TrueToFalseConverter());

            StackLayout demoSL = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                        {
                            imgChecked,
                            imgUnChecked,
                            new Label { Text=AppResources.Enable_Local,FontSize=20,TextColor=Color.Black}
                        }
            };

            var demoSLClick = new TapGestureRecognizer();
            demoSLClick.Tapped += delegate
            {
                SettingsViewModel.IsChecked = !SettingsViewModel.IsChecked;
            };
            demoSL.GestureRecognizers.Add(demoSLClick);

            StackLayout apiSL = new StackLayout
            {
                Children =
                {
                   new Label { Text=AppResources.Base_API_Address,TextColor=Color.Black,FontSize=20},
                   apiEntry
                }
            };

            StackLayout checkInSL = new StackLayout
            {
                Children =
                {
                   new Label { Text=AppResources.CheckIn_Model,TextColor=Color.Black,FontSize=20},
                }
            };

            MyListView list = new MyListView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    Image imgItemChecked = new Image();
                    imgItemChecked.VerticalOptions = LayoutOptions.Center;
                    imgItemChecked.Source = "checked_checkbox.png";
                    imgItemChecked.SetBinding(Image.IsVisibleProperty, new Binding("IsDigitModel"));

                    Image imgItemUnChecked = new Image();
                    imgItemUnChecked.VerticalOptions = LayoutOptions.Center;
                    imgItemUnChecked.Source = "unchecked_checkbox.png";
                    imgItemUnChecked.SetBinding(Image.IsVisibleProperty, "IsDigitModel", BindingMode.Default, new TrueToFalseConverter());

                    Label labName = new Label();
                    labName.TextColor = Color.Black;
                    labName.FontSize = 20;
                    labName.VerticalOptions = LayoutOptions.Center;
                    labName.SetBinding(Label.TextProperty, "Name");

                    return new MyViewCell
                    {
                        View = new StackLayout
                        {

                            Padding = new Thickness(0, 5, 0, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                               new StackLayout
                               {
                                   Orientation=StackOrientation.Horizontal,
                                   Children =
                                   {
                                       imgItemChecked,
                                       imgItemUnChecked,
                                       labName
                                   }
                                },
                            }
                        }
                    };
                })
            };
            list.HeightRequest = 150;
            list.SetBinding(ListView.ItemsSourceProperty, new Binding("Items"));
            list.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedCheckInModel", BindingMode.TwoWay));
            list.ItemTapped += List_ItemTapped;

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = 50 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 50 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 80 });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = 50 });

            grid.Children.Add(new Label { Text = AppResources.Settings, TextColor = Color.Black, FontSize = 30, FontFamily = "Bold" }, 0, 1);

            grid.Children.Add(apiSL, 0, 2);

            grid.Children.Add(new StackLayout
            {
                Padding = new Thickness(0, 30, 0, 0),
                Children =
                {
                   new Label { Text=AppResources.Demo_Model,TextColor=Color.Black,FontSize=20},
                   demoSL
                }
            }, 0, 3);

            grid.Children.Add(new StackLayout
            {
                Padding = new Thickness(0, 30, 0, 0),
                Children =
                {
                   new Label { Text=AppResources.CheckIn_Model,TextColor=Color.Black,FontSize=20},
                   list
                }
            }, 0, 4);

            grid.Children.Add(btnSave, 0, 5);

            Content = grid;

            MessagingCenter.Subscribe<SettingsViewModel, string>(this, "DisplayAlert", (sender, value) =>
            {
                DisplayAlert(AppResources.Error, AppResources.Enter_BaseAPI, AppResources.OK);
            });
        }

        private void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            SettingsViewModel.CheckInModel model = e.Item as SettingsViewModel.CheckInModel;
            if (model.IsDigitModel)
                return;
            SettingsViewModel.Items.Where(x => x.IsDigitModel).FirstOrDefault().IsDigitModel = false;
            model.IsDigitModel = true;
        }
    }
}
