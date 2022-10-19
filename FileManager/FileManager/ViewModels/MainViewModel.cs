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

        private Directory? selectedDirectory;

        public Directory? SelectedDirectory { 
            get { return selectedDirectory; }
            set
            {
                SetProperty(ref selectedDirectory, value);
                SelectedDirectory?.LoadData();
            }
        }

        

       
        public DelegateCommand<Directory> SelectedDirectoryCommand { get;}

        public MainViewModel()
        {
            foreach (var item in Directory.GetDrives())
            {
                Directories.Add(item);
            }

            SelectedDirectoryCommand = new DelegateCommand<Directory>((d) => { 
                SelectedDirectory = d;
            });
        }



    }
}
