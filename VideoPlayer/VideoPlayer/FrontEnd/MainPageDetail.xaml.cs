using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayer.FrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageDetail : ContentPage
    {
        private Common.Tools tool { get; set; }
        public ObservableCollection<Common.VideoViewModel> videos { get; set; }
        public MainPageDetail()
        {
            InitializeComponent();

            defaultActivityIndicator.IsRunning = true;
            lstView.IsVisible = false;

            Thread t = new Thread(() =>
            {
                LoadData();
            });
            t.IsBackground = true;
            t.Start();
        }

        async private void LoadData()
        {
            if (tool == null)
            {
                tool = new Common.Tools();
            }
            String hostUrl = "https://www.jikzy.com"; 
            String html = tool.GetHtml(hostUrl);
            Regex regex = new Regex("<td class=\"l\"><a href=\"(.*?)\" target|_blank\">(.*?)<font color=\"red\">(.*?)</font>|<a href=\"(.*?)\" target=\"_blank\">(.*?)</a>|(\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2})", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(html);
            videos = new ObservableCollection<Common.VideoViewModel>();
            for(int i = 0; i < matches.Count; i++)
            {
                try
                {
                    GroupCollection linkG = matches[i++].Groups;
                    GroupCollection titleG = matches[i++].Groups;
                    GroupCollection typeG = matches[i++].Groups;
                    GroupCollection dateG = matches[i].Groups;
                    String link = String.Format("{0}{1}", hostUrl, linkG[1].ToString().Trim());
                    String title = String.Format("{0} - {1}", titleG[2].Value.Trim(), titleG[3].Value.Trim());
                    String typeLInk = typeG[4].Value.Trim();
                    int typeID = Convert.ToInt32(typeLInk.Replace("/?m=vod-type-id-", "").Replace(".html", ""));
                    String typeName = typeG[5].Value.Trim();
                    String date = dateG[6].Value.Trim();
                    if (typeID == 11 || typeID == 16 || typeID > 17)
                    {
                        continue;
                    }
                    videos.Add(new Common.VideoViewModel { Name = title, Date = date, Image = "", Link = link });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            List<String> titleList = new List<String>();
            for(int i = 0; i < videos.Count; i++)
            {
                titleList.Add(videos[i].Name);
            }
            titleList = tool.PostHtml("http://www.khngai.com/chinese/tools/convert.php", titleList);
            for (int i = 0; i < videos.Count; i++)
            {
                videos[i].Name = titleList[i];
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