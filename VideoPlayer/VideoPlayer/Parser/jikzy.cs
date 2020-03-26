using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using VideoPlayer.Common;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.ObjectModel;
using VideoPlayer.FrontEnd;

namespace VideoPlayer.Parser
{
    public class JIKZY : IPageParser
    {
        private const String SITE = "jikzy";
        private const String HOST = "https://www.jikzy.com";
        private Common.Tools tool = new Tools();
        private List<string> favoriteIDs = new List<string>();
        private List<int> filterIDs = new List<int>() {
            1,2,3,4,5,6,7,8,9,10,12,13,14,15,17,19,20
        };
        public ObservableCollection<MainPageMasterMenuItem> mpmm { get; set; }
        public String videlUrl { get; set; }
        public JIKZY()
        {
            mpmm = new ObservableCollection<MainPageMasterMenuItem>(new[]
            {
                new MainPageMasterMenuItem { Link = String.Format("{0}", "FAVORITE"), Title = "我的最愛", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-index-pg-"), Title = "最新影視", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-4-pg-"), Title = "動漫", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-1-pg-"), Title = "電影", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-19-pg-"), Title = "台劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-12-pg-"), Title = "陸劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-14-pg-"), Title = "日劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-17-pg-"), Title = "韓劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-2-pg-"), Title = "連戲劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-13-pg-"), Title = "港劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = String.Format("{0}", "https://www.jikzy.com/?m=vod-type-id-15-pg-"), Title = "歐美劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE }
            });
        }
        public List<Common.VideoViewModel> GetList(String URL)
        {
            HtmlDocument document = new HtmlDocument();
            List<Common.VideoViewModel> vvm = new List<Common.VideoViewModel>();
            String html = tool.GetHtml((videlUrl == null) ? URL : videlUrl);
            document.LoadHtml(html);
            Regex regex = new Regex("<td class=\"l\"><a href=\"(.*?)\" target|_blank\">(.*?)<font color=\"red\">(.*?)</font>|<a href=\"(.*?)\" target=\"_blank\">(.*?)</a>|(\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2})", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(html);
            for (int i = 0; i < matches.Count; i++)
            {
                try
                {
                    GroupCollection linkG = matches[i++].Groups;
                    GroupCollection titleG = matches[i++].Groups;
                    GroupCollection typeG = matches[i++].Groups;
                    GroupCollection dateG = matches[i].Groups;
                    String link = String.Format("{0}{1}", HOST, linkG[1].ToString().Trim());
                    String title = String.Format("{0} - {1}", titleG[2].Value.Trim(), titleG[3].Value.Trim());
                    String typeLInk = typeG[4].Value.Trim();
                    int typeID = Convert.ToInt32(typeLInk.Replace("/?m=vod-type-id-", "").Replace(".html", ""));
                    String typeName = typeG[5].Value.Trim();
                    String date = dateG[6].Value.Trim();
                    Regex idRegex = new Regex("id-(.*?).html", RegexOptions.Singleline);
                    Match match = idRegex.Match(link);
                    String videoID = match.Success ? match.Groups[1].Value : "-1";
                    if (!filterIDs.Contains(typeID))
                    {
                        continue;
                    }
                    String imageUrl = favoriteIDs.Contains(videoID) ? tool.LIKEURL : tool.DISLIKEURL;
                    Boolean favorite = favoriteIDs.Contains(videoID);
                    vvm.Add(new Common.VideoViewModel { ID = videoID, Name = title, Date = date, Image = imageUrl, Link = link, Favorite = favorite, Site = SITE });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            // Get Traditional Chinese Word
            List<String> titleList = (from data in vvm select data.Name).ToList<String>();
            titleList = tool.SCToTC(titleList);
            for (int i = 0; i < vvm.Count; i++)
            {
                vvm[i].Name = titleList[i];
            }
            // Get Next Page
            var nextPage = document.DocumentNode.Descendants("a").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "pagelink_a" && x.InnerText.Trim().CompareTo("下一页") == 0);
            if (nextPage != null)
            {
                videlUrl = String.Format("{0}{1}", HOST, nextPage.Attributes["href"].Value);
            }
            return vvm;
        }
        public Common.VideoDetailModel GetDetail(String URL)
        {
            Common.VideoDetailModel vdm = new Common.VideoDetailModel();
            vdm.Vvm = new List<VideoViewModel>();

            HtmlDocument document = new HtmlDocument();
            String html = tool.GetHtml(URL);
            document.LoadHtml(html);
            bool start = false;
            var container = document.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "movievod");
            if (container != null)
            {
                foreach (var liNode in container.Descendants("li"))
                {
                    if (liNode == null) continue;
                    if (liNode.InnerText.Trim() == "jsm3u8")
                    {
                        start = true;
                    }
                    if (!start) continue;
                    var inputNode = liNode.Descendants("input").FirstOrDefault(x => x.Attributes.Contains("value"));
                    if (inputNode == null) continue;
                    String inputValue = inputNode.Attributes["value"].Value;
                    if (inputValue.IndexOf(".m3u8") < 0) continue;
                    String[] segs = inputValue.Split('$');
                    String title = segs[0].Trim();
                    String link = segs[1].Trim();
                    vdm.Vvm.Add(new Common.VideoViewModel { Name = title, Type = "", Image = "", Link = link, Site = SITE });
                }
            }
            // Get Traditional Chinese Word
            List<String> titleList = (from data in vdm.Vvm select data.Name).ToList<String>();
            titleList = tool.SCToTC(titleList);
            for (int i = 0; i < vdm.Vvm.Count; i++)
            {
                vdm.Vvm[i].Name = titleList[i];
            }
            // Get Image Link
            container = document.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "videoPic");
            if (container != null)
            {
                var image = container.Descendants("img").FirstOrDefault(x => x.Attributes.Contains("src"));
                if (image != null)
                {
                    vdm.Image = String.Format("{0}/{1}", HOST, image.Attributes["src"].Value);
                }
            }
            // Get Video Detail
            container = document.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "videoDetail");
            if (container != null)
            {
                String content = container.InnerText.Trim();
                List<String> contentList = new List<String>();
                contentList.Add(content);
                contentList = tool.SCToTC(contentList);
                vdm.Description = contentList.First();
            }
            return vdm;
        }
    }
}
