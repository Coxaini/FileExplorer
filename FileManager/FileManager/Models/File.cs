﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public class File : BindableBase, IFileable
    {

        private FileInfo fileInfo;
        private string _name;
        private string _fullName;


        public File( string fullname)
        {   
            FullName = fullname;
            fileInfo = new FileInfo(FullName);
            Name = fileInfo.Name;
            
        }
        
        public string Name { get => _name; private set
        {
                _name = value;
                RaisePropertyChanged("Name");
        } }

        public string FullName { get => _fullName; private set
            {
                _fullName = value;
                RaisePropertyChanged("FullName");
            } }

        public long Size { get => fileInfo.Length; }

        public DateTime LastWriteTime { get => fileInfo.LastWriteTime; }

        public string FileExtention { get => fileInfo.Extension; }
    }
}
