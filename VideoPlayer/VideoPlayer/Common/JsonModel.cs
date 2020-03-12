using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace VideoPlayer.Common
{
    public class Video
    {
        [JsonProperty("movies")]
        public List<VideoData> video { get; set; }
    }

    public class VideoData
    {
        public string m_idx { get; set; }
        public string m_img { get; set; }
        public string m_t { get; set; }
        public string m_p_l { get; set; }
        public string m_s_t { get; set; }
    }
}