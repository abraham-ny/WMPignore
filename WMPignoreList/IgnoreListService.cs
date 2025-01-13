using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace WMPignoreList
{
    public class IgnoreListService
    {
        private WindowsMediaPlayer wmp;
        private List<string> ignoreList;

        public IgnoreListService()
        {
            wmp = new WindowsMediaPlayer();
            ignoreList = LoadIgnoreList();
        }

        public void StartMonitoring()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        FilterMediaLibrary();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    Task.Delay(5000).Wait(); // Check every 5 seconds
                }
            });
        }

        private void FilterMediaLibrary()
        {
            IWMPPlaylist allMedia = wmp.mediaCollection.getAll();
            for (int i = allMedia.count - 1; i >= 0; i--)
            {
                IWMPMedia media = allMedia.get_Item(i);
                string path = media.sourceURL;

                if (IsInIgnoreList(path))
                {
                    Console.WriteLine($"Removing {path} from library...");
                    //wmp.mediaCollection.remove(media);
                    wmp.mediaCollection.remove(media, false);
                }
            }
        }

        private bool IsInIgnoreList(string path)
        {
            return ignoreList.Any(ignorePath =>
                path.StartsWith(ignorePath, StringComparison.OrdinalIgnoreCase));
        }

        public List<string> LoadIgnoreList()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IgnoreList.txt");
            return File.Exists(filePath)
                ? File.ReadAllLines(filePath).ToList()
                : new List<string>();
        }

        public void AddToIgnoreList(string folderPath)
        {
            ignoreList.Add(folderPath);
            SaveIgnoreList();
        }

        public void RemoveFromIgnoreList(string folderPath)
        {
            ignoreList.Remove(folderPath);
            SaveIgnoreList();
        }

        private void SaveIgnoreList()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IgnoreList.txt");
            File.WriteAllLines(filePath, ignoreList);
        }
    }
}
