using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.IO.Compression;
using System.Web;
using IpLocation.Models;
using System.Configuration;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using NLog;

namespace IpLocation.App_Start
{
    public class BaseUpdater
    {
        private IBackgroundBaseUpdater _upd;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private WebClient webClient = new WebClient();

        private Thread th;

        public BaseUpdater(IBackgroundBaseUpdater upd)
        {
            _upd = upd;
            th = new Thread(
                    ()=>Update(ConfigurationManager.AppSettings["DayToUpdate"])
                );
            th.IsBackground = true;
            th.Start();
        }

        private void DownloadBase(string baseDirectory)
        {
            var curDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            try
            {
                logger.Info("Start downloading");

                foreach (var d in curDir.GetFiles("*.gz"))
                {
                    using (FileStream fileStream = d.OpenRead())
                    {
                        var gz = new GZipInputStream(fileStream);

                        var tarArchive = TarArchive.CreateInputTarArchive(gz);
                        tarArchive.ExtractContents(AppDomain.CurrentDomain.BaseDirectory);
                        tarArchive.Close();

                        logger.Info("Deleting archive file. Name:" + d.FullName );

                        gz.Close();
                        d.Delete();
                    }

                }

                foreach (var d in curDir.GetDirectories("GeoLite2*"))
                {
                    foreach (var dd in d.GetFiles(ConfigurationManager.AppSettings["UnzippedBaseName"]))
                    {
                        baseDirectory = dd.FullName;
                    }
                }

                _upd.Update(baseDirectory);

                foreach (var d in curDir.GetDirectories("GeoLite2*"))
                {
                    logger.Info("Deleting maxmindDB files from directory was named:" + d.FullName);

                    d.Delete();
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Download_Base");
            }
        }

        private void Update(String date)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["BaseName"];

            try
            {

                webClient.DownloadFileCompleted += (sender, arg) => DownloadBase(basePath);

                while (true)
                {
                    if (DateTime.Today.DayOfWeek == ((DayOfWeek)Enum.Parse(typeof(DayOfWeek), ConfigurationManager.AppSettings["DayToUpdate"])))
                    {
                        logger.Info("Start download maxmindBD");

                        webClient.DownloadFileAsync(
                                new Uri(ConfigurationManager.AppSettings["LoadBaseUrl"]),basePath);

                        logger.Info("Updating thread sleep");
                        Thread.Sleep(new TimeSpan(24, 0, 0));
                    }
                    else
                    {
                        logger.Info("Updating thread sleep ");
                        Thread.Sleep(new TimeSpan(24, 0, 0));
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Update");
            }

        }

    }

}