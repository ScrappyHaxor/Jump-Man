using System;

using JumpMan.Core;

namespace JumpMan
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args) => new App().Run(args);
    }
}
