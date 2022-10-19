using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileManager.Models
{
    public class Directory : TreeViewItemBase, IFileable
    {
        private string _name;
        private string _fullName;

        public ObservableCollection<Directory> Children { get; } = new ObservableCollection<Directory>();

        public Directory( string name ,string fullName)
        {
            FullName = fullName;
            Name = name;

            if (FullName == "*")
                return;
            if(Children.Count == 0)
            Children.Add(new Directory("*", "*"));

            
            directoryInfo = new DirectoryInfo(FullName);
            
           


        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public DateTime LastWriteTime { get {
                if (directoryInfo != null)
                    return directoryInfo.LastWriteTime;
                else 
                    return new DateTime(1999, 1, 1); }}

        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                RaisePropertyChanged("FullName");
            }
        }

        public long Size { get => 0; }

        private DirectoryInfo directoryInfo;

        public static ObservableCollection<Directory> GetDrives()
        {
            ObservableCollection<Directory> directories = new ObservableCollection<Directory>();
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            foreach (DriveInfo driveInfo in driveInfos)
            {
                directories.Add(new Directory(driveInfo.Name, driveInfo.Name));
            }
            return directories;
        }
        public override void ClearData()
        {
            if (IsSelected)
                return;

            Children.Clear();
        }
        public override void LoadData()
        {
            Children.Clear();

            try
            {
                foreach (DirectoryInfo subDir in directoryInfo.GetDirectories())
                {
                    Directory directory = new Directory(subDir.Name ,subDir.FullName);
                    Children.Add(directory);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
