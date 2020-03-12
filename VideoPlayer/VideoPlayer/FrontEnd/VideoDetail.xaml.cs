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
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayer.FrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoDetail : ContentPage
    {
        private Common.Tools tool { get; set; }
        public ObservableCollection<Common.VideoViewModel> videos { get; set; }
        public VideoDetail(String videoUrl)
        {
            InitializeComponent();

            if (tool == null)
            {
                tool = new Common.Tools();
            }
            String html = tool.GetHtml(videoUrl);
            String hostUrl = "https://www.jikzy.com";
            HtmlDocument document = new HtmlDocument();
            //your html stream
            document.LoadHtml(html);
            // Get Image Link
            var container = document.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "videoPic");
            if (container != null)
            {
                var image = container.Descendants("img").FirstOrDefault(x => x.Attributes.Contains("src"));
                if (image != null)
                {
                    videoImage.Source = String.Format("{0}/{1}", hostUrl, image.Attributes["src"].Value);
                }
            }
            // Get Video Detail
            container = document.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "videoDetail");
            if (container != null)
            {
                videoDescription.Text = container.InnerText.Trim();
            }
            Regex regex = new Regex("value=\"(.*?)\\$(.*?).m3u8\" checked=\"", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(html);

            videos = new ObservableCollection<Common.VideoViewModel>();
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                String title = groups[1].Value.Trim();
                String link = groups[2].Value.Trim() + ".m3u8";
                videos.Add(new Common.VideoViewModel { Name = title, Type = "", Image = "", Link = link });
            }
            lstView.ItemsSource = videos;
            lstView.ItemSelected += ListView_ItemSelected;
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