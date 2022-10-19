using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace FileManager.Models
{
    public class Directory : TreeViewItemBase, IFileable
    {
        private string _name;
        private string _fullName;
        private DirectoryInfo directoryInfo;

        public ObservableCollection<Directory> Children { get; } = new ObservableCollection<Directory>();

        public Directory(string fullName)
        {
            FullName = fullName;
            
            if (FullName == "*")
                return;
            if(Children.Count == 0)
            Children.Add(new Directory("*"));

            directoryInfo = new DirectoryInfo(FullName);

            Name = directoryInfo.Name;
        }

        public string Name
        {
            get { return _name; }
            private set
            {
                //Change name of directory
               /* try
                {
                    directoryInfo.MoveTo(directoryInfo.Root.FullName + "\\" + value);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }*/
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
            private set
            {
              /*  try
                {
                    directoryInfo.MoveTo(value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }*/

                _fullName = value;
                RaisePropertyChanged("FullName");
            }
        }

        public long Size { get => 0; }

       

        public static ObservableCollection<Directory> GetDrives()
        {
            ObservableCollection<Directory> directories = new ObservableCollection<Directory>();
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            foreach (DriveInfo driveInfo in driveInfos)
            {
                directories.Add(new Directory( driveInfo.Name));
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
                    Directory directory = new Directory(subDir.FullName);
                    Children.Add(directory);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ObservableCollection<IFileable> GetAllFiles()
        {
            ObservableCollection<IFileable> files = new ObservableCollection<IFileable>(Children);

            try
            {
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    IFileable f = new File(file.FullName);
                    files.Add(f);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return files;

        }


    }
}
