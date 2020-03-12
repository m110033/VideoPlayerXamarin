using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    }
}