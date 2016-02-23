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
            var padding = new Thickness(20, 20, 20, 20);

            Padding = padding;

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
            //grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            //grid2.Children.Add(logoImage2, 0, 0);
            //grid2.Children.Add(labStep2, 0, 0);

            grid2.Children.Add(streetSl, 0, 0);

            grid2.Children.Add(citySl, 0, 1);

            grid2.Children.Add(stateAndZipSl, 0, 2);

            grid2.Children.Add(btnStep2Sl, 0, 3);

            #endregion

            //Step3
            #region Step3

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

            Button btnAdd = new Button
            {
                BorderColor = Color.Black,
                BorderWidth = 2,
                BorderRadius = 10,
                BackgroundColor = Color.Transparent,
            };

            //Button btn = new Button
            //{
            //    BorderColor = Color.Black,
            //    BorderWidth = 2,
            //    BorderRadius = 1,
            //    BackgroundColor = Color.Transparent
            //};

            CheckBox ck = new CheckBox() { };

            Image patientImage = new Image
            {
                Source = "cat.png",
                HeightRequest = 60,
                WidthRequest = 61
            };

            var patientGrid = new Grid();
            patientGrid.VerticalOptions = LayoutOptions.Center;
            patientGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            patientGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(35) });
            patientGrid.Children.Add(new Button
            {
                BorderColor = Color.Black,
                BorderWidth = 2,
                BorderRadius = 1,
                BackgroundColor = Color.Transparent
            }, 0, 0);
            patientGrid.Children.Add(ck, 0, 0);
            //Grid.SetColumnSpan(ck, 2);

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
                VerticalOptions = LayoutOptions.End,
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

            var sl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    patientGrid,
                    patientImage,
                    labPatientName
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
            addAnotherGrid.Children.Add(btnAdd, 0, 0);
            addAnotherGrid.Children.Add(addAnotherSl, 0, 0);

            var grid4 = new Grid();
            grid4.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
            grid4.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            grid4.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid4.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });

            grid4.Children.Add(addAnotherGrid, 0, 0);

            grid4.Children.Add(whoIsSl, 0, 1);

            grid4.Children.Add(sl, 0, 2);

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

            var dogGrid = new Grid();
            dogGrid.VerticalOptions = LayoutOptions.Center;
            dogGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            dogGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(35) });
            dogGrid.Children.Add(new Button
            {
                BorderColor = Color.Black,
                BorderWidth = 2,
                BorderRadius = 1,
                BackgroundColor = Color.Transparent
            }, 0, 0);
            dogGrid.Children.Add(new CheckBox() { }, 0, 0);

            var fishGrid = new Grid();
            fishGrid.VerticalOptions = LayoutOptions.Center;
            fishGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            fishGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(35) });
            fishGrid.Children.Add(new Button
            {
                BorderColor = Color.Black,
                BorderWidth = 2,
                BorderRadius = 1,
                BackgroundColor = Color.Transparent
            }, 0, 0);
            fishGrid.Children.Add(new CheckBox() { }, 0, 0);

            var catGrid = new Grid();
            catGrid.VerticalOptions = LayoutOptions.Center;
            catGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            catGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(35) });
            catGrid.Children.Add(new Button
            {
                BorderColor = Color.Black,
                BorderWidth = 2,
                BorderRadius = 1,
                BackgroundColor = Color.Transparent
            }, 0, 0);
            catGrid.Children.Add(new CheckBox() { }, 0, 0);

            var hamsterGrid = new Grid();
            hamsterGrid.VerticalOptions = LayoutOptions.Center;
            hamsterGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            hamsterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(35) });
            hamsterGrid.Children.Add(new Button
            {
                BorderColor = Color.Black,
                BorderWidth = 2,
                BorderRadius = 1,
                BackgroundColor = Color.Transparent
            }, 0, 0);
            hamsterGrid.Children.Add(new CheckBox() { }, 0, 0);

            var birdGrid = new Grid();
            birdGrid.VerticalOptions = LayoutOptions.Center;
            birdGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            birdGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(35) });
            birdGrid.Children.Add(new Button
            {
                BorderColor = Color.Black,
                BorderWidth = 2,
                BorderRadius = 1,
                BackgroundColor = Color.Transparent
            }, 0, 0);
            birdGrid.Children.Add(new CheckBox() { }, 0, 0);

            var otherGrid = new Grid();
            otherGrid.VerticalOptions = LayoutOptions.Center;
            otherGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });
            otherGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(35) });
            otherGrid.Children.Add(new Button
            {
                BorderColor = Color.Black,
                BorderWidth = 2,
                BorderRadius = 1,
                BackgroundColor = Color.Transparent
            }, 0, 0);
            otherGrid.Children.Add(new CheckBox() { }, 0, 0);

            StackLayout titleAndEntrySl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    labWhoIsAdd,
                    patientNameEntry
                }
            };

            StackLayout dogSl = new StackLayout
            {
                Padding = new Thickness(0, 0, 100, 0),
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    dogGrid,
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
                    fishGrid,
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
                    catGrid,
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
                    hamsterGrid,
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
                    birdGrid,
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
                    otherGrid,
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
                HeightRequest = 50,
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
            grid4Other.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            grid4Other.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150) });

            grid4Other.Children.Add(breedSl, 0, 0);

            grid4Other.Children.Add(genderAndBirthSl, 0, 1);

            grid4Other.Children.Add(btnStep4OtherSl, 0, 2);

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

            grid2.IsVisible = false;
            grid3.IsVisible = false;
            grid4.IsVisible = false;
            grid4Add.IsVisible = false;
            grid4Other.IsVisible = false;

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
                grid4Add.IsVisible = true;
            };

            btnStep4AddNext.Clicked += delegate
            {
                grid4Add.IsVisible = false;
                grid4Other.IsVisible = true;

            };

            //btnStep4OtherNext.Clicked += delegate
            //{
            //    grid1.IsVisible = false;
            //    grid2.IsVisible = true;
            //};

            //Back
            btnStep1Back.Clicked += delegate
            {

            };

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

            //Binding
            titleEntry.SetBinding(Entry.TextProperty, new Binding("Title"));
            firstEntry.SetBinding(Entry.TextProperty, new Binding("FirstName"));
            lastEntry.SetBinding(Entry.TextProperty, new Binding("LastName"));

            streetEntry.SetBinding(Entry.TextProperty, new Binding("StreetAddress"));
            cityEntry.SetBinding(Entry.TextProperty, new Binding("City"));
            stateEntry.SetBinding(Entry.TextProperty, new Binding("State"));
            zipEntry.SetBinding(Entry.TextProperty, new Binding("Zip"));

            phoneEntry.SetBinding(Entry.TextProperty, new Binding("Phone"));
            emailEntry.SetBinding(Entry.TextProperty, new Binding("Email"));

            Content = grid;
        }
    }
}
