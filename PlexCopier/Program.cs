using MediaLib;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace PlexCopier
{
    class Program
    {

        static void Main(string[] args)
        {
			//			PlayonCutlist.Trim(@"H:\Recordings\TV\Blue Bloods\Season 8\Blue Bloods - s08e01 - Cutting Losses.mp4", @"C:\temp\bar.mp4");

			//Mpeg.GetMetadata(@"H:\Recordings\TV\Valor\Season 1\Valor - s01e01 - Pilot  (Ep.101).mp4", @"C:\temp\meta.txt");
			//Mpeg.GetMetadata(@"H:\Recordings\TV\Valor\Season 1\Valor - s01e01 - Pilot  (Ep.101).mp4", @"C:\temp\meta2.txt");
			//Mpeg.RemoveChapters(@"c:\temp\meta2.txt");

			//Mpeg.addMeta(@"c:\temp\foo.mp4", @"c:\temp\foo2.mp4", @"C:\temp\meta2.txt");

			//PlayonCutlist.Trim(@"H:\Recordings\TV\Valor\Season 1\Valor - s01e01 - Pilot  (Ep.101).mp4", @"C:\temp\foo.mp4");
			//Mpeg.GetMetadata(@"C:\temp\foo.mp4", @"c:\temp\meta3.txt");

			//return;

//            PlayonCutlist pcl = new PlayonCutlist(@"f:\temp\yy.mp4");
			Processor proc = new Processor();
            proc.processDirectory("");
//                PlayonCutlist.Trim(@"F:\Recordings\TV\Current\Scorpion\Season 3\Scorpion - s03e02 - It Isn't the Fall That Kills You.mp4", @"f:\temp\foo.mp4");
//                PlayonCutlist.Trim(@"F:\Recordings\TV\Current\Suits\Season 7\Suits - s07e02 - The Statue.mp4", @"f:\temp\bar.mp4");
            //            processDirectory("Game of Thrones");
            //            Trace.Listeners.Add(new TextWriterTraceListener(@"f:\temp\plex.out"));
            //            GetListing();
            //            delete();
            //            GetListing();
            //            GetListing("/usr/local/plexdata/Plex%20Media%20Server");
            //            Trace.WriteLine("foo");
            //            Trace.Flush();
        }



        //public static void delete()
        //{
        //    FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://plexserver.attlocal.net/%2fusr/local/plexdata/Plex Media Server/Game of Thrones/Game of Thrones - s02e02 - The Night Lands.mp4"));
        //    req.Credentials = new NetworkCredential("root", "s3afa182");
        //    req.Method = WebRequestMethods.Ftp.DeleteFile;
        //    FtpWebResponse response = (FtpWebResponse)req.GetResponse();
        //    Console.WriteLine("Delete status: {0}", response.StatusDescription);
        //    response.Close();
        //}

        public static void GetListing()
        {
            //FtpWebRequest req = (FtpWebRequest) FtpWebRequest.Create(new Uri("ftp://plexserver.attlocal.net/%2fusr/local/plexdata/Plex Media Server/Game of Thrones/"));
            //req.Credentials = new NetworkCredential("root", "s3afa182");
            //req.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            //FtpWebResponse response = (FtpWebResponse)req.GetResponse();
            //// The following streams are used to read the data returned from the server.
            //Stream responseStream = null;
            //StreamReader readStream = null;
            //try
            //{
            //    responseStream = response.GetResponseStream();
            //    readStream = new StreamReader(responseStream, System.Text.Encoding.UTF8);

            //    if (readStream != null)
            //    {
            //        // Display the data received from the server.
            //        Console.WriteLine(readStream.ReadToEnd());
            //    }
            //    Console.WriteLine("List status: {0}", response.StatusDescription);
            //}
            //finally
            //{
            //    if (readStream != null)
            //    {
            //        readStream.Close();
            //    }
            //    if (response != null)
            //    {
            //        response.Close();
            //    }
            //}


        }

//        public static void GetListing(FtpClient conn, string path)
//        {
//            foreach (FtpListItem item in conn.GetListing(path,
//                 FtpListOption.Modify | FtpListOption.Size | FtpListOption.Recursive))
//            {
//                switch (item.Type)
//                {
//                    case FtpFileSystemObjectType.Directory:
//                        GetListing(conn, item.FullName);
//                        break;
//                    case FtpFileSystemObjectType.File:
//                        System.Console.WriteLine(item.FullName);
//                        break;
//                    case FtpFileSystemObjectType.Link:
//                        // derefernece symbolic links
//                        if (item.LinkTarget != null)
//                        {
//                            // see the DereferenceLink() example
//                            // for more details about resolving links.
//                            item.LinkObject = conn.DereferenceLink(item);

//                            if (item.LinkObject != null)
//                            {
//                                // switch (item.LinkObject.Type)...
//                            }
//                        }
//                        break;
//                }
//            }
//        }
//        public static void GetListing2(FtpClient conn, string path)
//        {
//            // same example except automatically dereference symbolic links.
//            // see the DereferenceLink() example for more details about resolving links.
//            foreach (FtpListItem item in conn.GetListing(path,
//                FtpListOption.Modify | FtpListOption.Size | FtpListOption.DerefLinks))
//            {

//                switch (item.Type)
//                {
//                    case FtpFileSystemObjectType.Directory:
//                        break;
//                    case FtpFileSystemObjectType.File:
//                        break;
//                    case FtpFileSystemObjectType.Link:
//                        if (item.LinkObject != null)
//                        {
//                            // switch (item.LinkObject.Type)...
//                        }
//                        break;
//                }
//            }
//        }

//        public static void GetListing(string path)
//        {
//            using (FtpClient conn = new FtpClient())
//            {
//                conn.Host = "plexserver.attlocal.net";
//                conn.Credentials = new NetworkCredential("root", "s3afa182");
////                conn.SetWorkingDirectory(path);
//                GetListing(conn, path);
 
//            }
//        }


//        public static void OpenWrite()
//        {
//            using (FtpClient conn = new FtpClient())
//            {
//                conn.Host = "localhost";
//                conn.Credentials = new NetworkCredential("ftptest", "ftptest");

//                using (Stream ostream = conn.OpenWrite("/full/or/relative/path/to/file"))
//                {
//                    try
//                    {
//                        // istream.Position is incremented accordingly to the writes you perform
//                    }
//                    finally
//                    {
//                        ostream.Close();
//                    }
//                }
//            }
//        }
    }
}
