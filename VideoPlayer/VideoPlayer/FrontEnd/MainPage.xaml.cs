using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayer.FrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Split;
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainPageMasterMenuItem;
            if (item == null)
                return;
            var page = (VideoList)Activator.CreateInstance(item.TargetType, item.Site, item.Link);
            page.Title = item.Title;
            Detail = new NavigationPage(page);
            if (Device.RuntimePlatform != Device.UWP)
            {
                IsPresented = false;
            }
            MasterPage.ListView.SelectedItem = null;
        }
    }
}