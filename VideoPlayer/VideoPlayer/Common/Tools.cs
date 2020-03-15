using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Xamarin.Forms;

namespace VideoPlayer.Common
{
    public class Tools : ContentPage
    {
        public Tools()
        {
        }

        public String GetHtml(String url)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
            HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        public List<String> PostHtml(String url, List<String> dataList)
        {
            HttpClient client = new HttpClient();
            String data = String.Join("<br>", dataList.ToArray());
            string FormStuff = string.Format("data={0}&action=Simplified to Traditional", data);
            StringContent content = new StringContent(FormStuff, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
            String tempHtml = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(tempHtml);
            var container = document.DocumentNode.Descendants("textarea").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "wsuni");
            if (container != null)
            {
                String result = container.InnerText.Trim();
                return result.Split(new String[] { "<br>" }, StringSplitOptions.None).ToList();
            }
            else
            {
                return new List<String>();
            }
        }
    }
}