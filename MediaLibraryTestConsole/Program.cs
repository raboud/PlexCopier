using MediaLib;
using System;
using System.Collections.Generic;

namespace MediaLibraryTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
			string foo = Environment.GetEnvironmentVariable("FFMPEGDir", EnvironmentVariableTarget.Machine);

			string file1 = @"H:\Recordings\TV\Valor\Season 1\Valor - s01e01 - Pilot  (Ep.101).mp4";
//			string file2 = @"H:\Recordings\TV\Blue Bloods\Season 8\Blue Bloods - s08e01 - Cutting Losses.mp4";
			string file3 = @"C:\temp\foo.mp4";

			IEnumerable<Chapter> pc1 = Mpeg.GetChapters(file1);
//			IEnumerable<Chapter> pc2 = Mpeg.GetChapters(file2);
			IEnumerable<Chapter> pc3 = Mpeg.GetChapters(file3);

			string m1 = Mpeg.GetMetadata(file1);
//			string m2 = Mpeg.GetMetadata(file2);
			string m3 = Mpeg.GetMetadata(file3);


			IEnumerable<Chapter> pcl = Mpeg.GetChapters(@"C:\temp\foo.mp4");
			Console.WriteLine("Hello World!");
        }
    }
}
