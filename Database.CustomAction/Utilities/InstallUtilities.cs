using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Deployment.WindowsInstaller;

namespace Database.CustomAction.Utilities
{
    /// <summary>
    ///     Class with utilities.
    /// </summary>
    public static class InstallUtilities
    {
        #region Fields

        /// <summary>
        ///     Current Assembly
        /// </summary>
        public static Assembly AssemblyCurrent;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Open dialog box to select the folder name.
        /// </summary>
        /// <param name="folderDefault">Default Folder .</param>
        /// <returns>folder name.</returns>
        public static string GetFolder(string folderDefault)
        {
            string sPath = folderDefault;
            var t = new Thread(() =>
            {
                var fbd = new FolderBrowserDialog();
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                fbd.Description = @"Select a folder:";
                fbd.SelectedPath = folderDefault;
                fbd.ShowNewFolderButton = true;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    sPath = fbd.SelectedPath;
                }
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            return sPath;
        }

        /// <summary>
        ///     Returns a number of characters from the right of a text..
        /// </summary>
        /// <param name="text">Text .</param>
        /// <param name="length">number of characters to be returned.</param>
        /// <returns>Returns a number of characters from the right of a text.</returns>
        public static string Right(string text, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            if (length == 0 || text.Length == 0)
            {
                return "";
            }
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            if (text.Length <= length)
            {
                return text;
            }
            return text.Substring(text.Length - length, length);
        }


        public static IPAddress GetExternalIp()
        {
            string whatIsMyIp = "http://whatismyip.com";

            string getIpRegex = @"(?<=<TITLE>.*)\d*\.\d*\.\d*\.\d*(?=</TITLE>)";

            var wc = new WebClient();

            var utf8 = new UTF8Encoding();

            string requestHtml = "";

            try
            {
                requestHtml = utf8.GetString(wc.DownloadData(whatIsMyIp));
            }

            catch (WebException we)
            {
                // do something with exception  

                Console.Write(we.ToString());
            }

            var r = new Regex(getIpRegex);

            Match m = r.Match(requestHtml);

            IPAddress externalIp = null;

            if (m.Success)
            {
                externalIp = IPAddress.Parse(m.Value);
            }

            return externalIp;
        }

        /// <summary>
        ///     Writes a message in the  installation log.
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="message">Message.</param>
        /// <param name="ex">Exception.</param>
        /// <param name="includeLine"> Indicates whether separator includes text messages at startup .</param>
        public static void WriteLogInstall(Session session, string message, Exception ex, bool includeLine)
        {
            if (session != null)
            {
                if (includeLine)
                {
                    session.Log(DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") +
                                ": ============================================================");
                }
                if (!string.IsNullOrWhiteSpace(message))
                {
                    session.Log(message);
                }
                if (ex != null)
                {
                    session.Log("Exception:");
                    session.Log(ex.Message);
                }
            }
        }

        #endregion Methods
    }

    /// <summary>
    ///     installation files for database.
    ///     Used in database scripts to create database.
    /// </summary>
    public class DataBasePathTO
    {
        /// <summary>
        ///     Variable name in script database to be replaced with the supplied path: <see cref="Path" />:
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Description to be presented to the user in the form of routes.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Path Setup database file.
        /// </summary>
        public string Path { get; set; }
    }

    /// <summary>
    ///     Feature to be installed.
    /// </summary>
    public class FeatureInstallTO
    {
        /// <summary>
        ///     Name of feature
        /// </summary>
        public string Feature { get; set; }

        /// <summary>
        ///     Title property.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Directory path where the file is installed.
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        ///     Filename installed.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     Order of presentation in the form of features
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}