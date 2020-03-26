using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace VideoPlayer.Common
{
    public class VideoViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Site { get; set; }
        public Boolean Favorite { get; set; }
    }

    public class VideoDetailModel
    {
        public string Image { get; set; }
        public string Description { get; set; }
        public List<VideoViewModel> Vvm { get; set; }
    }
}
