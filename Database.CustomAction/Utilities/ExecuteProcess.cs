using System;
using System.Diagnostics;
using System.Threading;

namespace Database.CustomAction.Utilities
{
    /// <summary>
    ///     Class with utilities to run process as *. EXE , . * MSI *. BAT .
    ///     From an installer can invoke another EXE or MSI installer .
    /// </summary>
    public class ExecuteProcess
    {
        #region Fields

        /// <summary>
        ///     Location of the file to be executed.
        /// </summary>
        private readonly string m_filePath = "";

        /// <summary>
        ///     The process in which the executable is run.
        /// </summary>
        private readonly Process m_process = null;

        /// <summary>
        ///     Notification that the process thread (Setup.exe ) is over.
        /// </summary>
        private readonly AutoResetEvent m_setupCompleted = new AutoResetEvent(false);

        /// <summary>
        ///     Log path location.
        /// </summary>
        private string m_pathLog = "";

        /// <summary>
        ///     Wait for end processes in milliseconds (5 min = 300000, 2 min = 120000).
        /// </summary>
        private int m_waitForExit = 300000;

        #endregion Fields

        #region Constructs

        /// <summary>
        ///     Constructor , creates a new instance.
        /// </summary>
        /// <param name="pathLog">Path log information .</param>
        /// <param name="filePaht">location of file to be executed.</param>
        public ExecuteProcess(string pathLog, string filePaht)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(m_filePath))
                    throw new ArgumentNullException("_filePath");

                m_pathLog = pathLog;
                m_filePath = filePaht;

                m_process.Exited += ExecuteProcess_Exited;
                m_process.EnableRaisingEvents = true;

                // _process.WaitForExit(_waitForExit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Constructs

        #region Public methods

        /// <summary>
        ///     Handles the event notification process thread (Setup.exe ) is over.
        /// </summary>
        /// <param name="sender">object that fires the event.</param>
        /// <param name="e">Arguments <seealso cref="EventArgs" />  with event data ..</param>
        public void setupExeExited(object sender, EventArgs e)
        {
            m_setupCompleted.Set();
        }

        /// <summary>
        ///     Invokes the process using the process class <seealso cref="System.Diagnostics.Process" />.
        /// </summary>
        public Process Start()
        {
            return Start(null);
        }

        /// <summary>
        ///     Invokes the process using the process class <seealso cref="System.Diagnostics.Process" />.
        /// </summary>
        /// <param name="arguments">Arguments to run the file.</param>
        public Process Start(string arguments)
        {
            Process rtnProcess = null;

            try
            {
                if (string.IsNullOrWhiteSpace(m_filePath))
                    throw new ArgumentNullException("_filePath");

                if (null != arguments)
                {
                    rtnProcess = Process.Start(m_filePath, arguments);
                }
                else
                {
                    rtnProcess = Process.Start(m_filePath);
                }
            }
            catch
            {
                throw;
            }

            return rtnProcess;
        }

        #endregion Public methods

        #region Handle events

        /// <summary>
        ///     Handles the event <seealso cref="Exited" /> fired to end the process.
        /// </summary>
        /// <param name="sender">object that fired the event.</param>
        /// <param name="e">Arguments with event data <seealso cref="EventArgs" />.</param>
        private void ExecuteProcess_Exited(object sender, EventArgs e)
        {
            // Do nothing.            
        }

        #endregion Handle events
    }
}