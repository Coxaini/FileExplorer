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

                SelectedDirectory.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(AllFilesAndDirs)); };

                RaisePropertyChanged(nameof(FileCount));
                RaisePropertyChanged(nameof(AllFilesAndDirs));
                RaisePropertyChanged(nameof(FilesSize));

                RaisePropertyChanged(nameof(CurrentDisc));
                RaisePropertyChanged(nameof(DiskFilling));
                RaisePropertyChanged(nameof(FreeSpaceOnDisc));

                RaisePropertyChanged(nameof(ExtensionFilters));
                ExtensionFilter = "All Files";
            }
        }

        public string ExtensionFilter { get => SelectedDirectory.ShowFilter; set
            {
                if (SelectedDirectory != null)
                    SelectedDirectory.ShowFilter = value;
            } }

        public List<string> ExtensionFilters { get
            {
                if (SelectedDirectory != null)
                    return SelectedDirectory.GetFileExtensions();
                else
                    return new List<string> { "All Files" };
            } }

        public List<IFileable> AllFilesAndDirs { get {
                if (SelectedDirectory != null)
                    return SelectedDirectory.AllFilesAndDirs.ToList();
                else
                    return new List<IFileable>();
            } }

        public List<IFileable> AllFiles { get {
                if (SelectedDirectory != null)
                    return SelectedDirectory.Files.ToList();
                else
                    return new List<IFileable>();
            } }

        private List<DriveM> driveMs = DriveInfo.GetDrives().Select(x=>new DriveM(x)).ToList();

        public DriveM CurrentDisc { get {
                if (SelectedDirectory != null)
                    return driveMs.Find(x=>x.DiscName == Path.GetPathRoot(SelectedDirectory.FullName)) ?? driveMs[0];
                else
                    return driveMs[0];
            }
        }
        public string FreeSpaceOnDisc { get {

                return $"{CurrentDisc.AvailableFreeSpace / 1073741824} Gb вільно із {CurrentDisc.TotalSpace / 1073741824} Gb";

        } }

        public double DiskFilling { get => 100d - (CurrentDisc.AvailableFreeSpace * 1d / CurrentDisc.TotalSpace) * 100; }

        public int FileCount { get => AllFiles.Count(); }
        public long FilesSize { get => AllFiles.Select((x) => x.Size).Sum(); }

        public DelegateCommand<Directory> SelectedDirectoryCommand { get;}
        public DelegateCommand<IFileable> OpenDirectoryCommand { get;}

        public MainViewModel()
        {
            foreach (var item in Directory.GetDirectoryDrives())
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
