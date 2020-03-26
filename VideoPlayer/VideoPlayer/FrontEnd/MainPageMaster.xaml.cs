using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VideoPlayer.Parser;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayer.FrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        private JIKZY jikzy = new JIKZY();
        private ZUIDAZY zuidazy = new ZUIDAZY();
        private Common.Database database = new Common.Database();
        private ObservableCollection<MainPageMasterMenuItem> menuItems = new ObservableCollection<MainPageMasterMenuItem>();
        public ListView ListView;
        public MainPageMaster()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.UWP)
            {
                restoreBtn.IsVisible = backupBtn.IsVisible = true;
            }
            ListView = MenuItemsListView;
            MenuPicker.Items.Add("jikzy");
            MenuPicker.Items.Add("zuidazy");
            MenuPicker.SelectedIndexChanged += MenuPicker_SelectedIndexChanged;
            //Default Item
            MenuPicker.SelectedIndex = 0;
            MenuItemsListView.ItemsSource = jikzy.mpmm;
        }

        private void MenuPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(MenuPicker.SelectedItem)
            {
                case "jikzy":
                    MenuItemsListView.ItemsSource = jikzy.mpmm;
                    break;
                case "zuidazy":
                    MenuItemsListView.ItemsSource = zuidazy.mpmm;
                    break;
            }
        }

        private void backupBtn_Clicked(object sender, EventArgs e)
        {
            database.BackupAsync();
        }

        private void restoreBtn_Clicked(object sender, EventArgs e)
        {
            database.RestoreAsync();
        }
    }
}