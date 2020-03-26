using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using VideoPlayer.Common;
using VideoPlayer.UWP;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseTool))]
namespace VideoPlayer.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new VideoPlayer.App());
        }
    }

    public class DatabaseTool : IDatabaseTool
    {
        public DatabaseTool()
        {

        }
        public async System.Threading.Tasks.Task BackupAsync(List<Common.VideoViewModel> vvm)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeChoices.Add("JSON File", new List<string>() { ".json" });
            picker.SuggestedFileName = "database";
            Windows.Storage.StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Common.VideoViewModel>));//initialises the serialiser
                    serializer.Serialize(stream.AsStreamForWrite(), vvm);
                    await stream.FlushAsync();
                    stream.Size = stream.Position;
                }
            }
        }
        public async System.Threading.Tasks.Task<List<Common.VideoViewModel>> RestoreAsync()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".json");
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            List<Common.VideoViewModel> deserializedList = new List<Common.VideoViewModel>();
            if (file != null)
            {
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Common.VideoViewModel>));//initialises the serialiser
                    deserializedList = (List<Common.VideoViewModel>)serializer.Deserialize(stream.AsStreamForRead());
                }
            }
            return deserializedList;
        }
    }
}
