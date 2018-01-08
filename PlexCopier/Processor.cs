using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace PlexCopier
{
    public class Processor
    {
        static readonly string baseDir = @"H:\Recordings\TV\Valor\";  // Game of Thrones
        static readonly string baseDest = @"Z:\TV\Valor\";  // Game of Thrones

        ConcurrentQueue<Work> trim = new ConcurrentQueue<Work>();

        public void processDirectory(string path)
        {
            this.process(path);

            FileSystemWatcher fsw = new FileSystemWatcher(Path.Combine(baseDir, path), "*.mp4");
            fsw.IncludeSubdirectories = true;
            fsw.NotifyFilter = 
                NotifyFilters.FileName | 
                NotifyFilters.DirectoryName | 
                NotifyFilters.LastWrite |
                NotifyFilters.CreationTime ;
            fsw.Created += Fsw_Created;
            fsw.EnableRaisingEvents = true;
            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Run(() => worker()));

            Task.WaitAll(tasks.ToArray());
        }

        private void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            string p1 = Path.GetFullPath(baseDir);
            string p2 = Path.GetFullPath(e.FullPath);

            if (p2.StartsWith(p1))
            {
                p2 = p2.Substring(p1.Length);

                string dest = Path.Combine(baseDest, p2);
                if (!File.Exists(dest))
                {
                    this.trim.Enqueue(new Work(e.FullPath, dest));
                }
                else
                {
                    System.Console.WriteLine("Skipping {0}", e.FullPath);
                }
            }
            else
            {
                System.Console.WriteLine("Error {0}", e.FullPath);
            }
        }

        public static bool WaitForFileReady(String sFilename, int? timeout)
        {
            bool bReturn = false;
            DateTime to = DateTime.MaxValue;
            if (timeout != null)
            {
                to = DateTime.Now.AddMilliseconds(timeout.Value);
                
            }
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            while (DateTime.Now < to)
            {
                try
                {
                    using (FileStream inputStream = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        if (inputStream.Length > 0)
                        {
                            bReturn = true;
                            break;
                        }
                    }
                    Task.Delay(100).Wait();
                }
                catch (Exception)
                {
                }
            }
            return bReturn;
        }

        public void process(string path)
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(baseDir, path));
            foreach (FileInfo fi in di.GetFiles())
            {
                string dest = Path.Combine(baseDest, path, fi.Name);
                if (!File.Exists(dest))
                {
                    this.trim.Enqueue(new Work(fi.FullName, dest));
                }
                else
                {
                    System.Console.WriteLine("Skipping {0}", dest);
                }

                //                Uri uri = new Uri(Path.Combine(baseUri, path, fi.Name));
            }
            foreach (DirectoryInfo di2 in di.GetDirectories())
            {
//                string dir = Path.Combine(baseDest, path, di2.Name);
//                Directory.CreateDirectory(dir);
//                CreateDir(dir);
                process(Path.Combine(path, di2.Name));
            }

        }

        public void worker()
        {
            while (true)
            {
                Work work = null;
                if (trim.TryDequeue(out work))
                {
                    try
                    {
                        if (WaitForFileReady(work.source, 2000))
                        {
                            System.Console.WriteLine("Transcoding {0}", work.destination);
                            PlayonCutlist.Trim(work.source, work.destination);
                        }
                        else
                        {
                            trim.Enqueue(work);
                        }
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Exception {0}", work.destination);
                    }
                }
                else
                {
                    Task.Delay(100).Wait();
                }


            }

        }
        public static void UploadFile(Uri uri, string fileName)
        {
            FileStream fs = null;
            Stream requestStream = null;
            try
            {
                Console.WriteLine("Upload File Started - {0}", fileName);

                FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(uri);
                req.Credentials = new NetworkCredential("root", "s3afa182");
                req.Method = WebRequestMethods.Ftp.UploadFile;
                // Copy the contents of the file to the request stream.
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                req.ContentLength = fs.Length;


                using (requestStream = req.GetRequestStream())
                {
                    fs.CopyTo(requestStream);
                }
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                response.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Upload File Exception, {0}", e.Message);
            }
            if (fs != null)
            {
                fs.Close();
                fs.Dispose();
            }
        }

    }
}
