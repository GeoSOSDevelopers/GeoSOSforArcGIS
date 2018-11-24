using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoSOS.CommonLibrary.Struct
{
    public struct StructACOParameters
    {
        public int Q;
        public int Alpha;
        public int Beta;
        public float Rho;
        public float WeightSuitable;
        public float WeightCompact;
        public int AntCount;
        public int InterationCount;
        public int MapRefreshInterval;
        public string OutputFolder;
        public bool IsOutput;
    }
}
