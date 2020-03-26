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
    public class ZUIDAZY : IPageParser
    {
        private const String SITE = "zuidazy";
        private const String HOST = "http://www.zuidazy2.com";
        private Common.Tools tool = new Tools();
        private List<string> favoriteIDs = new List<string>();
        private List<int> filterIDs = new List<int>() {
            1,2,3,4,5,6,7,8,9,10,12,13,14,15,17,19,20,21
        };
        public ObservableCollection<MainPageMasterMenuItem> mpmm { get; set; }
        public String videlUrl { get; set; }
        public ZUIDAZY()
        {
            mpmm = new ObservableCollection<MainPageMasterMenuItem>(new[]
            {
                new MainPageMasterMenuItem { Link = "FAVORITE", Title = "我的最愛", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png" },
                new MainPageMasterMenuItem { Link = HOST, Title = "最新影視", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = "http://www.zuidazy2.com/?m=vod-type-id-3.html", Title = "綜藝", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = "http://www.zuidazy2.com/?m=vod-type-id-1.html", Title = "電影", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = "http://www.zuidazy2.com/?m=vod-type-id-4.html", Title = "動漫", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },                
                new MainPageMasterMenuItem { Link = "http://www.zuidazy2.com/?m=vod-type-id-19.html", Title = "台劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = "http://www.zuidazy2.com/?m=vod-type-id-12.html", Title = "陸劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = "http://www.zuidazy2.com/?m=vod-type-id-20.html", Title = "日劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = "http://www.zuidazy2.com/?m=vod-type-id-14.html", Title = "韓劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = "http://www.zuidazy2.com/?m=vod-type-id-15.html", Title = "歐美劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE },
                new MainPageMasterMenuItem { Link = "http://www.zuidazy2.com/?m=vod-type-id-21.html", Title = "海外劇", TargetType = typeof(VideoList), IconSource = "https://i.imgur.com/Zkw9VFU.png", Site = SITE }
            });
        }
        public List<Common.VideoViewModel> GetList(String URL)
        {
            HtmlDocument document = new HtmlDocument();
            List<Common.VideoViewModel> vvm = new List<Common.VideoViewModel>();
            String html = tool.GetHtml((videlUrl == null) ? URL : videlUrl);
            document.LoadHtml(html);
            var videoBlock = document.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "xing_vb");
            if (videoBlock != null)
            {
                foreach(var item in videoBlock.Descendants("li"))
                {
                    if (item.Descendants("span").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "tt") == null)
                        continue;
                    var node = item.Descendants("a").FirstOrDefault();
                    String link = (node != null) ? String.Format("{0}{1}", HOST, node.Attributes["href"].Value.Trim()) : "";
                    String title = (node != null) ? node.InnerText.Trim() : "";
                    node = item.Descendants("span").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "xing_vb6");
                    String date = (node != null) ? node.InnerText.Trim() : "";
                    Regex idRegex = new Regex("id-(.*?).html", RegexOptions.Singleline);
                    Match match = idRegex.Match(link);
                    String videoID = match.Success ? match.Groups[1].Value : "-1";
                    String imageUrl = favoriteIDs.Contains(videoID) ? tool.LIKEURL : tool.DISLIKEURL;
                    Boolean favorite = favoriteIDs.Contains(videoID);
                    vvm.Add(new Common.VideoViewModel { ID = videoID, Name = title, Date = date, Image = imageUrl, Link = link, Favorite = favorite, Site = SITE });
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
            var container = document.DocumentNode.Descendants("span").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "suf" && x.InnerText.Trim().IndexOf("m3u8") >= 0);
            if (container != null)
            {
                while(container.Name.ToLower() != "div")
                {
                    container = container.ParentNode;
                }
                foreach (var liNode in container.Descendants("li"))
                {
                    String linkTxt = liNode.InnerText.Trim();
                    if (linkTxt.IndexOf(".m3u8") < 0) continue;
                    String[] segs = linkTxt.Split('$');
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
            container = document.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "vodImg");
            if (container != null)
            {
                var image = container.Descendants("img").FirstOrDefault(x => x.Attributes.Contains("src"));
                if (image != null)
                {
                    vdm.Image = image.Attributes["src"].Value;
                }
            }
            // Get Video Detail
            container = document.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "vodinfobox");
            if (container != null)
            {
                String content = container.InnerText.Trim().Replace("  ", "");
                content += "\r\n\r\n";
                List<String> contentList = new List<String>();
                contentList.Add(content);
                contentList = tool.SCToTC(contentList);
                vdm.Description = contentList.First();
            }
            return vdm;
        }
    }
}
