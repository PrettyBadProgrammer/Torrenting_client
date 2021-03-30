using System;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace SearchWebsite
{
    public class Generate_torrent
    {
        public static string generate_torrent_F()
        {
            /*Console.WriteLine("Hello World!");
            string Name = HttpUtility.UrlEncode("IEach hello");
            Console.WriteLine(Name);*/
            Console.WriteLine("Enter title to search:");
            string search = Console.ReadLine();
            search ??= "+";
            if (search == string.Empty)
            {
                search = "+";
            }
            //Console.WriteLine(search);
            //List<string> 
            //Console.WriteLine("");
            string data = string.Empty;
            string urlAddress = "https://nyaa.si/?f=0&c=0_0&q=" + search;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }
            //Console.WriteLine(data);
            Regex reg = new Regex(@"a href=\042\/view\/(\d+)\042 title=\042(.*)\042>");
            MatchCollection hello = reg.Matches(data);
            //Console.WriteLine(hello.Count);

            Regex Torrent_FS = new Regex(@"(\d+)\.?(\d*) (GiB|MiB)");
            MatchCollection Torrent_FS_Match = Torrent_FS.Matches(data);
            int x = 0;

            List<List<string>> matrix = new List<List<string>>(); //Creates new nested List


            //Console.WriteLine(matrix[0][1]);

            int z = 0;

            foreach (Match term in hello)
            {
                matrix.Add(new List<string>());
                int m = 0;
                foreach (Match newterm in hello)
                {
                    if (m == z)
                    {

                        matrix[z].Add(Regex.Replace(Convert.ToString(newterm.Groups[2]), @"[^\u0000-\u007F]+", string.Empty));
                        matrix[z].Add(Convert.ToString(newterm.Groups[1]));
                        int k = 0;
                        foreach (Match filesize in Torrent_FS_Match)
                        {
                            if (k == z)
                            {
                                matrix[z].Add(Convert.ToString(filesize.Groups[0]));
                               //Console.WriteLine(filesize.Groups[0]);
                                
                               //Console.WriteLine(newterm.Groups[2]);

                                
                               //Console.WriteLine(newterm.Groups[1]);
                                //Console.WriteLine(k);
                            }
                            k++;
                           
                        }
                    }
                    m++;
                    
                }
                z++;
                
            }

           


            //Console.WriteLine(matrix[1][1]);


            for (int j = 0; j < matrix.Count(); j++)
            {


                Console.WriteLine("{0}. {1}", j + 1, matrix[j][0]);


            }

            Console.WriteLine("Select which file you want to download:");
            string which_file = Console.ReadLine();

            Console.WriteLine(matrix[Convert.ToInt32(which_file)][0]);
            Console.WriteLine(matrix[Convert.ToInt32(which_file)][1]);
            Console.WriteLine(matrix[Convert.ToInt32(which_file)][2]);
            string file_code = (matrix[Convert.ToInt32(which_file)][1]);
            string file_name = (matrix[Convert.ToInt32(which_file)][0]);
            string Save_Path = @"Enter path to save torrent" + file_code + ".torrent";
           
            DownloadTorrent.DownloadFile("https://nyaa.si/download/" + file_code + ".torrent", Save_Path, 1000);
            return Save_Path;
        }


    }
}
