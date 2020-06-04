using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using VideoPlayer.Parser;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayer.FrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoList : ContentPage
    {
        private String site = "";
        private String videoUrl = "";
        private Boolean isBusy = false;
        private Common.Tools tool = new Common.Tools();
        private IPageParser pageParser;
        private Boolean isFavoritePage = false;
        private Common.Database database = null;
        private List<Common.VideoViewModel> favoriteVVM = null;
        private ObservableCollection<Common.VideoViewModel> videos = new ObservableCollection<Common.VideoViewModel>();

        public VideoList(String site, String videoUrl)
        {
            InitializeComponent();
            if (database == null)
            {
                database = new Common.Database();
                favoriteVVM = database.getVideo();
            }
            this.site = site;
            this.videoUrl = videoUrl;
            // Get Parser
            switch (this.site)
            {
                case "zuidazy":
                    pageParser = new ZUIDAZY();
                    break;
                case "jikzy":
                    pageParser = new JIKZY();
                    break;
            }
            isFavoritePage = (videoUrl.CompareTo("FAVORITE") == 0) ? true : false;
            listView.ItemsSource = videos;
            // Bind events
            listView.ItemSelected += ListView_ItemSelected;
            listView.ItemAppearing += listView_ItemAppearing;
            loadMoreButton.Clicked += LoadMoreButton_Clicked;
            // Load Videos
            if (isFavoritePage)
            {
                loadMoreButton.IsVisible = false;
                foreach (var video in favoriteVVM)
                {
                    video.Image = video.Favorite ? tool.LIKEURL : tool.DISLIKEURL;
                    videos.Add(video);
                }
            }
            else
            {
                LoadData();
            }
        }
        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (isBusy || videos.Count == 0)
                return;
            if (!isFavoritePage) LoadData();
        }
        private void listView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (isBusy || videos.Count == 0)
                return;
            if ((Common.VideoViewModel)e.Item == videos[videos.Count - 1] && !isFavoritePage)
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            if (isBusy) return;
            isBusy = defaultActivityIndicator.IsVisible = defaultActivityIndicator.IsRunning = true;
            loadMoreButton.IsVisible = false;
            Task.Run(async () =>
            {
                List<Common.VideoViewModel> vvm = pageParser.GetList(videoUrl);
                Device.BeginInvokeOnMainThread(() => {
                    foreach (var item in vvm) videos.Add(item);
                    isBusy = defaultActivityIndicator.IsVisible = defaultActivityIndicator.IsRunning = false;
                    loadMoreButton.IsVisible = true;
                });
            });
        }

        async private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Common.VideoViewModel;
            if (item == null)
                return;
            await Navigation.PushAsync(new VideoDetail(item.Site, item.Link));
            listView.SelectedItem = null;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            listView.ItemsSource = videos.Where(item => item.Name.Contains(listSearch.Text));
        }

        private void FavoriteButton_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            Common.VideoViewModel vvm = (Common.VideoViewModel)button.Source.BindingContext;
            vvm.Favorite = vvm.Favorite ^ true;
            if (vvm.Favorite)
            {
                button.Source = tool.LIKEURL;
                database.addVideo(vvm);
            }
            else
            {
                button.Source = tool.DISLIKEURL;
                database.removeVideo(vvm);
            }
        }
    }
}