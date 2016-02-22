using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppointMaster.Controls
{
    public class IndexedPages : CarouselPage
    {
        private bool _internalPageChange;

        public IndexedPages()
        {
            CurrentPageChanged += OnCurrentPageChanged;
        }

        protected override bool OnBackButtonPressed()
        {
            if (SelectedIndex > 0)
            {
                SelectedIndex--;
                return true;
            }

            return base.OnBackButtonPressed();
        }

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create("SelectedIndex", typeof(int), typeof(IndexedPages), -1, BindingMode.TwoWay,
                (BindableProperty.ValidateValueDelegate)null, OnIndexPropertyChanged,
                (BindableProperty.BindingPropertyChangingDelegate)null, (BindableProperty.CoerceValueDelegate)null,
                (BindableProperty.CreateDefaultValueDelegate)null);

        private static void OnIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var indexedPages = bindable as IndexedPages;
            if (indexedPages != null && !indexedPages._internalPageChange)
            {
                var index = (int)newValue;
                indexedPages.SetCurrentPageByIndex(index);
            }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private void OnCurrentPageChanged(object sender, EventArgs eventArgs)
        {
            if (_internalPageChange)
                return;

            var indexedPages = sender as IndexedPages;
            if (indexedPages != null)
            {
                var currentPage = indexedPages.CurrentPage;
                SetSelectedIndexByPage(currentPage);
            }
        }

        protected virtual void SetSelectedIndexByPage(ContentPage contentPage)
        {
            _internalPageChange = true;
            var index = Children.IndexOf(contentPage);
            SelectedIndex = index;
            _internalPageChange = false;
        }

        protected virtual void SetCurrentPageByIndex(int index)
        {
            _internalPageChange = true;
            SelectedIndex = index;
            if (index > Children.Count - 1 || index == -1)
            {
                _internalPageChange = false;
                return;
            }

            var currentPage = Children[index];
            if (currentPage != null)
            {
                CurrentPage = currentPage;
            }

            _internalPageChange = false;
        }


        protected override ContentPage CreateDefault(object item)
        {
            var contentPage = new ContentPage();
            if (item != null)
                contentPage.Title = item.ToString();
            return contentPage;
        }
    }
}
