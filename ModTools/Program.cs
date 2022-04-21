using ModTools.Core;
using System;

namespace ModTools
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args) => new App().Run(args);
    }
}
