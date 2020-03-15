using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayer.FrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        public ListView ListView;

        public MainPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageMasterMenuItem> MenuItems { get; set; }

            public MainPageMasterViewModel()
            {
                //String hostUrl = "https://drive.google.com/uc?export=download&id=";
                //MenuItems = new ObservableCollection<MainPageMasterMenuItem>(new[]
                //{
                //    new MainPageMasterMenuItem { Link = String.Format("{0}{1}", hostUrl, "1d2AQ-8rF9HxuioMwK9Uu2ujVgTiFxf0z"), Title = "動漫", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                //    new MainPageMasterMenuItem { Link = String.Format("{0}{1}", hostUrl, "1iUIcp-IeD8rjn-a84uS9MoslYeoBWe5X"), Title = "電影", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                //    new MainPageMasterMenuItem { Link = String.Format("{0}{1}", hostUrl, "1gkrl_72IZnsnDKNkcJhkeXGmfmBb73qK"), Title = "陸劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                //    new MainPageMasterMenuItem { Link = String.Format("{0}{1}", hostUrl, "1ZYjAzED4n6UxgiJvmgoB7jSyLUX7dX_d"), Title = "日劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                //    new MainPageMasterMenuItem { Link = String.Format("{0}{1}", hostUrl, "18gxyuOnIK7LCZON-Q6a3YMuWiAhSK6NV"), Title = "韓劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                //    new MainPageMasterMenuItem { Link = String.Format("{0}{1}", hostUrl, "1vYffF2yHm4Ir0wumoWU-J3uDf-T3wGsY"), Title = "連戲劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                //    new MainPageMasterMenuItem { Link = String.Format("{0}{1}", hostUrl, "1Cg7vGk_lHhjs62Tgy5lYDWx_sNIdc4SA"), Title = "港劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                //    new MainPageMasterMenuItem { Link = String.Format("{0}{1}", hostUrl, "1HbvikpZuXhQq5RhTPwwM0VrmFbPLmWBR"), Title = "美劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                //});
                MenuItems = new ObservableCollection<MainPageMasterMenuItem>(new[]
                {
                    new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-4-pg-"), Title = "動漫", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                    new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-1-pg-"), Title = "電影", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                    new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-12-pg-"), Title = "陸劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                    new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-14-pg-"), Title = "日劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                    new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-17-pg-"), Title = "韓劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                    new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-2-pg-"), Title = "連戲劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                    new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-13-pg-"), Title = "港劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                    new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-15-pg-"), Title = "歐美劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}