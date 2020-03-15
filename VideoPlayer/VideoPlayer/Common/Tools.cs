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

        public String PostHtml(String url, String data)
        {
            HttpClient client = new HttpClient();
            string FormStuff = string.Format("textbody={0}", data);
            StringContent content = new StringContent(FormStuff, Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
            String tempHtml = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            String target = "<textarea class=\"inputc\" id=\"textall\" readonly=\"readonly\">";
            int s = tempHtml.IndexOf(target) + target.Length;
            int e = tempHtml.IndexOf("</textarea") - s;
            return tempHtml.Substring(s, e);
        }
    }
}