//using MediaToolkit;
//using MediaToolkit.Model;
//using MediaToolkit.Options;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PlexCopier
//{
//    public class Trimmer
//    {
//        public static void Trim(string input, string output)
//        {
//            string mp4 = GetTempMP4Name();

//            using (var engine = new Engine())
//            {
//                var inputFile = new MediaFile { Filename = input };
//                engine.GetMetadata(inputFile);
//                string command = $"-ss 6 -i \"{input}\" -vcodec copy -acodec copy -to {inputFile.Metadata.Duration.Subtract(TimeSpan.FromSeconds(13))} \"{mp4}\"";
//                engine.CustomCommand(command);
//            }

//            if (!Directory.Exists(Path.GetDirectoryName(output)))
//                Directory.CreateDirectory(Path.GetDirectoryName(output));
//            if (File.Exists(output))
//                File.Delete(output);
//            File.Move(mp4, output);
//        }
//        public static string TrimCut(string input, Cut cut)
//        {
//            string temptemp = Path.GetTempFileName();
//            string temp = Path.ChangeExtension(temptemp, ".mp4");
//            File.Delete(temptemp);

//            using (var engine = new Engine())
//            {
//                var inputFile = new MediaFile { Filename = input };
//                //                engine.GetMetadata(inputFile);
//                string command = $"-ss {cut.Start} -i \"{input}\" -vcodec copy -acodec copy -to {cut.End - cut.Start} \"{temp}\"";
//                engine.CustomCommand(command);
//            }

//            return temp;
//        }

//        public static void Trim(string input, string output, Cutlist cuts)
//        {
//            if (cuts == null || cuts.Count == 0)
//            {
//                Trim(input, output);
//                return;
//            }
//            List<string> files = new List<string>();
//            string temp = Path.GetTempFileName();
//            files.Add(temp);
//            foreach (Cut cut in cuts)
//            {
//                string file = TrimCut(input, cut);
//                files.Add(file);
//                using (StreamWriter sw = File.AppendText(temp))
//                {
//                    sw.WriteLine($"file '{file}'");
//                }
//            }

//            string mp4 = GetTempMP4Name();
//            //{
//            //    ProcessStartInfo psi = new ProcessStartInfo(@"C:\Program Files\ffmpeg\bin\ffmpeg.exe")
//            //    {
//            //        RedirectStandardOutput = false,
//            //        UseShellExecute = false,
//            //        CreateNoWindow = false,
//            //        Arguments = string.Format("-f concat -safe 0 -i \"{0}\" -c copy \"{1}\"", temp, mp4)
//            //    };

//            //    Process p = Process.Start(psi);
//            //    p.WaitForExit();
//            //}

//            using (var engine = new Engine())
//            {
//                string command = $"-f concat -safe 0 -i \"{temp}\" -c copy \"{mp4}\"";
//                engine.CustomCommand(command);
//            }
//            if (!Directory.Exists(Path.GetDirectoryName(output)))
//                Directory.CreateDirectory(Path.GetDirectoryName(output));
//            if (File.Exists(output))
//                File.Delete(output);
//            foreach(string file in files)
//            {
//                File.Delete(file);
//            }
//            File.Move(mp4, output);
//        }

//        public static string GetTempMP4Name()
//        {
//            string temptemp = Path.GetTempFileName();
//            string temp = Path.ChangeExtension(temptemp, ".mp4");
//            File.Delete(temptemp);
//            return temp;
//        }
//    }
//}
