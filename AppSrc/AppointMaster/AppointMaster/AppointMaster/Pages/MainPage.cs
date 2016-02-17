using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace AppointMaster.Pages
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            var label1 = new Label
            {
                Text = AppointMaster.Resources.AppResources.String1,
                FontSize = 20,
                TextColor = Color.Black
            };

            var label2 = new Label
            {
                Text = AppointMaster.Resources.AppResources.String1,
                FontSize = 20
            };

            var label3 = new Label
            {
                Text = AppointMaster.Resources.AppResources.String1,
                FontSize = 20
            };

            var image = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "logo.png",
                VerticalOptions=LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HeightRequest=50,
                WidthRequest=100
            };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var sl = new StackLayout
            {
                Children = {
                    label1,
                    label2,
                    label3,
                }
            };

            grid.Children.Add(sl, 0, 0);
            grid.Children.Add(image, 0, 0);
            Content = grid;
            //Content = new StackLayout
            //{
            //    Children = {
            //        label1,
            //        label2,
            //        label3,
            //        image
            //    }
            //};

            //var rl = new RelativeLayout();
            //rl.Children.Add(sl,
            //Constraint.RelativeToView(sl, (Parent, sibling) =>
            //{
            //    return sibling.X + 20;
            //}), 
            //Constraint.RelativeToView(sl, (parent, sibling) =>
            //{
            //    return sibling.Y + 20;
            //}),
            //Constraint.RelativeToParent((parent) =>
            //{
            //    return parent.Width * .5;
            //}),
            //Constraint.RelativeToParent((parent) =>
            //{
            //    return parent.Height * .5;
            //}));
            BackgroundColor = Color.White;
            //helloResponse.SetBinding(Label.TextProperty, new Binding("YourNickname"));
        }
    }
}
