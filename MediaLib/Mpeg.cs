using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MediaLib
{
	public class Mpeg
	{
		static string FFMPEG = Environment.GetEnvironmentVariable("FFMPEGDir", EnvironmentVariableTarget.Machine);
		        static readonly string DEBUG = @"";
		//		static readonly string DEBUG = @"-v quiet ";
		public static string GetMetadata(string input)
		{
			string meta = Path.GetTempFileName();
			GetMetadata(input, meta);
			return meta;
		}

		public static void GetMetadata(string input, string meta)
		{
			ProcessStartInfo psiFfmpeg = new ProcessStartInfo($"{FFMPEG}ffmpeg.exe")
			{
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = false,
				Arguments = $"{DEBUG}-y -i \"{input}\" -f ffmetadata {meta}"
			};
			Process pFfmpeg = Process.Start(psiFfmpeg);
			pFfmpeg.WaitForExit();
			pFfmpeg.Close();

		}

		public static void combine(List<string> files, string mp4, string meta, double duration)
		{
			string concat = Path.GetTempFileName();
			using (TextWriter tw = File.CreateText(concat))
			{
				foreach (string file in files)
				{
					tw.WriteLine($"file '{file}'");
				}
			}

			ProcessStartInfo psiFfmpeg = new ProcessStartInfo($"{FFMPEG}ffmpeg.exe")
			{
				RedirectStandardOutput = true,
//				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = false,
				Arguments = $"{DEBUG}-y -f concat -safe 0 -i \"{concat}\" -f ffmetadata -i {meta} -codec copy -map_metadata 1 -t {duration} \"{mp4}\""
//				Arguments = $"{DEBUG}-y -f concat -safe 0 -i \"{concat}\" -codec copy -t {duration} \"{mp4}\""
			};
			Process pFfmpeg = Process.Start(psiFfmpeg);
			pFfmpeg.WaitForExit();
			pFfmpeg.Close();
			File.Delete(concat);

		}

		public static void RemoveChapters(string meta)
		{
			List<string> m2 = File.ReadAllLines(meta, Encoding.UTF8).ToList();
			int has = m2.FindIndex(s => s == "HasChapters=1");
			if (has != -1)
			{
				m2[has] = "HasChapters=0";
				int First = m2.FindIndex(0, (s) => s == "[CHAPTER]");
				if (First != -1)
				{
					int Last = m2.FindLastIndex((s) => s == "[CHAPTER]");
					Last = m2.FindIndex(Last + 1, (s) => s.StartsWith("["));
					if (Last == -1)
					{
						Last = m2.Count;
					}
					m2.RemoveRange(First, Last - First);
				}
				File.WriteAllText(meta, string.Join("\n", m2) + "\n");
			}
		}

		public static void addMeta(string input, string mp4, string meta)
		{
			ProcessStartInfo psiFfmpeg = new ProcessStartInfo($"{FFMPEG}ffmpeg.exe")
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = false,
				Arguments = $"{DEBUG}-y -f ffmetadata -i {meta} -i \"{input}\" -map_metadata 0 -codec copy \"{mp4}\""
			};
			Process pFfmpeg = Process.Start(psiFfmpeg);
			string output = pFfmpeg.StandardError.ReadToEnd();
			pFfmpeg.WaitForExit();
			pFfmpeg.Close();

		}

		public static void Trim2(string input, string mp4, Cutlist Cuts)
		{
			ProcessStartInfo psiFfmpeg = new ProcessStartInfo($"{FFMPEG}ffmpeg.exe")
			{
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = false
				//                ,
				//                Arguments = $"-i \"{input}\" {buildComplexFilter(Cuts)} f:\\temp\\cut.mp4"
			};

			if (Cuts.Count == 0)
			{
				//                psiFfmpeg.Arguments = $"-ss 6 - i \"{input}\" -vcodec copy -acodec copy -to {inputFile.Metadata.Duration.Subtract(TimeSpan.FromSeconds(13))} \"{mp4}\"";
			}
			else
			{
				double dur = Cuts.Sum(c => c.End - c.Start);
				psiFfmpeg.Arguments = $"-i \"{input}\" {buildComplexFilter(Cuts)} -c:v copy -c:a copy -t {dur} {mp4}";
			}

			Process pFfmpeg = Process.Start(psiFfmpeg);
			pFfmpeg.WaitForExit();
			pFfmpeg.Close();
		}

		public static void Trim(string input, string mp4, Cut cut)
		{
			ProcessStartInfo psiFfmpeg = new ProcessStartInfo($"{FFMPEG}ffmpeg.exe")
			{
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = false,
				Arguments = $"{DEBUG}-y -ss {cut.Start} -i \"{input}\" -t {cut.End - cut.Start} -codec copy \"{mp4}\""
			};
			Process pFfmpeg = Process.Start(psiFfmpeg);
			pFfmpeg.WaitForExit();
			pFfmpeg.Close();
		}

		private static string buildComplexFilter(Cutlist Cuts)
		{
			char index = 'a';
			string filter = $"-filter_complex \"";
			string concat = "";
			foreach (Cut c in Cuts)
			{
				filter += $"[0:v]trim=start={c.Start}:end={c.End - .1},setpts=PTS-STARTPTS[{index}v];";
				filter += $"[0:a]atrim=start={c.Start}:end={c.End - .1},asetpts=PTS-STARTPTS[{index}a];";
				concat += $"[{index}v][{index}a]";
				index++;
			}
			filter += $"{concat}concat=n={Cuts.Count}:v=1:a=1 [v] [a]\" -map \"[v]\" -map \"[a]\"";
			return filter;
		}

		public static double GetDuration(string input)
		{
			ProcessStartInfo psiProbe = new ProcessStartInfo($"{FFMPEG}ffprobe.exe")
			{
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = false,
				Arguments = $"{DEBUG}-show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{input}\""
			};

			Process pProbe = Process.Start(psiProbe);
			pProbe.WaitForExit();
			string duration = pProbe.StandardOutput.ReadToEnd();
			pProbe.Close();
			return double.Parse(duration);
		}

		public static IEnumerable<Chapter> GetChapters(string input)
		{
			Cutlist Cuts = new Cutlist();

			ProcessStartInfo psiProbe = new ProcessStartInfo($"{FFMPEG}ffprobe.exe")
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = false,
				Arguments = $"-show_chapters -print_format xml \"{input}\""
//				Arguments = $"{DEBUG}-show_chapters -print_format xml \"{input}\""
			};

			Process pProbe = Process.Start(psiProbe);

			Chapters data;
			XmlSerializer ser = new XmlSerializer(typeof(Chapters));

//			string output = pProbe.StandardOutput.ReadToEnd();
			string error = pProbe.StandardError.ReadToEnd();

			pProbe.WaitForExit();
			data = (Chapters)ser.Deserialize(pProbe.StandardOutput);

			pProbe.Close();
			return data.chapters;
		}


	}
}
