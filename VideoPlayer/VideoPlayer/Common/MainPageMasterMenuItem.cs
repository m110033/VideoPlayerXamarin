using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayer.FrontEnd
{

    public class MainPageMasterMenuItem
    {
        public MainPageMasterMenuItem()
        {
            TargetType = typeof(MainPageMasterMenuItem);
        }
        public string Site { get; set; }
        public string Title { get; set; }

        public string Link { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
    }
}