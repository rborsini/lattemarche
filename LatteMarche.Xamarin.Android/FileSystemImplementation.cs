using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LatteMarche.Xamarin.Droid;
using LatteMarche.Xamarin.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileSystemImplementation))]
namespace LatteMarche.Xamarin.Droid
{
    public class FileSystemImplementation : IFileSystem
    {
        public void ExportDb(string dbFilePath)
        {

            var bytes = System.IO.File.ReadAllBytes(dbFilePath);

            var folderPath = GetExternalStorage();
            var fileCopyName = Path.Combine(folderPath, $"Database_{DateTime.Now:dd-MM-yyyy_HH-mm-ss-tt}.db");

            System.IO.File.WriteAllBytes(fileCopyName, bytes);
        }

        private string GetExternalStorage()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).Path;
        }
    }
}