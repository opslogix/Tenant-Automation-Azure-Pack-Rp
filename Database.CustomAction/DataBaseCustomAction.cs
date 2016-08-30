/* ========================================================================== 
Links: 
 * 
Debug: http://blog.torresdal.net/CommentView , guid, BFEBE347 - AD82 - 4C76 - A96E - 1C22AA39EFC9.aspx
SearchAndReplace : http://dotnetbyexample.blogspot.com/2010/11/wix-configurable-search-replace-custom.html
Pass properties: http://www.progtown.com/topic242296-wixmsi-customaction-impersonation-and-obtaining-property.html
Web Installer : http://blogs.planetsoftware.com.au/paul/archive/2011/02/26/creating-a-web-application-installer-with-wix-3.5-and-visual-once-more.aspx
MSI Tabas . Http://msdn.microsoft.com/en-us/library/windows/desktop/aa368295 ( v = vs.85 ) aspx
Serialize Json.Net : http://json.codeplex.com/
Progress bar : http://taocoyote.wordpress.com/2009/05/19/adding-managed-custom-actions-to-the-progressbar/
Execution sequence : http://code.dblock.org/msi-property-patterns-upgrading-firstinstall-and-maintenance
Run the uninstall actions : http://stackoverflow.com/questions/320921/how-to-add-a-wix-custom-action-that-happens-only-on-uninstall-via-msi
Conditional Expressions : http://msdn.microsoft.com/en-us/library/aa368012 % 28v = VS.85 % 29.aspx
WixUI_InstallMode : http://neilsleightholm.blogspot.com/2008/08/customised-uis-for-wix.html

========================================================================== */

/* ========================================================================== 
VARIABLES TO INSTALL :
 * 
$ ( DATABASE_MAILBOX ) : mailbox mail , used to configure the mail service .
$ ( DATABASE_MAILIP ) : IP address of the mail server used to configure the mail service .
$ ( DATABASE_MAILPROFILENAME ) : mail profile name , used to configure the mail service , recommended the same instance name .
$ ( DATABASE_OPERATORMAILBOX ) Mailboxes to receive notifications, for the operator : " OperatorWixDataBase " .
$ ( DATABASE_PROXYPASSWORD ) : User password used to crar Windos proxy account.
$ ( DATABASE_PROXYWINDOWSUSER ) : Windows user account used to crar proxy .
$ ( DATABASE_NAME ): Name of the database

========================================================================== */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using Microsoft.Deployment.WindowsInstaller;
using Database.CustomAction.UI;
using Database.CustomAction.Utilities;
using View = Microsoft.Deployment.WindowsInstaller.View;

namespace Database.CustomAction
{
    public class DataBaseCustomAction
    {
        #region Fields

        /// <summary>
        ///     SQl Connection to SQL Server.
        /// </summary>
        private static readonly SqlConnection m_sqlConectionMain = new SqlConnection();

        #endregion Fields

        #region Custom actions

        /// <summary>
        ///     Executes scripts in  database server and SQL Server database.
        ///     Declare this method in a CustomAction to Execute = "deferred" in InstallExecuteSequence with After = "..."
        ///     After calling the CustomAction preparing Session.CustomActionData collection of values.
        ///     The collection of session values [ propertyName ] is not available in CustomAction to Execute = "deferred "
        ///     Therefore must establish collection Session.CustomActionData values [ propertyName ]
        ///     In a CustomAction to Execute = "immediate" .
        /// </Summary>
        /// <param Name="session"> Windows Installer Session . </param>
        /// Returns
        /// <returns> execution status . </returns>
        [CustomAction]
        public static ActionResult ExecuteSQLScripts(Session session)
        {
            var data = new CustomActionData();

            try
            {
                //  Active message only for debugging developing
                //  MessageBox.Show("SwhowPathInstall: For debugging , the Debug menu, ' Attach to process ' ate processes : msiexec.exe y rundll32.exe.",
                //    "Depurar: SwhowPathInstall", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //bool isCustomActionData = false;
                //List<DataBasePathTO> listPaths;
                List<FeatureInstallTO> listFeactureScripts;
                var listFeactureNames = new List<string>();

                InstallUtilities.WriteLogInstall(session, "Initialised SwhowPathInstall ...", null, true);

                if (session == null)
                {
                    throw new ArgumentNullException("session");
                }

                // Setup Routes taken from CustomTable Id = " TABLE_DATABASE_PATHS " and set default route
                //listPaths = GetCustomTableDataBasePaths(session, isCustomActionData);

                // Open the user form to change installation paths database .
                /*
                var frmA = new frmPaths(listPaths);
                if (frmA.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Error");
                    throw new InstallerException("Configuración de rutas de instalación no realizada");
                }
                */
                // / Prepare CustomActionData for deferred CustomAction 


                // Update CustomTable Id = " TABLE_DATABASE_PATHS " with modified routes
                // UpdateCustomTableDataBasePaths (session , listPaths , isCustomActionData );

                // Prepare list of paths to be sent to CustomActionData for deferred CustomAction
                //data.AddObject("DATABASE_PATHS", listPaths);

                // Add the route list as a session property
                // Data.Add ( " DATABASE_PATHS " JsonConvert.SerializeObject ( listPaths ) ) ;

                // To retrieve the serialized property
                // List <DataBasePathTO> JsonConvert.DeserializeObject listPaths = < List <DataBasePathTO> > ( session.CustomActionData [" DATABASE_PATHS "] );

                // Prepare properties to be sent to CustomActionData for deferred CustomAction
                SetCustomActionData(session, "INSTALLLOCATION", data);
                SetCustomActionData(session, "DATABASE_SERVERNAME", data);
                SetCustomActionData(session, "DATABASE_NAME", data);
                SetCustomActionData(session, "DATABASE_WINDOWSAUTHENTICATION", data);
                SetCustomActionData(session, "DATABASE_AUTHENTICATEDATABASE", data);
                SetCustomActionData(session, "DATABASE_EXECUTESCRIPTS", data);
                SetCustomActionData(session, "DATABASE_USERNAME", data);
                SetCustomActionData(session, "DATABASE_PASSWORD", data);
                SetCustomActionData(session, "DATABASE_PROXYPASSWORD", data);
                SetCustomActionData(session, "DATABASE_MAILPROFILENAME", data);
                SetCustomActionData(session, "DATABASE_MAILBOX", data);
                SetCustomActionData(session, "DATABASE_MAILIP", data);
                SetCustomActionData(session, "DATABASE_OPERATORNAMENAME", data);
                SetCustomActionData(session, "DATABASE_OPERATORMAILBOX", data);
                SetCustomActionData(session, "DATABASE_PROXYWINDOWSUSER", data);

                // / Database Scripts to be installed, taken from the MSI  Feature using sql files * .
                foreach (FeatureInfo fi in session.Features)
                {

                    if (fi.RequestState == InstallState.Local || fi.RequestState == InstallState.Source ||
                        fi.RequestState == InstallState.Default)
                    {
                        listFeactureNames.Add(fi.Name);
                        InstallUtilities.WriteLogInstall(session,
                            "FEATURE fi.Name: " + fi.Name + ", fi.CurrentState: " + fi.CurrentState +
                            ", fi.RequestState:" + fi.RequestState, null, false);
                    }
                }
                listFeactureScripts = GetFeatureScriptDataBase(session, listFeactureNames);
                data.AddObject("DATABASE_FEACTURESCRIPTS", listFeactureScripts);
                // Add all the properties in a single variable
                //session["CUSTOMACTIONDATA_PROPERTIES"] = data.ToString();

                // Schedule deferred actions 
                //session.DoAction("CA_DataBaseExecuteScripts", data);
                //return ActionResult.Success;
            }
            catch (Exception ex)
            {
                InstallUtilities.WriteLogInstall(session, "Exception to establish installation paths for database.", ex, true);
                return ActionResult.Failure;
            }

            try
            {
                // Active message only for debugging developing
                // MessageBox.Show ( " ExecuteSQLScripts : To debug in VS Net, on the Debug menu, ' Attach to process '   
                // Msiexec.exe and rundll32.exe " DEBUG : ExecuteSQLScripts " , MessageBoxButtons.OK , MessageBoxIcon.Information );
                MessageResult iResult;
                string sInstallLocation, sServer, sDatabase, sMessage;
                int iTotalTicks, iTickIncrement = 1;
                bool isCustomActionData = true;
                
                if (session == null)
                {
                    throw new ArgumentNullException("session");
                }
               // sInstallLocation = GetSessionProperty(session, "INSTALLLOCATION", isCustomActionData);
                sServer = GetSessionProperty(session, "DATABASE_SERVERNAME", false);
                //PRESET TO STANDARD DATABASE NAME
                //sDatabase = GetSessionProperty(session, "DATABASE_NAME", isCustomActionData);

                //List<DataBasePathTO> listPaths = GetCustomTableDataBasePaths(session, isCustomActionData);

                var listF = data.GetObject<List<FeatureInstallTO>>("DATABASE_FEACTURESCRIPTS");
                iTotalTicks = listF.Count;

                InstallUtilities.WriteLogInstall(session, "Initialised ExecuteSQLScripts ...", null, true);
                iResult = InstallProgress.ResetProgress(session, iTotalTicks);
                if (iResult == MessageResult.Cancel)
                {
                    return ActionResult.UserExit;
                }

                sMessage = "Executed Script on SQL Server: " + sServer;
                iResult = InstallProgress.DisplayStatusActionStart(session, sMessage, sMessage, "[1] / [2]: [3]");
                if (iResult == MessageResult.Cancel)
                {
                    return ActionResult.UserExit;
                }

                iResult = InstallProgress.NumberOfTicksPerActionData(session, iTickIncrement, true);
                if (iResult == MessageResult.Cancel)
                {
                    return ActionResult.UserExit;
                }
                
                for (int i = 0; i < listF.Count; i++)
                {
                    ExecuteSQLScript(session, listF[i].DirectoryPath, listF[i].FileName, isCustomActionData);
                    iResult = InstallProgress.DisplayActionData3(session, (i + 1).ToString(), iTotalTicks.ToString(),
                    InstallUtilities.Right(Path.Combine(listF[i].DirectoryPath, listF[i].FileName), 200));
                    if (iResult == MessageResult.Cancel)
                    {
                        return ActionResult.UserExit;
                    }
                }
            }
            catch (Exception ex)
            {
                InstallUtilities.WriteLogInstall(session, @"Exception executing script against the database", ex, true);
 
                MessageBox.Show(ex.Message, @"Error while executing scripts", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ActionResult.Failure;
            }

            return ActionResult.Success;
        }

        /// <summary>
        ///     Set the default value of Session [" DATABASE_MAILIP " ] property if it is blank .
        ///     Declare this method in a CustomAction to Execute = " immediate" and invoke it from a button with Event = "
        ///     DoAction"
        /// </summary>
        /// <param Name="session"> Windows Installer Session . </param>
        /// Returns
        /// <returns> execution status . </returns>
        /// returns>
        [CustomAction]
        public static ActionResult SetDefaultIPAdress(Session session)
        {
            try
            {
                string sHostName, sIP, sIPCurrent;

                sIPCurrent = GetSessionProperty(session, "DATABASE_MAILIP", false);
                if (string.IsNullOrWhiteSpace(sIPCurrent))
                {
                    sHostName = Dns.GetHostName();
                    sIP = Dns.GetHostEntry(sHostName).AddressList[0].ToString();
                    SetSessionProperty(session, "DATABASE_MAILIP", sIP);
                }

                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                InstallUtilities.WriteLogInstall(session, "Exception setting Default IP", ex, true);
                return ActionResult.Failure;
            }
        }


        [CustomAction]
        public static ActionResult SwhowPathInstall(Session session)
        {
            try
            {
                //  Active message only for debugging developing
                //  MessageBox.Show("SwhowPathInstall: For debugging , the Debug menu, ' Attach to process ' ate processes : msiexec.exe y rundll32.exe.",
                //    "Depurar: SwhowPathInstall", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //bool isCustomActionData = false;
                //List<DataBasePathTO> listPaths;
                List<FeatureInstallTO> listFeactureScripts;
                var listFeactureNames = new List<string>();

                InstallUtilities.WriteLogInstall(session, "Initialised SwhowPathInstall ...", null, true);

                if (session == null)
                {
                    throw new ArgumentNullException("session");
                }

                // Setup Routes taken from CustomTable Id = " TABLE_DATABASE_PATHS " and set default route
                //listPaths = GetCustomTableDataBasePaths(session, isCustomActionData);
                
                // Open the user form to change installation paths database .
                /*
                var frmA = new frmPaths(listPaths);
                if (frmA.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Error");
                    throw new InstallerException("Configuración de rutas de instalación no realizada");
                }
                */
                // / Prepare CustomActionData for deferred CustomAction 
                var data = new CustomActionData();

                // Update CustomTable Id = " TABLE_DATABASE_PATHS " with modified routes
                // UpdateCustomTableDataBasePaths (session , listPaths , isCustomActionData );

                // Prepare list of paths to be sent to CustomActionData for deferred CustomAction
                //data.AddObject("DATABASE_PATHS", listPaths);

                // Add the route list as a session property
                // Data.Add ( " DATABASE_PATHS " JsonConvert.SerializeObject ( listPaths ) ) ;

                // To retrieve the serialized property
                // List <DataBasePathTO> JsonConvert.DeserializeObject listPaths = < List <DataBasePathTO> > ( session.CustomActionData [" DATABASE_PATHS "] );

                // Prepare properties to be sent to CustomActionData for deferred CustomAction
                SetCustomActionData(session, "INSTALLLOCATION", data);
                SetCustomActionData(session, "DATABASE_SERVERNAME", data);
                SetCustomActionData(session, "DATABASE_NAME", data);
                SetCustomActionData(session, "DATABASE_WINDOWSAUTHENTICATION", data);
                SetCustomActionData(session, "DATABASE_AUTHENTICATEDATABASE", data);
                SetCustomActionData(session, "DATABASE_EXECUTESCRIPTS", data);
                SetCustomActionData(session, "DATABASE_USERNAME", data);
                SetCustomActionData(session, "DATABASE_PASSWORD", data);
                SetCustomActionData(session, "DATABASE_PROXYPASSWORD", data);
                SetCustomActionData(session, "DATABASE_MAILPROFILENAME", data);
                SetCustomActionData(session, "DATABASE_MAILBOX", data);
                SetCustomActionData(session, "DATABASE_MAILIP", data);
                SetCustomActionData(session, "DATABASE_OPERATORNAMENAME", data);
                SetCustomActionData(session, "DATABASE_OPERATORMAILBOX", data);
                SetCustomActionData(session, "DATABASE_PROXYWINDOWSUSER", data);

                // / Database Scripts to be installed, taken from the MSI  Feature using sql files * .
                
                foreach (FeatureInfo fi in session.Features)
                {

                    if (fi.RequestState == InstallState.Local || fi.RequestState == InstallState.Source ||
                        fi.RequestState == InstallState.Default)
                    {
                        listFeactureNames.Add(fi.Name);
                        InstallUtilities.WriteLogInstall(session,
                            "FEATURE fi.Name: " + fi.Name + ", fi.CurrentState: " + fi.CurrentState +
                            ", fi.RequestState:" + fi.RequestState, null, false);
                    }
                }
                listFeactureScripts = GetFeatureScriptDataBase(session, listFeactureNames);
                data.AddObject("DATABASE_FEACTURESCRIPTS", listFeactureScripts);
                // Add all the properties in a single variable
                //session["CUSTOMACTIONDATA_PROPERTIES"] = data.ToString();

                // Schedule deferred actions 
                session.DoAction("CA_DataBaseExecuteScripts", data);
                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                InstallUtilities.WriteLogInstall(session,"Exception to establish installation paths for database.", ex, true);
                MessageBox.Show(ex.Message);
                return ActionResult.Failure;
            }
        }

        /// <summary>Make test the connection to the data source.</summary>
        /// Declare this method in a CustomAction to Execute = " immediate" and invoke it from a button with  Event="DoAction".
        /// <param name="session">Windows Installer Session.</param>
        /// <returns>Returns execution status .</returns>
        [CustomAction]
        public static ActionResult TestSqlConnection(Session session)
        {
            try
            {
                if (session == null)
                {
                    throw new ArgumentNullException("session");
                }

                SetSessionProperty(session, "DATABASE_TEST_CONNECTION", "0");
                string sConnectionString = GetConnectionString(session, false);

                using (var sqlConect = new SqlConnection(sConnectionString))
                {
                    sqlConect.Open();
                }
                SetSessionProperty(session, "DATABASE_TEST_CONNECTION", "1");

                MessageBox.Show(@"Test successful!", @"Test authentication", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                InstallUtilities.WriteLogInstall(session, "An Exception occured trying to connect.", ex, true);
                MessageBox.Show(ex.Message, @"Test authentication", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return ActionResult.Success;
        }

        #endregion Custom actions

        #region Private methods

        /// <summary>
        ///     Returns the current assembly ..
        /// </summary>
        /// <returns>Returns the current assembly.</returns>
        private static Assembly GetAssembly()
        {
            if (InstallUtilities.AssemblyCurrent == null)
            {
                InstallUtilities.AssemblyCurrent = Assembly.GetExecutingAssembly();
            }
            return InstallUtilities.AssemblyCurrent;
        }

        /// <summary>
        ///     Returns the connection to SQL Server.
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="isCustomActionData">
        ///     <para>
        ///         <c>true</c>: Indicates return the value stored in session. CustomActionData[propertyName] . Useful for
        ///         deferred action.
        ///     </para>
        ///     <para><c>false</c>: Indicates return the value stored in session[propertyName] . Useful for immediate action.</para>
        /// </param>
        /// <returns>Returns connection to SQL Server.</returns>
        private static SqlConnection GetConnection(Session session, bool isCustomActionData)
        {
            try
            {
                if (m_sqlConectionMain == null || m_sqlConectionMain.State == ConnectionState.Closed)
                {
                    string sConnectionString = GetConnectionString(session, isCustomActionData);
                    m_sqlConectionMain.ConnectionString = sConnectionString;
                    m_sqlConectionMain.Open();
                }
                return m_sqlConectionMain;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Returns connection string using data supplied by the user.
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="isCustomActionData">
        ///     <para>
        ///         <c>true</c>: Indicates return the value stored in session.CustomActionData[propertyName] . Useful for
        ///         deferred action.
        ///     </para>
        ///     <para><c>false</c>: Indicates return the value stored in session[propertyName] . Useful for immediate action.</para>
        /// </param>
        /// <returns> Returns  connection string</returns>
        private static string GetConnectionString(Session session, bool isCustomActionData)
        {
            //Obsolete 
            isCustomActionData = false;
            string sConnectionString;

            if (GetSessionProperty(session, "DATABASE_WINDOWSAUTHENTICATION", isCustomActionData) == "1")
            {
                sConnectionString =
                    string.Format("Integrated Security=SSPI;Persist Security Info=False;Data Source={0};",
                        GetSessionProperty(session, "DATABASE_SERVERNAME", isCustomActionData).Trim());
            }
            else
            {
                sConnectionString =
                    string.Format("Persist Security Info=False;Data Source={0};User ID={1};Password={2};",
                        GetSessionProperty(session, "DATABASE_SERVERNAME", isCustomActionData),
                        GetSessionProperty(session, "DATABASE_USERNAME", isCustomActionData),
                        GetSessionProperty(session, "DATABASE_PASSWORD", isCustomActionData));
            }

            if (GetSessionProperty(session, "DATABASE_AUTHENTICATEDATABASE", isCustomActionData) == "1")
            {
                sConnectionString += string.Format("Initial Catalog={0};",
                    GetSessionProperty(session, "DATABASE_NAME", isCustomActionData));
            }
            return sConnectionString;
        }

        /// <summary>
        ///     Returns the contents of the script file, stored as a resource in the assembly.
        /// </summary>
        /// <param name="path">folder where the file is located.</param>
        /// <param name="fileName">script file name.</param>
        /// <returns>Returns as text the script</returns>
        private static string GetSQLScriptFromAssembly(string path, string fileName)
        {
            StreamReader reader = null;
            string sFile = null;
            try
            {
                Assembly asm = GetAssembly();

                //foreach (string s in asm.GetManifestResourceNames())
                //{
                //    Console.WriteLine(s);
                //}

                // Folder names must not start with a number , if they do you must start the folder name with underscore (_ )
                sFile = asm.GetName().Name + "." + path.Replace("\\", ".") + "." + fileName;

                Stream strm = asm.GetManifestResourceStream(sFile);
                reader = new StreamReader(strm);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new InstallerException(
                    "Error while getting SQL Script form assembly: " + (string.IsNullOrEmpty(sFile) ? "" : sFile), ex);
            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        ///     Returns the contents of the script file, stored in a file on disk.
        /// </summary>
        /// <param name="path">folder where the file is located.</param>
        /// <param name="fileName">script file name.</param>
        /// <returns>Returns the script as text</returns>
        private static string GetSQLScriptFromFile(string path, string fileName)
        {

            string f = Path.Combine(path, fileName);
            if (File.Exists(f))
            {
                TextReader r = new StreamReader(f, Encoding.Default); //System.Text.Encoding.GetEncoding(1252)
                try
                {
                    return r.ReadToEnd();
                }
                finally
                {
                    try
                    {
                        r.Close();
                    }
                    catch
                    {
                    }
                }
            }
            throw new InstallerException("Error while Getting SQL Script From File: " +
                                         (string.IsNullOrEmpty(f) ? "" : f));
        }

        /// <summary>
        ///     Run the script in the data source.
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="path">folder where the file is located.</param>
        /// <param name="fileName">Name of file script.</param>
        /// <param name="isCustomActionData">
        ///     <para>
        ///         <c>true</c>: Indicates return the value stored in session.CustomActionData[propertyName]. Useful for deferred
        ///         action.
        ///     </para>
        ///     <para><c>false</c>: Indicates return the value stored in session[propertyName]. Useful for immediate action.</para>
        /// </param>
        /// <returns>Retorns <c>true</c> if the process is successful , otherwise  <c>false</c>.</returns>
        private static bool ExecuteSQLScript(Session session, string path, string fileName, bool isCustomActionData)
        {
            return ExecuteSQLScript(session, path, fileName, true, isCustomActionData);
        }

        /// <summary>
        ///     Run the script in the data source.
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="path">folder where the file is located.</param>
        /// <param name="fileName">Name of file script.</param>
        /// <param name="usingTransaction">Whether to use transactions.</param>
        /// <param name="isCustomActionData">
        ///     <para>
        ///         <c>true</c>: Indicates return the value stored in session.CustomActionData[propertyName]. Useful for deferred
        ///         action.
        ///     </para>
        ///     <para><c>false</c>: Indicates return the value stored in session[propertyName]. Useful for immediate action.</para>
        /// </param>
        /// <returns>Retorns <c>true</c> if the process is successful , otherwise  <c>false</c>.</returns>
        private static bool ExecuteSQLScript(Session session, string path, string fileName, bool usingTransaction, bool isCustomActionData)
        {
            bool bOK = true;
            string sCommandText = "", sScript = null, sMessage;
            var command = new SqlCommand();
            SqlConnection cnx;
            int i;
            try
            {
                InstallUtilities.WriteLogInstall(session,
                    string.Format("Begin ExecuteSQL, Path: {0}, Filename: {1} ...", path, fileName), null, false);

                // Texto script
                
                sScript = GetSQLScriptFromFile(path, fileName).Trim();
                //sScript = ReplaceVariables(session, sScript, isCustomActionData);
                if (string.IsNullOrWhiteSpace(sScript))
                {
                    return true;
                }

                cnx = GetConnection(session, isCustomActionData);
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 0;
                command.Connection = cnx;

                // Divide the file into separate segments for each segments run GO
                // Probably won't be needing this
                string[] splits =
                {
                    "GO\n", "GO\r\n", "GO\t", "GO \r\n", "GO \n", "GO  \r\n", "GO  \n", "GO   \n",
                    "GO   \r\n", "GO    \r\n"
                };
                string[] sSQL = sScript.Split(splits, StringSplitOptions.RemoveEmptyEntries);
                
                if (usingTransaction)
                {
                    using (var scope = new TransactionScope())
                    {
                        foreach (string s in sSQL)
                        {
                            sCommandText = s.Trim();
                            if (sCommandText.EndsWith("GO", StringComparison.OrdinalIgnoreCase))
                            {
                                sCommandText = sCommandText.Substring(0, sCommandText.Length - 2);
                            }

                            if (string.IsNullOrWhiteSpace(sCommandText))
                                continue;

                            command.CommandText = sCommandText;
                            command.ExecuteNonQuery();
                        }
                        scope.Complete();
                    }
                }
                else
                {
                    foreach (string s in sSQL)
                    {
                        sCommandText = s.Trim();
                        if (sCommandText.EndsWith("GO", StringComparison.OrdinalIgnoreCase))
                        {
                            sCommandText = sCommandText.Substring(0, sCommandText.Length - 2);
                        }

                        if (string.IsNullOrWhiteSpace(sCommandText))
                            continue;
                  
                        command.CommandText = sCommandText;
                        command.ExecuteNonQuery(); 
                     
                    }
                }
            }
            catch (Exception ex)
            {
                bOK = false;
                sMessage = "Exception while executing script: " + Path.Combine(path, fileName);
                InstallUtilities.WriteLogInstall(session, sMessage, ex, true);
                InstallUtilities.WriteLogInstall(session, "COMMAND EXECUTED :", null, false);
                InstallUtilities.WriteLogInstall(session, sCommandText, null, false);

                sCommandText = sCommandText.Length > 1000 ? sCommandText.Substring(0, 1000) + " ..." : sCommandText;
                sCommandText = sMessage + Environment.NewLine + Environment.NewLine +
                               " Exception: " + ex.Message + Environment.NewLine + Environment.NewLine + sCommandText;

                MessageBox.Show(sCommandText, @"Error executing database script", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                if (MessageBox.Show(
                    @"Continue running the following script?" + Environment.NewLine + Environment.NewLine +
                    @"If 'No' installation will be aborted." +
                    Environment.NewLine + @"Please Note that all the previous Scripts run to completion.", @"Continue Installation ",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bOK = true;
                }
                else
                {
                    throw new InstallerException(sMessage + ". " + ex.Message);
                }
            }
            finally
            {
                command.Dispose();
            }

            return bOK;
        }

        /// <summary>
        ///     Replaces the script variables to the values you provide.
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="script">text with script.</param>
        /// <param name="isCustomActionData">
        ///     <para>
        ///         <c>true</c>: Indicates return the value stored in session.CustomActionData[propertyName]. Useful for deferred
        ///         action.
        ///     </para>
        ///     <para><c>false</c>: Indicates return the value stored in session[propertyName]. Useful for immediate action.</para>
        /// </param>
        /// <returns> script text replaced with variables.</returns>
        private static string ReplaceVariables(Session session, string script, bool isCustomActionData)
        {
            int i;
            string sPath;
            if (string.IsNullOrWhiteSpace(script))
                return script;

            script = script.Replace("$(DATABASE_NAME)", GetSessionProperty(session, "DATABASE_NAME", isCustomActionData));

            script = script.Replace("$(DATABASE_MAILPROFILENAME)",
                GetSessionProperty(session, "DATABASE_MAILPROFILENAME", isCustomActionData));
            script = script.Replace("$(DATABASE_MAILBOX)",
                GetSessionProperty(session, "DATABASE_MAILBOX", isCustomActionData));
            script = script.Replace("$(DATABASE_MAILIP)",
                GetSessionProperty(session, "DATABASE_MAILIP", isCustomActionData));

            script = script.Replace("$(DATABASE_OPERATORMAILBOX)",
                GetSessionProperty(session, "DATABASE_OPERATORMAILBOX", isCustomActionData));
            script = script.Replace("$(DATABASE_PROXYWINDOWSUSER)",
                GetSessionProperty(session, "DATABASE_PROXYWINDOWSUSER", isCustomActionData));
            script = script.Replace("$(DATABASE_PROXYPASSWORD)",
                GetSessionProperty(session, "DATABASE_PROXYPASSWORD", isCustomActionData));

            /*
            List<DataBasePathTO> listPaths = GetCustomTableDataBasePaths(session, isCustomActionData);
            for (i = 0; i < listPaths.Count; i++)
            {
                if (listPaths[i].Path.EndsWith("\\"))
                {
                    sPath = listPaths[i].Path.Substring(0, listPaths[i].Path.Length - 1);
                }
                else
                {
                    sPath = listPaths[i].Path;
                }
                script = script.Replace("$(" + listPaths[i].Name + ")", sPath);
            }*/

            return script;
        }

        #endregion Private methods

        #region Session methods

        /// <summary>
        ///     Returns installation paths database stored in CustomTable .
        /// </summary>
        /// <param name="session"> Windows Installer Session.</param>
        /// <param name="isCustomActionData">
        ///     <para>
        ///         <c>true</c>: Indicates return the value stored in session.CustomActionData[propertyName]. Useful for deferred
        ///         action.
        ///     </para>
        ///     <para><c>false</c>: Indicates return the value stored in session[propertyName]. Useful for immediate action.</para>
        /// </param>
        /// <returns> Return installation paths for database.</returns>
        /// 
        /*private static List<DataBasePathTO> GetCustomTableDataBasePaths(Session session, bool isCustomActionData)
        {
            try
            {
                List<DataBasePathTO> listPaths;
                    //DATABASE_PATHS = null
                    listPaths = session.CustomActionData.GetObject<List<DataBasePathTO>>("DATABASE_PATHS");
                   
                }
                else
                {
                isCustomActionData = true;
                if (isCustomActionData)
                {
                    listPaths = new List<DataBasePathTO>();
                    DataBasePathTO path;
                    string sPath;
                    using (View v = session.Database.OpenView
                        ("SELECT * FROM `TABLE_DATABASE_PATHS`"))
                    {
                        if (v != null)
                        {
                            v.Execute();
                            for (Record r = v.Fetch(); r != null; r = v.Fetch())
                            {
                                sPath = r.GetString(3);
                                if (string.IsNullOrWhiteSpace(sPath))
                                {
                                    sPath = GetSessionProperty(session, "INSTALLLOCATION", isCustomActionData);
                                }

                                path = new DataBasePathTO
                                {
                                    Name = r.GetString(1),
                                    Description = r.GetString(2),
                                    Path = sPath
                                };
                                listPaths.Add(path);
                                r.Dispose();
                            }
                        }
                    }
                }

                if (listPaths == null || listPaths.Count == 0)
                {
                    throw new InstallerException("No configured installation paths ");
                }
                return listPaths;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Error database paths?");
                InstallUtilities.WriteLogInstall(session, "Exception , GetCustomTableDataBasePaths", ex, true);
                throw;
            }
        }
        */
        /// <summary>
        ///     Returns data from the MSI tables: `FeatureComponents`, `Feature`, `Component`, `File`.
        ///     We filter to just get the files that end in (*.SQL).
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="listFeatureNames">List list with names to be installed.</param>
        /// <returns>Returns scripts database files to be installed.</returns>
        private static List<FeatureInstallTO> GetFeatureScriptDataBase(Session session, List<string> listFeatureNames)
        {
            try
            {
                List<FeatureInstallTO> listFeatures;

                string sLevel = session["INSTALLLEVEL"];
                listFeatures = new List<FeatureInstallTO>();
                FeatureInstallTO f;
                int i;
                string sFileName;
                string sQuery = "SELECT `Feature`.`Feature`, `Feature`.`Title`, `Feature`.`Display`, " +
                                " `Component`.`Directory_`, `File`.`FileName` " +
                                " FROM `FeatureComponents`, `Feature`, `Component`, `File` " +
                                " WHERE `FeatureComponents`.`Feature_` = `Feature`.`Feature` " +
                                " AND `FeatureComponents`.`Component_` = `Component`.`Component` " +
                                " AND `File`.`Component_` = `Component`.`Component` " +
                                " AND `Feature`.`RuntimeLevel` > 0 AND `Feature`.`Level` > 0 " +
                    // " AND `Feature`.`Level` <= " + sLevel +
                                " ORDER BY `Feature`.`Display`";
                using (View v = session.Database.OpenView(sQuery))
                {

                    if (v != null)
                    {
                        v.Execute();
                        for (Record r = v.Fetch(); r != null; r = v.Fetch())
                        {
                            if (listFeatureNames.Contains(r.GetString("Feature")) &&
                                r.GetString("FileName").ToUpper().EndsWith(".SQL"))
                            {
                                i = r.GetString("FileName").IndexOf("|", StringComparison.Ordinal);
                                if (i > 0)
                                {
                                    sFileName = r.GetString("FileName").Substring(i + 1);
                                }
                                else
                                {
                                    sFileName = r.GetString("FileName");
                                }
                                f = new FeatureInstallTO
                                {
                                    Feature = r.GetString("Feature"),
                                    Title = r.GetString("Title"),
                                    DisplayOrder = r.GetInteger("Display"),
                                    FileName = sFileName,
                                    DirectoryPath = session.GetTargetPath(r.GetString("Directory_"))
                                };
                                listFeatures.Add(f);
                            }
                            r.Dispose();
                        }
                    }
                }

                return listFeatures;
            }
            catch (Exception ex)
            {
                InstallUtilities.WriteLogInstall(session, "Exception, ReadFeactureScriptDataBase", ex, true);
                throw;
            }
        }

        /// <summary>
        ///     Returns the value of an object property of the object <paramref name="session" />.
        ///     The value can be returned to: session[propertyName] or session.CustomActionData[propertyName].
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="propertyName">Property Name </param>
        /// <param name="isCustomActionData">
        ///     <para>
        ///         <c>true</c>: Indicates return the value stored in session.CustomActionData[propertyName]. Useful for deferred
        ///         action.
        ///     </para>
        ///     <para><c>false</c>: Indicates return the value stored in session[propertyName]. Useful for immediate action.</para>
        /// </param>
        private static string GetSessionProperty(Session session, string propertyName, bool isCustomActionData)
        {
            if (isCustomActionData)
            {
                return session.CustomActionData[propertyName];
            }
            return session[propertyName];
        }

        /// <summary>
        ///     Set the element object Session.CustomActionData <paramref name="session" />,
        ///     Using the value of the session[propertyName].
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="propertyName">Property Name </param>
        private static void SetCustomActionData(Session session, string propertyName)
        {
            if (session.CustomActionData.ContainsKey(propertyName))
                session.CustomActionData[propertyName] = session[propertyName];
            else
                session.CustomActionData.Add(propertyName, session[propertyName]);
        }

        /// <summary>
        ///     Set the CustomActionData collection element: <paramref name="data" />,
        ///     Using the value of the session property: session[propertyName].
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="propertyName">Property Name </param>
        /// <param name="data">Collection properties.</param>
        private static void SetCustomActionData(Session session, string propertyName, CustomActionData data)
        {
            if (data.ContainsKey(propertyName))
                data[propertyName] = session[propertyName];
            else
                data.Add(propertyName, session[propertyName]);
        }

        /// <summary>
        ///     Sets the value of a property.
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="propertyName">Property Name </param>
        /// <param name="value">Property value.</param>
        private static void SetSessionProperty(Session session, string propertyName, string value)
        {
            try
            {
                session[propertyName] = value;
            }
            catch
            {
            }
        }

        /// <summary>
        ///     Update the data stored in Session.CustomActionData with details <paramref name="listPaths" />.
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="listPaths">Paths installation database.</param>
        /// <param name="isCustomActionData">
        ///     <para>
        ///         <c>true</c>: Indicates return the value stored in session.CustomActionData[propertyName]. Useful for deferred
        ///         action.
        ///     </para>
        ///     <para><c>false</c>: Indicates return the value stored in session[propertyName]. Useful for immediate action.</para>
        /// </param>
        public static void UpdateCustomTableDataBasePaths(Session session, List<DataBasePathTO> listPaths,
            bool isCustomActionData)
        {
            try
            {
                Record record;
                View viewWI;

                if (listPaths == null || listPaths.Count < 0)
                    return;

                if (session == null)
                {
                    throw new ArgumentNullException("session");
                }

                if (isCustomActionData)
                {
                    if (session.CustomActionData.ContainsKey("DATABASE_PATHS"))
                    {
                        session.CustomActionData.Remove("DATABASE_PATHS");
                        session.CustomActionData.AddObject("DATABASE_PATHS", listPaths);
                    }
                    else
                    {
                        session.CustomActionData.AddObject("DATABASE_PATHS", listPaths);
                    }
                }
                else
                {
                    //TableInfo info = session.Database.Tables["TABLE_DATABASE_PATHS"];
                    //for (int i = 0; i < listPaths.Count; i++)
                    //{
                    //    record = session.Database.CreateRecord(info.Columns.Count);
                    //    record.FormatString = info.Columns.FormatString;
                    //    record.SetString(1, listPaths[i].Name);
                    //    record.SetString(2, listPaths[i].Description);
                    //    record.SetString(3, listPaths[i].Path);
                    //    session.Database.Execute(info.SqlInsertString + " TEMPORARY", record);
                    //}

                    // Otro metodo
                    //viewWI = session.Database.OpenView("DELETE FROM `TABLE_DATABASE_PATHS`");
                    //viewWI.Execute();
                    //viewWI.Close();

                    TableInfo info = session.Database.Tables["TABLE_DATABASE_PATHS"];
                    viewWI = session.Database.OpenView("SELECT * FROM TABLE_DATABASE_PATHS");
                    //viewWI.Execute();
                    for (int i = 0; i < listPaths.Count; i++)
                    {
                        //                        record = session.Database.CreateRecord(3);
                        record = session.Database.CreateRecord(info.Columns.Count);
                        record.FormatString = info.Columns.FormatString;

                        //record.SetString(1, listPaths[i].Name);
                        //record.SetString(2, listPaths[i].Description);
                        //record.SetString(3, listPaths[i].Path);

                        record[1] = listPaths[i].Name;
                        record[2] = listPaths[i].Description;
                        record[3] = listPaths[i].Path;

                        //                        viewWI.Modify(ViewModifyMode.Replace, record);
                        viewWI.Modify(ViewModifyMode.InsertTemporary, record);
                        record.Dispose();
                    }
                    viewWI.Close();
                }
            }
            catch (Exception ex)
            {
                InstallUtilities.WriteLogInstall(session, "Exception when updating rows CustomTable", ex, true);
                throw;
            }
        }

        #endregion Session methods
    }
}