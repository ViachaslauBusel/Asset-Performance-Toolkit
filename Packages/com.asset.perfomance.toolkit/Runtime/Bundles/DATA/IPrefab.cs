﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bundles.DATA
{
    public interface IPrefab
    {
         string GUID { get; }
         string Path { get; }
         string Bundle { get; }
    }
}
