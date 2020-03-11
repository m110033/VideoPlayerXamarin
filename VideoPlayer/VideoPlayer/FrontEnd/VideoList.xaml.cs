using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VideoPlayer.FrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoList : ContentPage
    {
        public ObservableCollection<Common.VideoViewModel> videos { get; set; }
        public VideoList()
        {
            InitializeComponent();

            videos = new ObservableCollection<Common.VideoViewModel>();
            videos.Add(new Common.VideoViewModel { Name = "Tomato", Type = "Fruit", Image = "tomato.png" });
            videos.Add(new Common.VideoViewModel { Name = "Romaine Lettuce", Type = "Vegetable", Image = "lettuce.png" });
            videos.Add(new Common.VideoViewModel { Name = "Zucchini", Type = "Vegetable", Image = "zucchini.png" });
            lstView.ItemsSource = videos;
        }
    }
}