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
using System.Runtime.CompilerServices;
using System.Globalization;

namespace AppointMaster.Pages
{
    public class RegistrationPage : ContentPage //IndexedPages
    {
        private RegistrationViewModel RegistrationViewModel
        {
            get { return BindingContext as RegistrationViewModel; }
        }

        public RegistrationPage()
        {
            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            Padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20);

            var logoImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207
            };

            var labStep = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Registration_Step1,
                TextColor = Color.Black,
                FontSize = 20
            };

            //Step1
            #region Step1
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

            MyPicker titlePicker = new MyPicker
            {
                WidthRequest = 120,
                HeightRequest = 50,
                DisplayProperty = "Title"
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
                HorizontalOptions = LayoutOptions.Start,
                Padding = new Thickness(Device.OnPlatform(130, 145, 0), 0, 0, 0),
                Children =
                {
                    new Label
                    {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        Text = AppResources.Title,
                        TextColor = Color.Black,
                        FontSize = 20
                    },
                    titlePicker
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
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            grid1.Children.Add(titleSl, 0, 0);

            grid1.Children.Add(firstSl, 0, 1);

            grid1.Children.Add(lastSl, 0, 2);

            grid1.Children.Add(btnStep1Sl, 0, 3);

            #endregion

            //Step2
            #region Step2

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
                Text = string.Format("{0}/{1}", AppResources.State, AppResources.Province),
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

            MyPicker statePicker = new MyPicker
            {
                HeightRequest = 50,
                WidthRequest = 120,
                DisplayProperty = "State"
            };

            var zipEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 130,
                TextColor = Color.Black,
                FontSize = 20
            };

            var postalEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 130,
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
                Padding = new Thickness(0, 0, 30, 0),
                Children =
                {
                   labState,
                   statePicker
                }
            };

            var postalSl = new StackLayout
            {
                Padding = new Thickness(0, 0, 30, 0),
                Children =
                {
                   new Label
                   {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        Text = AppResources.Postal_Code,
                        TextColor = Color.Black,
                        FontSize = 20
                   },
                   postalEntry
                }
            };

            var zipSl = new StackLayout
            {
                Children =
                {
                   new Label
                   {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        Text = AppResources.Zip,
                        TextColor = Color.Black,
                        FontSize = 20
                   },
                   zipEntry
                }
            };

            var stateAndPostalSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                  stateSl,
                  postalSl,
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
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            grid2.Children.Add(streetSl, 0, 0);

            grid2.Children.Add(citySl, 0, 1);

            grid2.Children.Add(stateAndPostalSl, 0, 2);

            grid2.Children.Add(btnStep2Sl, 0, 3);

            #endregion

            //Step3
            #region Step3

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
                    new Label
                    {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        Text = AppResources.Phone,
                        TextColor = Color.Black,
                        FontSize = 20
                    },
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
            grid3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            grid3.Children.Add(phoneSl, 0, 0);

            grid3.Children.Add(emailSl, 0, 1);

            grid3.Children.Add(btnStep3Sl, 0, 2);

            #endregion

            //Step4
            #region Step4

            Label labWhoIs = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Who_Is_With,
                TextColor = Color.Black,
                FontSize = 20
            };

            Label labPatientName = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                Text = "Fido",
                TextColor = Color.Black,
                FontSize = 20
            };

            Label labAddAnother = new Label
            {
                Text = AppResources.Add_Another,
                TextColor = Color.Black,
                FontSize = 20
            };

            Label labCompanion = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = AppResources.Companion,
                TextColor = Color.Black,
                FontSize = 20
            };

            BoxView line = new BoxView
            {
                WidthRequest = 1,
                HeightRequest = 1,
                BackgroundColor = Color.Black,
                VerticalOptions = LayoutOptions.Start
            };

            Button btnStep4Back = new Button
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
            Button btnStep4Cancel = new Button
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
            Button btnStep4Next = new Button
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

            var btnStep4Sl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep4Back } },
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep4Cancel } },
                    new StackLayout {Children= { btnStep4Next } }
                }
            };

            var whoIsSl = new StackLayout
            {
                Children =
                {
                   labWhoIs,
                   line
                }
            };

            var addAnotherSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                   labAddAnother,
                   labCompanion
                }
            };

            var addAnotherGrid = new Grid();
            addAnotherGrid.HorizontalOptions = LayoutOptions.End;
            addAnotherGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
            addAnotherGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            addAnotherGrid.Children.Add(new Button
            {
                BorderColor = Color.Black,
                BorderWidth = 2,
                BorderRadius = 10,
                BackgroundColor = Color.Transparent,
            }, 0, 0);
            addAnotherGrid.Children.Add(addAnotherSl, 0, 0);

            MyListView lstPatient = new MyListView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    Image imgChecked = new Image();
                    imgChecked.VerticalOptions = LayoutOptions.Center;
                    imgChecked.Source = "checked_checkbox.png";
                    imgChecked.SetBinding(Image.IsVisibleProperty, new Binding("IsChecked"));

                    Image imgUnChecked = new Image();
                    imgUnChecked.VerticalOptions = LayoutOptions.Center;
                    imgUnChecked.Source = "unchecked_checkbox.png";
                    imgUnChecked.SetBinding(Image.IsVisibleProperty, "IsChecked", BindingMode.Default, new TrueToFalseConverter());

                    Image patientImg = new Image();
                    patientImg.HeightRequest = 60;
                    patientImg.WidthRequest = 61;
                    patientImg.VerticalOptions = LayoutOptions.Center;
                    patientImg.SetBinding(Image.SourceProperty, new Binding("Image"));

                    Label nameLable = new Label();
                    nameLable.TextColor = Color.Black;
                    nameLable.FontSize = 20;
                    nameLable.VerticalOptions = LayoutOptions.Center;
                    nameLable.SetBinding(Label.TextProperty, "PatientName");

                    return new MyViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5, 0, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                               imgChecked,
                               imgUnChecked,
                               patientImg,
                               nameLable
                            }
                        }
                    };
                })
            };

            lstPatient.HeightRequest = 300;
            lstPatient.ItemTapped += LstPatient_ItemTapped;
            lstPatient.SetBinding(ListView.ItemsSourceProperty, new Binding("PatientList"));

            var grid4 = new Grid();
            grid4.RowDefinitions.Add(new RowDefinition { Height = 80 });
            grid4.RowDefinitions.Add(new RowDefinition { Height = 40 });
            grid4.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid4.RowDefinitions.Add(new RowDefinition { Height = 100 });

            grid4.Children.Add(addAnotherGrid, 0, 0);

            grid4.Children.Add(whoIsSl, 0, 1);

            grid4.Children.Add(lstPatient, 0, 2);

            grid4.Children.Add(btnStep4Sl, 0, 3);
            #endregion

            //Add Another
            #region Add Another

            Label labWhoIsAdd = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Who_Is_With,
                TextColor = Color.Black,
                FontSize = 20
            };

            var patientNameEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20,
            };

            StackLayout titleAndEntrySl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    labWhoIsAdd,
                    patientNameEntry
                }
            };

            //Dog
            Image imgDogChecked = new Image();
            imgDogChecked.VerticalOptions = LayoutOptions.Center;
            imgDogChecked.Source = "checked_checkbox.png";
            imgDogChecked.SetBinding(Image.IsVisibleProperty, new Binding("IsDog"));

            Image imgDogUnChecked = new Image();
            imgDogUnChecked.VerticalOptions = LayoutOptions.Center;
            imgDogUnChecked.Source = "unchecked_checkbox.png";
            imgDogUnChecked.SetBinding(Image.IsVisibleProperty, "IsDog", BindingMode.Default, new TrueToFalseConverter());

            //Fish
            Image imgFishChecked = new Image();
            imgFishChecked.VerticalOptions = LayoutOptions.Center;
            imgFishChecked.Source = "checked_checkbox.png";
            imgFishChecked.SetBinding(Image.IsVisibleProperty, new Binding("IsFish"));

            Image imgFishUnChecked = new Image();
            imgFishUnChecked.VerticalOptions = LayoutOptions.Center;
            imgFishUnChecked.Source = "unchecked_checkbox.png";
            imgFishUnChecked.SetBinding(Image.IsVisibleProperty, "IsFish", BindingMode.Default, new TrueToFalseConverter());

            //Cat
            Image imgCatChecked = new Image();
            imgCatChecked.VerticalOptions = LayoutOptions.Center;
            imgCatChecked.Source = "checked_checkbox.png";
            imgCatChecked.SetBinding(Image.IsVisibleProperty, new Binding("IsCat"));

            Image imgCatUnChecked = new Image();
            imgCatUnChecked.VerticalOptions = LayoutOptions.Center;
            imgCatUnChecked.Source = "unchecked_checkbox.png";
            imgCatUnChecked.SetBinding(Image.IsVisibleProperty, "IsCat", BindingMode.Default, new TrueToFalseConverter());

            //Hamster
            Image imgHamsterChecked = new Image();
            imgHamsterChecked.VerticalOptions = LayoutOptions.Center;
            imgHamsterChecked.Source = "checked_checkbox.png";
            imgHamsterChecked.SetBinding(Image.IsVisibleProperty, new Binding("IsHamster"));

            Image imgHamsterUnChecked = new Image();
            imgHamsterUnChecked.VerticalOptions = LayoutOptions.Center;
            imgHamsterUnChecked.Source = "unchecked_checkbox.png";
            imgHamsterUnChecked.SetBinding(Image.IsVisibleProperty, "IsHamster", BindingMode.Default, new TrueToFalseConverter());

            //Bird
            Image imgBirdChecked = new Image();
            imgBirdChecked.VerticalOptions = LayoutOptions.Center;
            imgBirdChecked.Source = "checked_checkbox.png";
            imgBirdChecked.SetBinding(Image.IsVisibleProperty, new Binding("IsBird"));

            Image imgBirdUnChecked = new Image();
            imgBirdUnChecked.VerticalOptions = LayoutOptions.Center;
            imgBirdUnChecked.Source = "unchecked_checkbox.png";
            imgBirdUnChecked.SetBinding(Image.IsVisibleProperty, "IsBird", BindingMode.Default, new TrueToFalseConverter());

            //Other
            Image imgOtherChecked = new Image();
            imgOtherChecked.VerticalOptions = LayoutOptions.Center;
            imgOtherChecked.Source = "checked_checkbox.png";
            imgOtherChecked.SetBinding(Image.IsVisibleProperty, new Binding("IsOther"));

            Image imgOtherUnChecked = new Image();
            imgOtherUnChecked.VerticalOptions = LayoutOptions.Center;
            imgOtherUnChecked.Source = "unchecked_checkbox.png";
            imgOtherUnChecked.SetBinding(Image.IsVisibleProperty, "IsOther", BindingMode.Default, new TrueToFalseConverter());

            StackLayout dogSl = new StackLayout
            {
                Padding = new Thickness(0, 0, 100, 0),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    imgDogChecked,
                    imgDogUnChecked,
                    new Image
                    {
                        Source = "dog.png",
                        HeightRequest = 60,
                        WidthRequest = 61
                    }
                }
            };

            StackLayout fishSl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                   imgFishChecked,
                   imgFishUnChecked,
                    new Image
                    {
                        Source = "fish.png",
                        HeightRequest = 60,
                        WidthRequest = 61
                    }
                }
            };

            StackLayout dogAndFishSl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    dogSl,
                    fishSl
                }
            };

            StackLayout catSl = new StackLayout
            {
                Padding = new Thickness(0, 0, 100, 0),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    imgCatChecked,
                    imgCatUnChecked,
                    new Image
                    {
                        Source = "cat.png",
                        HeightRequest = 60,
                        WidthRequest = 61
                    }
                }
            };

            StackLayout hamsterSl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    imgHamsterChecked,
                    imgHamsterUnChecked,
                    new Image
                    {
                        Source = "hamster.png",
                        HeightRequest = 60,
                        WidthRequest = 61
                    }
                }
            };

            StackLayout catAndHamsterSl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    catSl,
                    hamsterSl
                }
            };

            StackLayout birdSl = new StackLayout
            {
                Padding = new Thickness(0, 0, 100, 0),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    imgBirdChecked,
                    imgBirdUnChecked,
                    new Image
                    {
                        Source = "bird.png",
                        HeightRequest = 60,
                        WidthRequest = 61
                    }
                }
            };

            StackLayout otherSl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    imgOtherChecked,
                    imgOtherUnChecked,
                    new Label
                    {
                        VerticalOptions=LayoutOptions.Center,
                        Text = AppResources.Other,
                        TextColor=Color.Black,
                        FontSize=24
                    }
                }
            };

            StackLayout birdAndOtherSl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    birdSl,
                    otherSl
                }
            };

            Button btnStep4AddBack = new Button
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
            Button btnStep4AddCancel = new Button
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
            Button btnStep4AddNext = new Button
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

            var btnStep4AddSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep4AddBack } },
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep4AddCancel } },
                    new StackLayout { Children= { btnStep4AddNext } }
                }
            };

            dogSl.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => SelectPatientSpecies("Dog"))
            });

            fishSl.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => SelectPatientSpecies("Fish"))
            });

            catSl.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => SelectPatientSpecies("Cat"))
            });

            hamsterSl.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => SelectPatientSpecies("Hamster"))
            });

            birdSl.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => SelectPatientSpecies("Bird"))
            });

            otherSl.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => SelectPatientSpecies("Other"))
            });

            var grid4Add = new Grid();
            grid4Add.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid4Add.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            grid4Add.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            grid4Add.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid4Add.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });

            grid4Add.Children.Add(titleAndEntrySl, 0, 0);

            grid4Add.Children.Add(dogAndFishSl, 0, 1);

            grid4Add.Children.Add(catAndHamsterSl, 0, 2);

            grid4Add.Children.Add(birdAndOtherSl, 0, 3);

            grid4Add.Children.Add(btnStep4AddSl, 0, 4);

            #endregion

            //Other
            #region Other

            Label labBreed = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                Text = AppResources.Breed,
                TextColor = Color.Black,
                FontSize = 20
            };

            var breedEntry = new MyEntry
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20
            };

            MyPicker genderPicker = new MyPicker
            {
                BackgroundColor = Color.White,
                WidthRequest = 150,
                HeightRequest = 50
            };
            genderPicker.Items.Add("Male");
            genderPicker.Items.Add("Female");

            MyDatePicker datePicker = new MyDatePicker
            {
                WidthRequest = 150,
                HeightRequest = 50
            };

            Button btnStep4OtherBack = new Button
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
            Button btnStep4OtherCancel = new Button
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
            Button btnStep4OtherNext = new Button
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

            var btnStep4OtherSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.End,
                Children =
                {
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep4OtherBack } },
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep4OtherCancel } },
                    new StackLayout { Children= { btnStep4OtherNext } }
                }
            };

            var breedSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                      labBreed,
                      breedEntry
                }
            };

            var genterSl = new StackLayout
            {
                Padding = new Thickness(0, 0, 110, 0),
                Children =
                {
                      new Label
                      {
                        VerticalOptions = LayoutOptions.Center,
                        Text = AppResources.Gender,
                        TextColor = Color.Black,
                        FontSize = 20
                      },
                      genderPicker
                }
            };

            var datePickerSl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                     datePicker,
                     new Image
                     {
                        Source= "calendar.png",
                        HeightRequest=50,
                        WidthRequest=50
                     }
                }
            };

            var birthSl = new StackLayout
            {
                Children =
                {
                      new Label
                      {
                        VerticalOptions = LayoutOptions.Center,
                        Text = AppResources.Birthdate,
                        TextColor = Color.Black,
                        FontSize = 20
                      },
                      datePickerSl
                }
            };

            var genderAndBirthSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    genterSl,
                    birthSl
                }
            };

            var grid4Other = new Grid();
            grid4Other.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid4Other.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
            grid4Other.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });

            grid4Other.Children.Add(breedSl, 0, 0);

            grid4Other.Children.Add(genderAndBirthSl, 0, 1);

            grid4Other.Children.Add(btnStep4OtherSl, 0, 2);

            #endregion

            //Confirm
            #region Confirm

            Button btnEditClient = new Button
            {
                Text = AppResources.Edit,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2,
                BackgroundColor = Color.Transparent,
                HeightRequest = 40
            };

            Label labTitle = new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };

            Label labFirstName = new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };

            Label labLastName = new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };

            Label labZiP = new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };

            Label labPhone = new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };

            Label labEmail= new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };

            Grid clientInfoGrid = new Grid();
            clientInfoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            clientInfoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 300 });
            clientInfoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 100 });
            clientInfoGrid.Children.Add(new StackLayout
            {
                Children =
                {
                    new Label { Text=AppResources.Client_Information,TextColor=Color.Black,FontSize=25},
                    new BoxView { WidthRequest = 1, HeightRequest = 2, BackgroundColor = Color.Black, VerticalOptions = LayoutOptions.Start }
                }
            }, 0, 0);
            clientInfoGrid.Children.Add(btnEditClient, 1, 0);

            Button btnEditClientInfo = new Button
            {
                Text = AppResources.Edit,
                TextColor = Color.Black,
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2
            };

            StackLayout clientInfoSl = new StackLayout
            {
                Children = {
                    new StackLayout
                    {
                        Orientation=StackOrientation.Horizontal,
                        Children =
                        {
                            labTitle,
                            labFirstName,
                            labLastName,
                        }
                    },

                    new StackLayout
                    {
                        Orientation=StackOrientation.Horizontal,
                        Children =
                        {
                            labTitle,
                            labFirstName,
                            labLastName,
                        }
                    },
                }
            };

            var gridConfirm = new Grid();
            gridConfirm.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            gridConfirm.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            gridConfirm.Children.Add(new StackLayout
            {
                WidthRequest = 100,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    clientInfoSl,
                    btnEditClientInfo
                }
            }, 0, 0);

            gridConfirm.Children.Add(clientInfoSl, 0, 1);

            //Binding
            labTitle.SetBinding(Label.TextProperty, new Binding(""));
            labTitle.SetBinding(Label.TextProperty, new Binding(""));
            labTitle.SetBinding(Label.TextProperty, new Binding(""));

            labTitle.SetBinding(Label.TextProperty, new Binding(""));
            labTitle.SetBinding(Label.TextProperty, new Binding(""));
            labTitle.SetBinding(Label.TextProperty, new Binding(""));
            #endregion

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(logoImage, 0, 0);
            grid.Children.Add(labStep, 0, 0);

            grid.Children.Add(grid1, 0, 1);
            grid.Children.Add(grid2, 0, 1);
            grid.Children.Add(grid3, 0, 1);
            grid.Children.Add(grid4, 0, 1);
            grid.Children.Add(grid4Add, 0, 1);
            grid.Children.Add(grid4Other, 0, 1);
            grid.Children.Add(gridConfirm, 0, 1);

            grid1.IsVisible = false;
            grid2.IsVisible = false;
            grid3.IsVisible = false;
            grid4.IsVisible = false;
            grid4Add.IsVisible = false;
            grid4Other.IsVisible = false;
            gridConfirm.IsVisible = true;

            btnStep1Next.Clicked += delegate
            {
                grid1.IsVisible = false;
                grid2.IsVisible = true;
                labStep.Text = AppResources.Registration_Step2;
            };

            btnStep2Next.Clicked += delegate
            {
                grid2.IsVisible = false;
                grid3.IsVisible = true;
                labStep.Text = AppResources.Registration_Step3;
            };

            btnStep3Next.Clicked += delegate
            {
                grid3.IsVisible = false;
                grid4.IsVisible = true;
                labStep.Text = AppResources.Registration_Step4;
            };

            btnStep4Next.Clicked += delegate
            {
                grid4.IsVisible = false;
                gridConfirm.IsVisible = true;
            };

            btnStep4AddNext.Clicked += delegate
            {
                if (RegistrationViewModel.PatientSpecies == "Other")
                {
                    grid4Add.IsVisible = false;
                    grid4Other.IsVisible = true;
                }
                else
                {
                    grid4Add.IsVisible = false;
                    gridConfirm.IsVisible = true;
                }
            };

            btnStep4OtherNext.Clicked += delegate
            {
                grid4Other.IsVisible = false;
                gridConfirm.IsVisible = true;
            };

            //Back
            btnStep1Back.SetBinding(Button.CommandProperty, new Binding("ShowCheckInCommand"));

            btnStep2Back.Clicked += delegate
            {
                grid1.IsVisible = true;
                grid2.IsVisible = false;
                labStep.Text = AppResources.Registration_Step1;
            };

            btnStep3Back.Clicked += delegate
            {
                grid2.IsVisible = true;
                grid3.IsVisible = false;
                labStep.Text = AppResources.Registration_Step2;
            };

            btnStep4Back.Clicked += delegate
            {
                grid3.IsVisible = true;
                grid4.IsVisible = false;
                labStep.Text = AppResources.Registration_Step3;
            };

            btnStep4AddBack.Clicked += delegate
            {
                grid4.IsVisible = true;
                grid4Add.IsVisible = false;
            };

            btnStep4OtherBack.Clicked += delegate
            {
                grid4Add.IsVisible = true;
                grid4Other.IsVisible = false;
            };

            var addAnotherSlClick = new TapGestureRecognizer();
            addAnotherSlClick.Tapped += delegate
            {
                grid4.IsVisible = false;
                grid4Add.IsVisible = true;
            };
            addAnotherSl.GestureRecognizers.Add(addAnotherSlClick);

            //Binding
            titlePicker.SetBinding(ExtendedPicker.ItemsSourceProperty, new Binding("TitleList", BindingMode.TwoWay));
            titlePicker.SetBinding(ExtendedPicker.SelectedItemProperty, new Binding("SelectedTitle", BindingMode.TwoWay));

            statePicker.SetBinding(ExtendedPicker.ItemsSourceProperty, new Binding("StateList", BindingMode.TwoWay));
            statePicker.SetBinding(ExtendedPicker.SelectedItemProperty, new Binding("SelectedState", BindingMode.TwoWay));

            firstEntry.SetBinding(Entry.TextProperty, new Binding("FirstName"));
            lastEntry.SetBinding(Entry.TextProperty, new Binding("LastName"));

            streetEntry.SetBinding(Entry.TextProperty, new Binding("StreetAddress"));
            cityEntry.SetBinding(Entry.TextProperty, new Binding("City"));
            zipEntry.SetBinding(Entry.TextProperty, new Binding("Zip"));

            phoneEntry.SetBinding(Entry.TextProperty, new Binding("Phone"));
            emailEntry.SetBinding(Entry.TextProperty, new Binding("Email"));

            Content = grid;
        }

        private void LstPatient_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            PatientInfo model = e.Item as PatientInfo;
            model.IsChecked = !model.IsChecked;
        }

        private void SelectPatientSpecies(string species)
        {
            if (!string.IsNullOrEmpty(RegistrationViewModel.PatientSpecies))
            {
                if (RegistrationViewModel.PatientSpecies == species)
                    return;

                switch (RegistrationViewModel.PatientSpecies)
                {
                    case "Dog":
                        RegistrationViewModel.IsDog = false;
                        break;
                    case "Fish":
                        RegistrationViewModel.IsFish = false;
                        break;
                    case "Cat":
                        RegistrationViewModel.IsCat = false;
                        break;
                    case "Hamster":
                        RegistrationViewModel.IsHamster = false;
                        break;
                    case "Bird":
                        RegistrationViewModel.IsBird = false;
                        break;
                    case "Other":
                        RegistrationViewModel.IsOther = false;
                        break;
                }
            }

            switch (species)
            {
                case "Dog":
                    RegistrationViewModel.IsDog = true;
                    break;
                case "Fish":
                    RegistrationViewModel.IsFish = true;
                    break;
                case "Cat":
                    RegistrationViewModel.IsCat = true;
                    break;
                case "Hamster":
                    RegistrationViewModel.IsHamster = true;
                    break;
                case "Bird":
                    RegistrationViewModel.IsBird = true;
                    break;
                case "Other":
                    RegistrationViewModel.IsOther = true;
                    break;
            }

            RegistrationViewModel.PatientSpecies = species;
        }

        class TrueToFalseConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return !(bool)value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }
    }
}
