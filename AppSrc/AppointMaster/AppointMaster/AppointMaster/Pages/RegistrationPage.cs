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
using AppointMaster.Models;
using AppointMaster.Services;
using AppointMaster.Converters;

namespace AppointMaster.Pages
{
    public class RegistrationPage : ContentPage
    {
        private RegistrationViewModel RegistrationViewModel
        {
            get { return BindingContext as RegistrationViewModel; }
        }

        Grid grid1;
        Grid grid2;
        Grid grid3;
        Grid grid4;
        Grid grid4Add;
        Grid grid4Breed;
        Grid grid5;

        Label labStep;
        //Label labCheckIn;
        MyListView lstBreedNotPrimary;

        DisplayPatientModel selectPatientToCheckInItem;

        Style buttonNextStyle = new Style(typeof(Button))
        {
            Setters = {
              new Setter { Property = Button.BackgroundColorProperty, Value = DataHelper.GetInstance().PrimaryColor},
              new Setter { Property = Button.TextColorProperty, Value = Color.Black },
              new Setter { Property = Button.BorderRadiusProperty, Value = 1 },
              new Setter { Property = Button.BorderWidthProperty, Value = 2 },
              new Setter { Property = Button.BorderColorProperty, Value = Color.Black },
              new Setter { Property = Button.WidthRequestProperty, Value = 120 },
              new Setter { Property = Button.HeightRequestProperty, Value = 50 },
            }
        };

        Style buttonBackStyle = new Style(typeof(Button))
        {
            Setters = {
              new Setter { Property = Button.BackgroundColorProperty, Value = DataHelper.GetInstance().SecondaryColor },
              new Setter { Property = Button.TextColorProperty, Value = Color.Black },
              new Setter { Property = Button.BorderRadiusProperty, Value = 1 },
              new Setter { Property = Button.BorderWidthProperty, Value = 2 },
              new Setter { Property = Button.BorderColorProperty, Value = Color.Black },
              new Setter { Property = Button.WidthRequestProperty, Value = 120 },
              new Setter { Property = Button.HeightRequestProperty, Value = 50 },
            }
        };

        Style entryStyle = new Style(typeof(Entry))
        {
            Setters = {
              new Setter { Property = Entry.TextColorProperty, Value = Color.Black },
              new Setter { Property = Entry.FontSizeProperty, Value = 20 },
              new Setter { Property = Entry.WidthRequestProperty, Value = 470 },
              new Setter { Property = Entry.HeightRequestProperty, Value = 50 },
            }
        };

        Style labStyle = new Style(typeof(Label))
        {
            Setters = {
              new Setter { Property = Label.TextColorProperty, Value = Color.Black },
              new Setter { Property = Label.FontSizeProperty, Value = 20 },
            }
        };

        public RegistrationPage()
        {
            BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            var padding = new Thickness(20, Device.OnPlatform(40, 20, 20), 20, 20);

            Image imgLogo = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest = 100,
                WidthRequest = 207,
                Source="svc_logo.png"
            };
            if (DataHelper.GetInstance().Clinic.Logo != null)
            {
                imgLogo.Source = ImageSource.FromStream(() => new System.IO.MemoryStream(DataHelper.GetInstance().Clinic.Logo));
            }

            labStep = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                Text = AppResources.Registration_Step1,
                Style = labStyle
            };

            Step1();
            Step2();
            Step3();
            Step4();
            Step4AddBreed();
            Step4AddPatient();
            Step5();

            var loadingGrid = new Grid();
            loadingGrid.BackgroundColor = Color.Black;
            loadingGrid.Opacity = 0.5;
            loadingGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            loadingGrid.Children.Add(new ActivityIndicator()
            {
                IsRunning = true,
                WidthRequest = 50,
                HeightRequest = 50,
                Color = Color.Blue,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            });
            loadingGrid.SetBinding(Grid.IsVisibleProperty, new Binding("IsBusy"));

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = 150 });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(new StackLayout { Padding = padding, Children = { imgLogo } }, 0, 0);
            grid.Children.Add(new StackLayout { Padding = padding, Children = { labStep } }, 0, 0);

            grid.Children.Add(grid1, 0, 1);
            grid.Children.Add(grid2, 0, 1);
            grid.Children.Add(grid3, 0, 1);
            grid.Children.Add(grid4, 0, 1);
            grid.Children.Add(grid4Add, 0, 1);
            grid.Children.Add(grid4Breed, 0, 1);
            grid.Children.Add(grid5, 0, 1);

            grid.Children.Add(loadingGrid, 0, 0);
            Grid.SetRowSpan(loadingGrid, 2);

            grid1.IsVisible = true;
            grid2.IsVisible = false;
            grid3.IsVisible = false;
            grid4.IsVisible = false;
            grid4Add.IsVisible = false;
            grid4Breed.IsVisible = false;
            grid5.IsVisible = false;

            Content = grid;
        }

        private void Step1()
        {
            MyPicker titlePicker = new MyPicker
            {
                WidthRequest = 120,
                HeightRequest = 50
            };
            titlePicker.SetBinding(ExtendedPicker.ItemsSourceProperty, new Binding("TitleList", BindingMode.TwoWay));
            titlePicker.SetBinding(ExtendedPicker.SelectedItemProperty, new Binding("SelectedTitle", BindingMode.TwoWay));

            var firstEntry = new MyEntry
            {
                Style = entryStyle
            };
            firstEntry.SetBinding(Entry.TextProperty, new Binding("FirstName"));

            var lastEntry = new MyEntry
            {
                Style = entryStyle
            };
            lastEntry.SetBinding(Entry.TextProperty, new Binding("LastName"));

            var titleSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Start,
                Padding = new Thickness(Device.OnPlatform(150, 165, 0), 0, 0, 0),
                Children =
                {
                    new Label
                    {
                        Text = AppResources.Title,
                        Style=labStyle
                    },
                    titlePicker
                }
            };

            var firstSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = AppResources.FirstName,
                        Style=labStyle
                    },
                    firstEntry
                }
            };

            var lastSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = AppResources.LastName,
                        Style=labStyle
                    },
                    lastEntry
                }
            };

            Button btnStep1Back = new Button
            {
                Text = AppResources.Back,
                Style = buttonBackStyle,
                HorizontalOptions = LayoutOptions.Start,
            };

            Button btnStep1Cancel = new Button
            {
                Text = AppResources.Cancel,
                Style = buttonBackStyle,
                HorizontalOptions = LayoutOptions.Center,
            };

            Button btnStep1Next = new Button
            {
                Text = AppResources.Next,
                Style = buttonNextStyle,
                HorizontalOptions = LayoutOptions.End,
            };

            var btnStep1Sl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep1Back } },
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep1Cancel } },
                    new StackLayout {Children= { btnStep1Next } }
                }
            };


            btnStep1Next.Clicked += delegate
            {
                if (string.IsNullOrEmpty(RegistrationViewModel.FirstName))
                {
                    DisplayAlert(AppResources.Error, AppResources.Enter_FristName, AppResources.OK);
                    return;
                }
                if (string.IsNullOrEmpty(RegistrationViewModel.LastName))
                {
                    DisplayAlert(AppResources.Error, AppResources.Enter_LaseName, AppResources.OK);
                    return;
                }
                grid1.IsVisible = false;
                grid2.IsVisible = true;
                labStep.Text = AppResources.Registration_Step2;
            };

            btnStep1Back.SetBinding(Button.CommandProperty, new Binding("ShowCheckInCommand"));
            btnStep1Cancel.SetBinding(Button.CommandProperty, new Binding("ShowCancelCommand"));

            grid1 = new Grid();
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid1.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            grid1.Children.Add(titleSl, 0, 0);

            grid1.Children.Add(firstSl, 0, 1);

            grid1.Children.Add(lastSl, 0, 2);

            grid1.Children.Add(btnStep1Sl, 0, 3);
        }

        private void Step2()
        {
            var streetEntry = new MyEntry
            {
                Style = entryStyle
            };
            streetEntry.SetBinding(Entry.TextProperty, new Binding("StreetAddress"));

            var cityEntry = new MyEntry
            {
                Style = entryStyle
            };
            cityEntry.SetBinding(Entry.TextProperty, new Binding("City"));

            MyPicker statePicker = new MyPicker
            {
                HeightRequest = 50,
                WidthRequest = 200
            };
            statePicker.SetBinding(ExtendedPicker.ItemsSourceProperty, new Binding("StateList", BindingMode.TwoWay));
            statePicker.SetBinding(ExtendedPicker.SelectedItemProperty, new Binding("SelectedState", BindingMode.TwoWay));

            var postalEntry = new MyEntry
            {
                HeightRequest = 50,
                WidthRequest = 200,
                TextColor = Color.Black,
                FontSize = 20
            };
            postalEntry.SetBinding(Entry.TextProperty, new Binding("PostalCode"));

            var streetSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = AppResources.Street_Address,
                        Style=labStyle
                    },
                    streetEntry
                }
            };

            var citySl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                   new Label
                    {
                        Text = AppResources.City,
                        Style=labStyle
                    },
                   cityEntry
                }
            };

            var stateSl = new StackLayout
            {
                Padding = new Thickness(0, 0, 60, 0),
                Children =
                {
                    new Label
                    {
                        Text = string.Format("{0}/{1}", AppResources.State, AppResources.Province),
                        Style=labStyle
                    },
                   statePicker
                }
            };

            var postalSl = new StackLayout
            {
                Children =
                {
                   new Label
                   {
                        Text = AppResources.Postal_Code,
                        Style=labStyle
                   },
                   postalEntry
                }
            };

            var stateAndPostalSl = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                  stateSl,
                  postalSl
                }
            };

            Button btnStep2Back = new Button
            {
                Text = AppResources.Back,
                Style = buttonBackStyle,
                HorizontalOptions = LayoutOptions.Start,

            };

            Button btnStep2Cancel = new Button
            {
                Text = AppResources.Cancel,
                Style = buttonBackStyle,
                HorizontalOptions = LayoutOptions.Center,
            };
            btnStep2Cancel.SetBinding(Button.CommandProperty, new Binding("ShowCancelCommand"));

            Button btnStep2Next = new Button
            {
                Text = AppResources.Next,
                Style = buttonNextStyle,
                HorizontalOptions = LayoutOptions.End,
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

            btnStep2Next.Clicked += delegate
            {
                if (string.IsNullOrEmpty(RegistrationViewModel.StreetAddress))
                {
                    DisplayAlert(AppResources.Error, AppResources.Enter_Street_Address, AppResources.OK);
                    return;
                }
                if (string.IsNullOrEmpty(RegistrationViewModel.City))
                {
                    DisplayAlert(AppResources.Error, AppResources.Enter_City, AppResources.OK);
                    return;
                }
                //|| !System.Text.RegularExpressions.Regex.IsMatch(RegistrationViewModel.PostalCode, @"/^\d{5}$/")
                if (string.IsNullOrEmpty(RegistrationViewModel.PostalCode))
                {
                    DisplayAlert(AppResources.Error, AppResources.Enter_Postal_Code, AppResources.OK);
                    return;
                }
                grid2.IsVisible = false;
                grid3.IsVisible = true;
                labStep.Text = AppResources.Registration_Step3;
            };

            btnStep2Back.Clicked += delegate
            {
                grid1.IsVisible = true;
                grid2.IsVisible = false;
                labStep.Text = AppResources.Registration_Step1;
            };

            grid2 = new Grid();
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            grid2.Children.Add(streetSl, 0, 0);

            grid2.Children.Add(citySl, 0, 1);

            grid2.Children.Add(stateAndPostalSl, 0, 2);

            grid2.Children.Add(btnStep2Sl, 0, 3);
        }

        private void Step3()
        {
            var phoneEntry = new MyEntry
            {
                Style = entryStyle
            };
            phoneEntry.SetBinding(Entry.TextProperty, new Binding("Phone"));

            var emailEntry = new MyEntry
            {
                Style = entryStyle
            };
            emailEntry.SetBinding(Entry.TextProperty, new Binding("Email"));

            var phoneSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = AppResources.Phone,
                        Style=labStyle
                    },
                    phoneEntry
                }
            };

            var emailSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = AppResources.Email,
                        Style=labStyle
                    },
                    emailEntry
                }
            };

            Button btnStep3Back = new Button
            {
                Text = AppResources.Back,
                Style = buttonBackStyle,
            };

            Button btnStep3Cancel = new Button
            {
                Text = AppResources.Cancel,
                Style = buttonBackStyle,
                HorizontalOptions = LayoutOptions.Center,
            };
            btnStep3Cancel.SetBinding(Button.CommandProperty, new Binding("ShowCancelCommand"));

            Button btnStep3Next = new Button
            {
                Text = AppResources.Next,
                Style = buttonNextStyle,
                HorizontalOptions = LayoutOptions.End,
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

            btnStep3Next.Clicked += delegate
            {
                if (string.IsNullOrEmpty(RegistrationViewModel.Phone))
                {
                    DisplayAlert(AppResources.Error, AppResources.Enter_Phone, AppResources.OK);
                    return;
                }
                if (string.IsNullOrEmpty(RegistrationViewModel.Email) || !System.Text.RegularExpressions.Regex.IsMatch(RegistrationViewModel.Email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                {
                    DisplayAlert(AppResources.Error, AppResources.Enter_Email, AppResources.OK);
                    return;
                }
                grid3.IsVisible = false;
                grid4.IsVisible = true;
                labStep.Text = AppResources.Registration_Step4;
            };

            btnStep3Back.Clicked += delegate
            {
                grid2.IsVisible = true;
                grid3.IsVisible = false;
                labStep.Text = AppResources.Registration_Step2;
            };

            grid3 = new Grid();
            grid3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(120) });
            grid3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            grid3.Children.Add(phoneSl, 0, 0);

            grid3.Children.Add(emailSl, 0, 1);

            grid3.Children.Add(btnStep3Sl, 0, 2);
        }

        private void Step4()
        {
            var padding = new Thickness(20, 0, 20, 0);

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
                Style = buttonBackStyle,
            };

            Button btnStep4Cancel = new Button
            {
                Text = AppResources.Cancel,
                Style = buttonBackStyle,
                HorizontalOptions = LayoutOptions.Center,
            };
            btnStep4Cancel.SetBinding(Button.CommandProperty, new Binding("ShowCancelCommand"));

            Button btnStep4Next = new Button
            {
                Text = AppResources.Next,
                Style = buttonNextStyle,
                HorizontalOptions = LayoutOptions.End,
            };

            var btnStep4Sl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
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
                   new Label
                   {
                        Text = AppResources.Who_Is_With,
                        Style=labStyle
                   },
                   line
                }
            };

            var addAnotherSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = AppResources.Add_Another,
                        Style=labStyle
                    },
                    new Label
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Text = AppResources.Companion,
                        Style=labStyle
                    }
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
                BackgroundColor = DataHelper.GetInstance().SecondaryColor
            }, 0, 0);
            addAnotherGrid.Children.Add(addAnotherSl, 0, 0);

            MyListView lstPatient = new MyListView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    Button btnCheckIn = new Button
                    {
                        BorderWidth = 2,
                        BorderColor = Color.Black,
                        WidthRequest = 100,
                        HeightRequest = 40,
                        BackgroundColor = DataHelper.GetInstance().PrimaryColor,
                        BorderRadius = 1,
                        Text = AppResources.Check_In,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Color.Black
                    };

                    btnCheckIn.SetBinding(Button.CommandParameterProperty, new Binding("RegistrationID"));
                    btnCheckIn.SetBinding(Button.IsVisibleProperty, "IsChecked", BindingMode.Default, new TrueToFalseConverter());
                    btnCheckIn.Clicked += (sender, e) =>
                    {
                        int id = (int)((Button)sender).CommandParameter;

                        selectPatientToCheckInItem = RegistrationViewModel.PatientList.Where(x => x.RegistrationID == id).FirstOrDefault();

                        RegistrationViewModel.PatientName = selectPatientToCheckInItem.Name;

                        var item = RegistrationViewModel.SpeciesList.Where(x => x.IsChecked == true).FirstOrDefault();
                        if (item != null)
                            item.IsChecked = false;

                        var selectedItem = RegistrationViewModel.SpeciesList.Where(x => x.ID == selectPatientToCheckInItem.SpeciesID).FirstOrDefault();
                        selectedItem.IsChecked = true;
                        RegistrationViewModel.SelectedSpecies = selectedItem;
                        if (!selectedItem.PrimaryDisplay)
                            RegistrationViewModel.NotPrimarySpeciesName = selectedItem.Name;

                        RegistrationViewModel.Breed = selectPatientToCheckInItem.Breed;
                        RegistrationViewModel.PatientGender = RegistrationViewModel.GenderList.Where(x => x == selectPatientToCheckInItem.Gender).FirstOrDefault();
                        if (selectPatientToCheckInItem.Birthdate != null)
                            RegistrationViewModel.PatientBirth = selectPatientToCheckInItem.Birthdate;

                        grid4.IsVisible = false;
                        grid4Add.IsVisible = true;
                    };

                    Image imgChecked = new Image();
                    imgChecked.VerticalOptions = LayoutOptions.Center;
                    imgChecked.Source = "checked_checkbox.png";
                    imgChecked.SetBinding(Image.IsVisibleProperty, new Binding("IsChecked"));

                    Image imgPatient = new Image();
                    imgPatient.HeightRequest = 60;
                    imgPatient.WidthRequest = 61;
                    imgPatient.VerticalOptions = LayoutOptions.Center;
                    imgPatient.SetBinding(Image.SourceProperty, "Logo", BindingMode.Default, converter: new ByteToSourceConverter());

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
                                   WidthRequest=100,
                                   Orientation=StackOrientation.Horizontal,
                                   Children =
                                   {
                                       btnCheckIn,
                                       imgChecked
                                   }
                               },
                               imgPatient,
                               labName
                            }
                        }
                    };
                })
            };

            lstPatient.HeightRequest = 300;
            lstPatient.SetBinding(ListView.ItemsSourceProperty, new Binding("PatientList"));

            var addAnotherSlClick = new TapGestureRecognizer();
            addAnotherSlClick.Tapped += delegate
            {
                grid4.IsVisible = false;
                grid4Add.IsVisible = true;
            };
            addAnotherSl.GestureRecognizers.Add(addAnotherSlClick);

            btnStep4Next.Clicked += delegate
            {
                //var item = RegistrationViewModel.PatientList.Where(x => x.IsChecked == true).FirstOrDefault();
                //if (item == null)
                //{
                //    DisplayAlert(AppResources.Error, AppResources.Choose_Patient, AppResources.OK);
                //    return;
                //}

                RegistrationViewModel.CheckedPatientList.Clear();
                foreach (var patientItem in RegistrationViewModel.PatientList)
                {
                    RegistrationViewModel.CheckedPatientList.Add(patientItem);
                }

                grid4.IsVisible = false;
                grid5.IsVisible = true;
                labStep.Text = AppResources.Registration_Step5;
            };

            btnStep4Back.Clicked += delegate
            {
                grid3.IsVisible = true;
                grid4.IsVisible = false;
                labStep.Text = AppResources.Registration_Step3;
            };

            //labCheckIn = new Label
            //{
            //    Text = AppResources.Check_In_A_New_Patient,
            //    FontSize = 20,
            //    TextColor = Color.Black,
            //    IsVisible = false,
            //    HeightRequest = 300
            //};
            //labCheckIn.SetBinding(ListView.IsVisibleProperty, "IsCheckeInOrAdd");

            grid4 = new Grid();
            grid4.Padding = padding;
            grid4.RowDefinitions.Add(new RowDefinition { Height = 80 });
            grid4.RowDefinitions.Add(new RowDefinition { Height = 40 });
            grid4.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid4.RowDefinitions.Add(new RowDefinition { Height = 100 });

            grid4.Children.Add(addAnotherGrid, 0, 0);

            grid4.Children.Add(whoIsSl, 0, 1);

            //grid4.Children.Add(labCheckIn, 0, 2);
            grid4.Children.Add(lstPatient, 0, 2);

            grid4.Children.Add(btnStep4Sl, 0, 3);
        }

        private void Step4AddPatient()
        {
            var patientNameEntry = new MyEntry
            {
                HorizontalOptions = LayoutOptions.Center,
                Style = entryStyle
            };
            patientNameEntry.SetBinding(Entry.TextProperty, new Binding("PatientName"));

            StackLayout patientNameSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = AppResources.Who_Is_With,
                        Style=labStyle
                    },
                    patientNameEntry
                }
            };

            Button btnStep4AddBack = new Button
            {
                Text = AppResources.Back,
                Style = buttonBackStyle,
            };

            Button btnStep4AddCancel = new Button
            {
                Text = AppResources.Cancel,
                Style = buttonBackStyle,
                HorizontalOptions = LayoutOptions.Center,
            };
            btnStep4AddCancel.SetBinding(Button.CommandProperty, new Binding("ShowCancelCommand"));

            Button btnStep4AddNext = new Button
            {
                Text = AppResources.Next,
                Style = buttonNextStyle,
                HorizontalOptions = LayoutOptions.End,
            };

            var btnStep4AddSl = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep4AddBack } },
                    new StackLayout {Padding=new Thickness(0,0,50,0), Children= { btnStep4AddCancel } },
                    new StackLayout { Children= { btnStep4AddNext } }
                }
            };

            btnStep4AddNext.Clicked += delegate
            {
                if (string.IsNullOrEmpty(RegistrationViewModel.PatientName))
                {
                    DisplayAlert(AppResources.Error, AppResources.Enter_Patient_Name, AppResources.OK);
                    return;
                }
                if (RegistrationViewModel.SelectedSpecies == null)
                {
                    DisplayAlert(AppResources.Error, AppResources.Choose_Species, AppResources.OK);
                    return;
                }
                grid4Add.IsVisible = false;
                grid4Breed.IsVisible = true;
            };

            btnStep4AddBack.Clicked += delegate
            {
                selectPatientToCheckInItem = null;

                grid4.IsVisible = true;
                grid4Add.IsVisible = false;
            };

            MyListView lstBreedPrimary = new MyListView
            {
                WidthRequest = 200,
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

                    Image imgSpecies = new Image();
                    imgSpecies.HeightRequest = 60;
                    imgSpecies.WidthRequest = 61;
                    imgSpecies.VerticalOptions = LayoutOptions.Center;
                    imgSpecies.SetBinding(Image.SourceProperty, "Logo", BindingMode.Default, converter: new ByteToSourceConverter());

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
                               imgSpecies
                            }
                        }
                    };
                })
            };

            lstBreedNotPrimary = new MyListView
            {
                WidthRequest = 200,
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

                    Image imgSpecies = new Image();
                    imgSpecies.HeightRequest = 60;
                    imgSpecies.WidthRequest = 61;
                    imgSpecies.VerticalOptions = LayoutOptions.Center;
                    imgSpecies.SetBinding(Image.SourceProperty, "Logo", BindingMode.Default, converter: new ByteToSourceConverter());

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
                               imgSpecies
                            }
                        }
                    };
                })
            };

            lstBreedNotPrimary.IsVisible = false;

            lstBreedPrimary.SetBinding(ListView.ItemsSourceProperty, new Binding("SpeciesPrimaryList"));
            lstBreedNotPrimary.SetBinding(ListView.ItemsSourceProperty, new Binding("SpeciesNotPrimaryList"));

            lstBreedPrimary.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedSpecies", BindingMode.TwoWay));
            lstBreedNotPrimary.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedSpecies", BindingMode.TwoWay));

            lstBreedPrimary.ItemTapped += LstBreedPrimary_ItemTapped;
            lstBreedNotPrimary.ItemTapped += LstBreedPrimary_ItemTapped;

            Button btnOther = new Button
            {
                TextColor = Color.Black,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2,
                HeightRequest = 50,
                WidthRequest = 225,
                BackgroundColor = Color.Transparent
            };
            btnOther.SetBinding(Button.TextProperty, "NotPrimarySpeciesName");
            btnOther.Clicked += delegate
            {
                if (lstBreedNotPrimary.IsVisible)
                    lstBreedNotPrimary.IsVisible = false;
                else
                    lstBreedNotPrimary.IsVisible = true;
            };

            StackLayout notPrimarySl = new StackLayout
            {
                Children =
                {
                    new Label {Text=AppResources.Other,TextColor=Color.Black,FontSize=20, },
                    btnOther,
                    lstBreedNotPrimary
                }
            };
            notPrimarySl.SetBinding(StackLayout.IsVisibleProperty, "IsShowNotPrimarySl");

            grid4Add = new Grid();
            grid4Add.RowDefinitions.Add(new RowDefinition { Height = 100 });
            grid4Add.RowDefinitions.Add(new RowDefinition { Height = 300 });
            grid4Add.RowDefinitions.Add(new RowDefinition { Height = 80 });

            grid4Add.Children.Add(patientNameSl, 0, 0);

            grid4Add.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new StackLayout {Padding=new Thickness(0,0,40,0),Children= { lstBreedPrimary } } ,
                    notPrimarySl
                }
            }, 0, 1);

            grid4Add.Children.Add(btnStep4AddSl, 0, 2);
        }

        private void Step4AddBreed()
        {
            var breedEntry = new MyEntry
            {
                HeightRequest = 50,
                WidthRequest = 470,
                TextColor = Color.Black,
                FontSize = 20
            };
            breedEntry.SetBinding(Entry.TextProperty, new Binding("Breed"));

            MyPicker genderPicker = new MyPicker
            {
                BackgroundColor = Color.White,
                WidthRequest = 150,
                HeightRequest = 50
            };
            genderPicker.SetBinding(ExtendedPicker.ItemsSourceProperty, new Binding("GenderList"));
            genderPicker.SetBinding(ExtendedPicker.SelectedItemProperty, new Binding("PatientGender", BindingMode.TwoWay));

            MyDatePicker datePicker = new MyDatePicker
            {
                WidthRequest = 150,
                HeightRequest = 50
            };
            datePicker.SetBinding(DatePicker.DateProperty, new Binding("PatientBirth", BindingMode.TwoWay));

            Button btnStep4OtherBack = new Button
            {
                Text = AppResources.Back,
                Style = buttonBackStyle,
                HorizontalOptions = LayoutOptions.Start,
            };

            Button btnStep4OtherCancel = new Button
            {
                Text = AppResources.Cancel,
                Style = buttonBackStyle,
                HorizontalOptions = LayoutOptions.Center,
            };
            btnStep4OtherCancel.SetBinding(Button.CommandProperty, new Binding("ShowCancelCommand"));

            Button btnStep4OtherNext = new Button
            {
                Text = AppResources.Next,
                Style = buttonNextStyle,
                WidthRequest = 120,
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
                    new Label
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Text = AppResources.Breed,
                        Style=labStyle
                    },
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
                        Style=labStyle
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
                        Style=labStyle
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

            btnStep4OtherNext.Clicked += delegate
            {
                if (string.IsNullOrEmpty(RegistrationViewModel.Breed))
                {
                    DisplayAlert(AppResources.Error, AppResources.Enter_Breed, AppResources.OK);
                    return;
                }

                if (selectPatientToCheckInItem == null)
                {
                    int id = 1;
                    if (RegistrationViewModel.PatientList.Count != 0)
                        id = RegistrationViewModel.PatientList.Select(x => x.RegistrationID).Max() + 1;
                    
                    DisplayPatientModel patientInfo = new DisplayPatientModel
                    {
                        IsChecked = false,
                        Name = RegistrationViewModel.PatientName,
                        Breed = RegistrationViewModel.Breed,
                        Gender = RegistrationViewModel.PatientGender,
                        Birthdate = RegistrationViewModel.PatientBirth,
                        SpeciesID = RegistrationViewModel.SelectedSpecies.ID,
                        Logo = RegistrationViewModel.SelectedSpecies.Logo,
                        Species = RegistrationViewModel.SelectedSpecies.Name,
                        RegistrationID = id
                    };
                    RegistrationViewModel.PatientList.Add(patientInfo);
                }
                else
                {
                    var item = RegistrationViewModel.PatientList.Where(x => x.RegistrationID == selectPatientToCheckInItem.RegistrationID).FirstOrDefault();
                    item.IsChecked = true;
                    item.Name = RegistrationViewModel.PatientName;
                    item.Gender = RegistrationViewModel.PatientGender;
                    item.Breed = RegistrationViewModel.Breed;
                    item.Birthdate = RegistrationViewModel.PatientBirth;
                    item.Species = RegistrationViewModel.SelectedSpecies.Name;

                    selectPatientToCheckInItem = null;
                }

                RegistrationViewModel.Breed = null;
                RegistrationViewModel.PatientName = null;

                labStep.Text = AppResources.Registration_Step4;
                grid4Breed.IsVisible = false;
                grid4.IsVisible = true;
            };

            btnStep4OtherBack.Clicked += delegate
            {
                grid4Add.IsVisible = true;
                grid4Breed.IsVisible = false;
            };

            grid4Breed = new Grid();
            grid4Breed.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            grid4Breed.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
            grid4Breed.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });

            grid4Breed.Children.Add(breedSl, 0, 0);

            grid4Breed.Children.Add(genderAndBirthSl, 0, 1);

            grid4Breed.Children.Add(btnStep4OtherSl, 0, 2);
        }

        private void Step5()
        {
            const int fontSize = 25;

            Label labTitle = new Label
            {
                TextColor = Color.Black,
                FontSize = fontSize
            };

            Label labFirstName = new Label
            {
                TextColor = Color.Black,
                FontSize = fontSize
            };

            Label labLastName = new Label
            {
                TextColor = Color.Black,
                FontSize = fontSize
            };

            Label labStreetAddress = new Label
            {
                TextColor = Color.Black,
                FontSize = fontSize
            };

            Label labCity = new Label
            {
                TextColor = Color.Black,
                FontSize = fontSize
            };

            Label labState = new Label
            {
                TextColor = Color.Black,
                FontSize = fontSize
            };

            Label labZiP = new Label
            {
                TextColor = Color.Black,
                FontSize = fontSize
            };

            Label labPhone = new Label
            {
                TextColor = Color.Black,
                FontSize = fontSize
            };

            Label labEmail = new Label
            {
                TextColor = Color.Black,
                FontSize = fontSize
            };

            Label labPatientName = new Label
            {
                TextColor = Color.Black,
                FontSize = fontSize
            };

            Button btnEditClient = new Button
            {
                Text = AppResources.Edit,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2,
                BackgroundColor = DataHelper.GetInstance().SecondaryColor,
                HeightRequest = 40,
                TextColor = Color.Black
            };
            btnEditClient.Clicked += delegate
            {
                grid1.IsVisible = true;
                grid5.IsVisible = false;
                labStep.Text = AppResources.Registration_Step1;
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

            Button btnEditPatient = new Button
            {
                Text = AppResources.Edit,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2,
                BackgroundColor = DataHelper.GetInstance().SecondaryColor,
                HeightRequest = 40,
                TextColor = Color.Black
            };
            btnEditPatient.Clicked += delegate
            {
                grid4.IsVisible = true;
                grid5.IsVisible = false;
                labStep.Text = AppResources.Registration_Step4;
            };

            Button btnComplete = new Button
            {
                Text = AppResources.Complete,
                BorderColor = Color.Black,
                BorderRadius = 1,
                BorderWidth = 2,
                BackgroundColor = DataHelper.GetInstance().PrimaryColor,
                HeightRequest = 50,
                TextColor = Color.Black
            };
            btnComplete.SetBinding(Button.CommandProperty, new Binding("CompleteCommand"));

            Grid patientInfoGrid = new Grid();
            patientInfoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            patientInfoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 300 });
            patientInfoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 100 });
            patientInfoGrid.Children.Add(new StackLayout
            {
                Children =
                {
                    new Label { Text=AppResources.Patient_Information,TextColor=Color.Black,FontSize=25},
                    new BoxView { WidthRequest = 1, HeightRequest = 2, BackgroundColor = Color.Black, VerticalOptions = LayoutOptions.Start }
                }
            }, 0, 0);
            patientInfoGrid.Children.Add(btnEditPatient, 1, 0);

            StackLayout clientInfoSl = new StackLayout
            {
                Padding = new Thickness(0, 0, 0, 30),
                Children = {
                    clientInfoGrid,

                    new StackLayout
                    {
                        Orientation=StackOrientation.Horizontal,
                        Children =
                        {
                            labTitle,
                            labFirstName,
                            labLastName
                        }
                    },

                    labStreetAddress,

                    new StackLayout
                    {
                        Orientation=StackOrientation.Horizontal,
                        Children =
                        {
                            labCity,
                            labState,
                            labZiP
                        }
                    },

                    labPhone,

                    labEmail
                }
            };

            ListView lstPatient = new MyListView
            {
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    Label labName = new Label();
                    labName.TextColor = Color.Black;
                    labName.FontSize = 25;
                    labName.VerticalOptions = LayoutOptions.Center;
                    labName.SetBinding(Label.TextProperty, "Name");

                    Label labSpecies = new Label();
                    labSpecies.TextColor = Color.Black;
                    labSpecies.FontSize = 25;
                    labSpecies.VerticalOptions = LayoutOptions.Center;
                    labSpecies.SetBinding(Label.TextProperty, "Species");

                    Label labBreed = new Label();
                    labBreed.TextColor = Color.Black;
                    labBreed.FontSize = 25;
                    labBreed.VerticalOptions = LayoutOptions.Center;
                    labBreed.SetBinding(Label.TextProperty, "Breed");

                    Label labPatientGender = new Label();
                    labPatientGender.TextColor = Color.Black;
                    labPatientGender.FontSize = 25;
                    labPatientGender.VerticalOptions = LayoutOptions.Center;
                    labPatientGender.SetBinding(Label.TextProperty, "Gender");

                    Label labPatientBirth = new Label();
                    labPatientBirth.TextColor = Color.Black;
                    labPatientBirth.FontSize = 25;
                    labPatientBirth.VerticalOptions = LayoutOptions.Center;
                    labPatientBirth.SetBinding(Label.TextProperty, new Binding("Birthdate", stringFormat: ("{0:dd/MM/yyyy}")));

                    return new MyViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Vertical,
                            Children =
                            {
                               labName,
                               labSpecies,
                               labBreed,
                               new StackLayout
                               {
                                   Orientation=StackOrientation.Horizontal,
                                   Children=
                                   {
                                       labPatientGender,
                                       new Label { FontSize=25,Text="-",VerticalOptions=LayoutOptions.Center,TextColor=Color.Black },
                                       labPatientBirth
                                   }
                               }
                            }
                        }
                    };
                })
            };
            lstPatient.SetBinding(ListView.ItemsSourceProperty, new Binding("CheckedPatientList"));

            StackLayout patientInfoSl = new StackLayout
            {
                Children =
                {
                    patientInfoGrid,
                    lstPatient
                }
            };

            grid5 = new Grid();
            grid5.Padding = new Thickness(20, 0, 20, 0);
            grid5.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid5.RowDefinitions.Add(new RowDefinition { Height = 400 });
            grid5.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid5.Children.Add(clientInfoSl, 0, 0);
            grid5.Children.Add(patientInfoSl, 0, 1);
            grid5.Children.Add(new StackLayout { Padding = new Thickness(200, 0, 200, 0), Children = { btnComplete } }, 0, 2);

            //Binding
            labTitle.SetBinding(Label.TextProperty, new Binding("SelectedTitle"));
            labFirstName.SetBinding(Label.TextProperty, new Binding("FirstName"));
            labLastName.SetBinding(Label.TextProperty, new Binding("LastName"));

            labStreetAddress.SetBinding(Label.TextProperty, new Binding("StreetAddress"));
            labCity.SetBinding(Label.TextProperty, new Binding("City"));
            labState.SetBinding(Label.TextProperty, new Binding("SelectedState.State"));
            labZiP.SetBinding(Label.TextProperty, new Binding("PostalCode"));

            labPhone.SetBinding(Label.TextProperty, new Binding("Phone"));
            labEmail.SetBinding(Label.TextProperty, new Binding("Email"));
        }

        private void LstBreedPrimary_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DisplaySpeciesModel model = e.Item as DisplaySpeciesModel;
            if (model.IsChecked)
                return;

            foreach (var item in RegistrationViewModel.SpeciesNotPrimaryList)
            {
                if (item.IsChecked == true)
                {
                    item.IsChecked = false;
                    break;
                }
            }

            foreach (var item in RegistrationViewModel.SpeciesPrimaryList)
            {
                if (item.IsChecked == true)
                {
                    item.IsChecked = false;
                    break;
                }
            }
            model.IsChecked = true;

            if (model.PrimaryDisplay)
                RegistrationViewModel.NotPrimarySpeciesName = null;
            else
            {
                lstBreedNotPrimary.IsVisible = false;
                RegistrationViewModel.NotPrimarySpeciesName = model.Name;
            }


            if (selectPatientToCheckInItem != null)
                RegistrationViewModel.PatientList.Where(x => x.ID == selectPatientToCheckInItem.ID).FirstOrDefault().SpeciesID = model.ID;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RegistrationViewModel.SendMessage += RegistrationViewModel_SendMessage;
        }

        private void RegistrationViewModel_SendMessage(object sender, string e)
        {
            DisplayAlert(AppResources.Error, e, AppResources.OK);
        }
    }
}
