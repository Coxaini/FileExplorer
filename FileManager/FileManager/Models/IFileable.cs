﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Models
{
    public interface IFileable
    {
        public string Name { get; }
        public string FullName { get; }
        public DateTime LastWriteTime { get; }
        public long Size { get; }
    }
}
