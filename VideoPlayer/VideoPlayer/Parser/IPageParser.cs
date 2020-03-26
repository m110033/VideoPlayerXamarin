using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VideoPlayer.FrontEnd;

namespace VideoPlayer.Parser
{
    interface IPageParser
    {
        ObservableCollection<MainPageMasterMenuItem> mpmm { get; set; }
        List<Common.VideoViewModel> GetList(String URL);
        Common.VideoDetailModel GetDetail(String URL);
    }
}
