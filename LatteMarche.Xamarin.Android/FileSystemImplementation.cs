using System;
using System.Collections.Generic;
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
        public string GetExternalStorage()
        {
            //Context context = Android.App.Application.Context;
            //var filePath = context.GetExternalFilesDir("");

            //var filePath = context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads);

            //return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).Path;

            //return filePath.Path;
        }
    }
}