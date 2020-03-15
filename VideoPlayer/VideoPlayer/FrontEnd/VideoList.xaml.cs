using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayer.FrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoList : ContentPage
    {
        private Common.Tools tool { get; set; }

        public ObservableCollection<Common.VideoViewModel> videos { get; set; }

        public VideoList(String videoUrl)
        {
            InitializeComponent();

            defaultActivityIndicator.IsRunning = true;
            lstView.IsVisible = false;

            Thread t = new Thread(() =>
            {
                LoadData(videoUrl);
            });
            t.IsBackground = true;
            t.Start();
        }

        async private void LoadData(String videoUrl)
        {
            if (tool == null)
            {
                tool = new Common.Tools();
            }
            String html = tool.GetHtml(videoUrl);
            videos = new ObservableCollection<Common.VideoViewModel>();
            Common.Video jsonObj = JsonConvert.DeserializeObject<Common.Video>(html);
            for (int i = jsonObj.video.Count - 1; i >= 0; i--)
            {
                Common.VideoData video = jsonObj.video[i];
                videos.Add(new Common.VideoViewModel { Name = video.m_t, Type = video.m_s_t, Image = video.m_img, Link = video.m_p_l });
            }
            Device.BeginInvokeOnMainThread(() => {
                lstView.ItemsSource = videos;
                lstView.ItemSelected += ListView_ItemSelected;
                defaultActivityIndicator.IsRunning = false;
                lstView.IsVisible = true;
            });
            
        }

        async private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Common.VideoViewModel;
            if (item == null)
                return;
            await Navigation.PushAsync(new VideoDetail(item.Link));
        }
    }
}