using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace VideoPlayer.Common
{
    public class VideoViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public Command<Type> Command { get; set; }
    }
}
