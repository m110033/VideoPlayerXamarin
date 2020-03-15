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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayer.FrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoList : ContentPage
    {
        private String hostUrl = "https://www.jikzy.com";

        private String pageUrl { get; set; }

        private int pageNum { get; set; }

        private Common.Tools tool { get; set; }

        private Boolean isBusy { get; set; }

        public ObservableCollection<Common.VideoViewModel> videos { get; set; }

        public VideoList(String videoUrl)
        {
            InitializeComponent();
            pageUrl = videoUrl;
            videos = new ObservableCollection<Common.VideoViewModel>();
            lstView.ItemsSource = videos;
            lstView.ItemSelected += ListView_ItemSelected;
            lstView.ItemAppearing += LstView_ItemAppearing;
            loadMoreButton.Clicked += LoadMoreButton_Clicked;
            pageNum++;
            LoadData();
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (isBusy || videos.Count == 0)
                return;
            AppearData();
        }

        private void LstView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (isBusy || videos.Count == 0)
                return;

            //hit bottom!
            if ((Common.VideoViewModel)e.Item == videos[videos.Count - 1])
            {
                AppearData();
            }
        }

        private void AppearData()
        {
            defaultActivityIndicator.IsRunning = true;
            defaultActivityIndicator.IsVisible = true;
            Task.Run(async () =>
            {
                pageNum++;
                await LoadData();

            });
        }

        async Task LoadData()
        {
            if (isBusy) return;
            isBusy = true;
            String videoUrl = String.Format("{0}{1}.html", pageUrl, pageNum);
            if (tool == null)
            {
                tool = new Common.Tools();
            }
            String html = tool.GetHtml(videoUrl);
            Regex regex = new Regex("<td class=\"l\"><a href=\"(.*?)\" target|_blank\">(.*?)<font color=\"red\">(.*?)</font>|\">(.*?)</font></td>", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(html);
            List<Common.VideoViewModel> vvm = new List<Common.VideoViewModel>();
            for (int i = 0; i < matches.Count; i++)
            {
                try
                {
                    GroupCollection linkG = matches[i++].Groups;
                    GroupCollection titleG = matches[i++].Groups;
                    GroupCollection dateG = matches[i].Groups;
                    String link = String.Format("{0}{1}", hostUrl, linkG[1].ToString().Trim());
                    String title = String.Format("{0} - {1}", titleG[2].Value.Trim(), titleG[3].Value.Trim());
                    String date = dateG[4].Value.Trim();
                    vvm.Add(new Common.VideoViewModel { Name = title, Date = date, Image = "", Link = link });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            List<String> titleList = new List<String>();
            for (int i = 0; i < vvm.Count; i++)
            {
                titleList.Add(vvm[i].Name);
            }
            titleList = tool.PostHtml("http://www.khngai.com/chinese/tools/convert.php", titleList);
            try
            {
                Device.BeginInvokeOnMainThread(() => {
                    for (int i = 0; i < vvm.Count; i++)
                    {
                        vvm[i].Name = titleList[i];
                        videos.Add(vvm[i]);
                    }
                    defaultActivityIndicator.IsRunning = false;
                    defaultActivityIndicator.IsVisible = false;
                    isBusy = false;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                defaultActivityIndicator.IsRunning = false;
                isBusy = false;
            }
            isBusy = false;
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