using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using SQLite;
using Xamarin.Forms;

namespace VideoPlayer.Common
{
    public class Database : ContentPage
    {
        public SQLiteConnection db { get; set; }
        public Database()
        {
            var dbpath = Path.Combine(System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.LocalApplicationData), "video");
            db = new SQLiteConnection(dbpath, false);
            //db.DropTable<VideoViewModel>();
            db.CreateTable<VideoViewModel>();
        }
        public void addVideo(Common.VideoViewModel vvm)
        {
            IEnumerable<VideoViewModel> videos = db.Query<VideoViewModel>(String.Format("SELECT * FROM VideoViewModel WHERE Name = '{0}' AND ID = {1}", vvm.Name, vvm.ID));
            if (videos.Count() == 0)
            {
                db.Insert(vvm);
            }
        }
        public List<Common.VideoViewModel> getVideo()
        {
            List<Common.VideoViewModel> vvm = new List<VideoViewModel>();
            IEnumerable<VideoViewModel> videos = db.Query<VideoViewModel>("SELECT * FROM VideoViewModel");
            foreach(var video in videos)
            {
                vvm.Add(video);
            }
            return vvm;
        }
        public void removeVideo(Common.VideoViewModel vvm)
        {
            db.Execute(String.Format("DELETE FROM VideoViewModel WHERE Name = '{0}' AND ID = {1}", vvm.Name, vvm.ID));
        }
        public void removeVideo()
        {
            db.Execute("DELETE FROM VideoViewModel");
        }
        public async System.Threading.Tasks.Task BackupAsync()
        {
            //await DependencyService.Get<IDatabaseTool>().BackupAsync(getVideo());

            List<Common.VideoViewModel> vvm = getVideo();
            try
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DatabaseResource.txt");
                using (var writer = new System.IO.StreamWriter(fileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Common.VideoViewModel>));//initialises the serialiser
                    serializer.Serialize(writer, vvm);
                    await writer.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async System.Threading.Tasks.Task RestoreAsync()
        {
            //List<Common.VideoViewModel> vvms = await DependencyService.Get<IDatabaseTool>().RestoreAsync();

            try
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DatabaseResource.txt");
                List<Common.VideoViewModel> vvms;
                using (var reader = new StreamReader(fileName))
                {
                    var serializer = new XmlSerializer(typeof(List<Common.VideoViewModel>));
                    vvms = (List<Common.VideoViewModel>)serializer.Deserialize(reader);
                }
                removeVideo();
                foreach (var vvm in vvms)
                {
                    db.Insert(vvm);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    public interface IDatabaseTool
    {
        System.Threading.Tasks.Task BackupAsync(List<Common.VideoViewModel> vvm);
        System.Threading.Tasks.Task<List<Common.VideoViewModel>> RestoreAsync();
    }

}