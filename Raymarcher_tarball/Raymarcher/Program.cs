using System;

namespace Raymarcher
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: raymarcher.exe <input> <resX> <resY> <output>");
                return 1;
            }
            
            int resx;
            int resy;
            try
            {
                resx = Int32.Parse(args[1]);
                resy = Int32.Parse(args[2]);
            }
            catch (Exception)
            {
                Console.WriteLine("Usage: raymarcher.exe <input> <resX> <resY> <output>");
                return 1;
            }
            Scene s = new Scene(args[0]);
            s.RenderScene(args[3], resx, resy);
            return 0;
        }
    }
}