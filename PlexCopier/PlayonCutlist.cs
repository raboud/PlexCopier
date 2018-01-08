using MediaLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PlexCopier
{
    public class PlayonCutlist
    {
        public static Cutlist GetCuts(string input)
        {
            Cutlist Cuts = new Cutlist();
			IEnumerable<Chapter> chapters = Mpeg.GetChapters(input);

            List<Chapter> ads = chapters.Where(c => c.tag.value == "Video").ToList();

            foreach (Chapter chapter in ads)
            {
                Cut cut = new Cut() { Start = chapter.start_time + 2, End = chapter.end_time - 2 };
                if (Cuts.Count == 0)
                    cut.Start += 4;
                if (chapter == ads.Last())
                    cut.End -= 5;
                if (cut.End > cut.Start)
                {
                    Cuts.Add(cut);
                }
            }
            return Cuts;
        }

        public static void TrimN(string inputOrg, string output)
        {
            string input = Path.Combine(Path.GetTempPath(), Path.GetFileName(inputOrg));
            if (File.Exists(input))
            {
                File.Delete(input);
            }
            File.Copy(inputOrg, input);
            string mp4 = GetTempMP4Name();
            Trim(input, mp4);
            if (File.Exists(output))
            {
                File.Delete(output);
            }
            string outDir = Path.GetDirectoryName(output);
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }
            File.Move(mp4, output);
            File.Delete(input);

        }

		public static void Trim(string input, string output)
        {
            string outDir = Path.GetDirectoryName(output);
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }
            Cutlist Cuts = GetCuts(input);
			//if (Cuts.Count ==0)
			//{
			//	double duration = Mpeg.GetDuration(input);
			//	Cuts.Add(new Cut() { Start = 6, End = duration - 7 });
			//}
			if (Cuts.Count > 0)
            {
                string meta = Mpeg.GetMetadata(input);
				Mpeg.RemoveChapters(meta);
                List<string> files = new List<string>();
                foreach (Cut cut in Cuts)
                {
                    string mp4 = GetTempMP4Name();
					Mpeg.Trim(input, mp4, cut);
                    files.Add(mp4);
                }
				string temp = GetTempMP4Name();
				Mpeg.combine(files, output, meta, Cuts.Sum(c => c.End - c.Start));
//				Mpeg.addMeta(temp, output, meta);
                foreach(string file in files)
                {
                    File.Delete(file);
                }
                File.Delete(meta);
				File.Delete(temp);
            }
            else
            {
                double duration = Mpeg.GetDuration(input);
                Cut cut = new Cut() { Start = 6, End = duration - 7 };
                Mpeg.Trim(input, output, cut);
            }

        }


        public static string GetTempMP4Name()
        {
            string temptemp = Path.GetTempFileName();
            string temp = Path.ChangeExtension(temptemp, ".mp4");
            File.Delete(temptemp);
            return temp;
        }
    }



}
