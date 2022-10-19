using Prism.Mvvm;
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

        public File( string fullname)
        {   
            FullName = fullname;
            fileInfo = new FileInfo(FullName);
            Name = fileInfo.Name;
            
        }

        public string Name { get; set; }

        public string FullName { get; set; }

        public long Size { get => fileInfo.Length; }

        public DateTime LastWriteTime { get => fileInfo.LastWriteTime; }
    }
}
