using FileManager.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Directory = FileManager.Models.Directory;

namespace FileManager.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public ObservableCollection<Directory> Directories { get; } = new ObservableCollection<Directory>();

        private Directory selectedDirectory;

        public Directory SelectedDirectory {
            get { return selectedDirectory; }
            set
            {
                SetProperty(ref selectedDirectory, value);
                SelectedDirectory?.LoadData();
                RaisePropertyChanged(nameof(FileCount));
                RaisePropertyChanged(nameof(AllFiles));
                RaisePropertyChanged(nameof(FilesSize));
            }
        }

        public ObservableCollection<IFileable> AllFiles { get {
                if (SelectedDirectory != null)
                    return SelectedDirectory.AllFilesAndDirs;
                else
                    return new ObservableCollection<IFileable>();
         }}

       
        public int FileCount { get => AllFiles.Where((x)=>x is FileManager.Models.File).Count(); }
        public long FilesSize { get => AllFiles.Select((x) => x.Size).Sum(); }

        public DelegateCommand<Directory> SelectedDirectoryCommand { get;}
        public DelegateCommand<IFileable> OpenDirectoryCommand { get;}

        public MainViewModel()
        {
            foreach (var item in Directory.GetDrives())
            {
                Directories.Add(item);
            }

            SelectedDirectoryCommand = new DelegateCommand<Directory>((d) => { 
                SelectedDirectory = d;
            });

            OpenDirectoryCommand = new DelegateCommand<IFileable>((d) => {
                if (d == null) return;
                if(d is Directory dir)
                {
                    SelectedDirectory.IsExpanded = true;
                    SelectedDirectory = dir;
                    //SelectedDirectory.IsSelected = true;
                    SelectedDirectory.IsExpanded = true;
                }
            });

        }




    }
}
