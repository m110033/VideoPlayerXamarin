using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class VideoDetail : ContentPage
    {
        private String site = "";
        private String videoUrl = "";
        private IPageParser pageParser;
        private Common.Tools tool = new Common.Tools();
        public ObservableCollection<Common.VideoViewModel> videos { get; set; }
        public VideoDetail(String site, String videoUrl)
        {
            InitializeComponent();
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
            videos = new ObservableCollection<Common.VideoViewModel>();
            lstView.ItemsSource = videos;
            lstView.ItemSelected += ListView_ItemSelected;
            LoadData();
        }
        private void LoadData()
        {
            defaultActivityIndicator.IsRunning = true;
            lstView.IsVisible = false;
            Task.Run(async () =>
            {
                Common.VideoDetailModel vdm = pageParser.GetDetail(videoUrl);
                Device.BeginInvokeOnMainThread(() => {
                    foreach (var item in vdm.Vvm) videos.Add(item);
                    videoImage.Source = vdm.Image;
                    videoDescription.Text = vdm.Description;
                    defaultActivityIndicator.IsRunning = false;
                    lstView.IsVisible = true;
                });
            });
        }

        async private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Common.VideoViewModel;
            if (item == null)
                return;
            await Navigation.PushModalAsync(new WebVideoPage(item.Link));
        }
    }
}