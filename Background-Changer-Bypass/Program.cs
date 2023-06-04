using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Background_Changer_Bypass
{
    class Program
    {
        static ConsoleColor greenColor = ConsoleColor.Green;
        static ConsoleColor normalColor = ConsoleColor.Gray;
        static ConsoleColor redColor = ConsoleColor.Red;

        static void Main(string[] args)
        {
            string cacheFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Windows\Themes";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string cachedFileName = "CachedImage_1366_768_POS4.jpg";
            string transcodeName = "TranscodedWallpaper";

            Console.ForegroundColor = normalColor;

            string imagePath = GetPath();

            //Delete Transcoded Wallpaper
            //Delete The Cached Image
            DeleteFiles(cacheFilePath, cachedFileName, transcodeName, desktopPath);
            //Copy The New Background Image And Rename It To The Chached Image Name "CachedImage_1366_768_POS4.jpg"
            //Copy The New Background Image Again And Rename It To The Name "TranscodedWallpaper" With No Extention
            CopyNewImages(cacheFilePath, cachedFileName, transcodeName, desktopPath, imagePath);
            //Force Kill The Explorer
            //Wait Some Seconds
            //Start It Again
            Console.WriteLine("\n\r\n\rInitiating Explorer Restarter.");
            Thread.Sleep(2000);
            RestartingExplorer();
            //DONE

            Console.ReadLine();
        }

        //Getting The Image Path
        static string GetPath()
        {
            bool fileExists = false;
            Console.WriteLine("Drag And Drop Image To Console.");
            string imagePath = Console.ReadLine();
            while (!fileExists)
            {
                Console.Clear();
                if (File.Exists(imagePath)){
                    fileExists = true;
                }
                else
                {
                    Console.Write("The Image Doesn't Exist, Try Again!\n\r:");
                    imagePath = Console.ReadLine();
                }
            }
            //Prompt User And Make Sure File Exist Before Returning String!
            return imagePath;
        }

        static void DeleteFiles(string cacheFolderPath, string cacheImageName, string transcodeName, string desktopPath)
        {
            //Checking For The Images
            Console.WriteLine("Checking For Chached Image.");
            Thread.Sleep(200);
            if (File.Exists(cacheFolderPath + @"\" + cacheImageName))
            {
                Console.ForegroundColor = greenColor;
                Console.WriteLine("Cached Image Exists.");
                Console.WriteLine("File Path: " + cacheFolderPath + @"\" + cacheImageName);
                Console.ForegroundColor = normalColor;
            }

            Console.WriteLine("Checking For Transcoded File.");
            Thread.Sleep(200);
            if (File.Exists(cacheFolderPath + @"\" + transcodeName))
            {
                Console.ForegroundColor = greenColor;
                Console.WriteLine("Transcoded File Exists.");
                Console.WriteLine("File Path: " + cacheFolderPath + @"\" + transcodeName);
                Console.ForegroundColor = normalColor;
            }

            Console.Write("\n\r\n\rBacking Up Files To Desktop.");
            for (int i = 0; i < 20; i++)
            {
                Console.Write(".");
                Thread.Sleep(20);
            }
            Console.WriteLine("");

            Directory.CreateDirectory(desktopPath + @"\FileBackup");
            Console.WriteLine("Directory Made.");
            File.Copy(cacheFolderPath + @"\" + cacheImageName, desktopPath + @"\FileBackup\" + cacheImageName , true);
            File.Copy(cacheFolderPath + @"\" + transcodeName, desktopPath + @"\FileBackup\" + transcodeName , true);
            
            //Checks To See If Image Has Successfully Been Copied And If Not Exits The Program For Safety Reasons
            if (File.Exists(desktopPath + @"\FileBackup\" + cacheImageName)){
                Console.WriteLine("Cache Image Successfully Copied.");
            }
            else{
                Environment.Exit(0);
            }

            //Checks To See If Image Has Successfully Been Copied And If Not Exits The Program For Safety Reasons
            if (File.Exists(desktopPath + @"\FileBackup\" + transcodeName)){
                Console.WriteLine("Transcode File Successfully Copied.");
            }
            else{
                Environment.Exit(0);
            }


            Thread.Sleep(1000);
            Console.Write("\n\r\n\rProceeding To Delete Original Files.");

            for (int i = 0; i < 40; i++){
                Console.Write(".");
                Thread.Sleep(100);
            }


            File.Delete(cacheFolderPath + @"\" + cacheImageName);
            File.Delete(cacheFolderPath + @"\" + transcodeName);

            if(!File.Exists(cacheFolderPath + @"\" + cacheImageName) && !File.Exists(cacheFolderPath + @"\" + transcodeName)){
                Console.WriteLine("Files Successfully Deleted.");
                Thread.Sleep(500);
            }
            //Delition DONE
        }

        static void CopyNewImages(string cacheFolderPath, string cacheImageName, string transcodeName, string desktopPath , string newImagePath)
        {
            Console.WriteLine("\n\r\n\rCopying New Images.");
            File.Copy(newImagePath, cacheFolderPath + @"\" + cacheImageName, true);
            File.Copy(newImagePath, cacheFolderPath + @"\" + transcodeName, true);
            Console.ForegroundColor = greenColor;
            Console.WriteLine("Copying Done.");
            Console.ForegroundColor = normalColor;
        }

        static void RestartingExplorer()
        {
            Console.ForegroundColor = redColor;
            for (int i = 0; i < 10; i++){
            Console.WriteLine("DO NOT CLOSE WINDOW. Restarting Explorer.exe DO NOT CLOSE WINDOW.");
            }
            Console.ForegroundColor = normalColor;

            Thread.Sleep(500);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c " + "taskkill -im explorer.exe -f"; //Kills Explorer.exe

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start(); //Running The Command
            process.WaitForExit(); //Waiting For Completion
            int exitCode = process.ExitCode; //Get The Exit Code
            process.Close(); //Close The Process

            Thread.Sleep(5000);
            startInfo.Arguments ="/c" + " start expoStart.bat"; //Starts Explorer.exe Up Again

            for (int i = 0; i < 1; i++){
                process.StartInfo = startInfo;
                process.Start(); //Running The Command
                process.WaitForExit(); //Waiting For Completion
                exitCode = process.ExitCode; //Get The Exit Code
                process.Close(); //Close The Process
                Thread.Sleep(500);
            }
        }

    }
}
