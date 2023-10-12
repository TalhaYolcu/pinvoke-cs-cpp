using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    //ensures order is matched
    [StructLayout(LayoutKind.Sequential)]
    public struct Shoe
    {
        public int id;

        [MarshalAs(UnmanagedType.BStr)]
        public string color;

        public double size;

        [MarshalAs(UnmanagedType.BStr)]
        public string brand;
    }
}
