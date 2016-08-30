/* ========================================================================== 
Links: Progress bar: http://taocoyote.wordpress.com/2009/05/19/adding-managed-custom-actions-to-the-progressbar/
========================================================================== */

using Microsoft.Deployment.WindowsInstaller;

namespace Database.CustomAction.Utilities
{
    /// <summary>
    ///     Class methods to present a progress bar.
    /// </summary>
    public static class InstallProgress
    {
        /// <summary>
        ///     Resets the progress bar to zero and sets a new value for total ticks..
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="totalTicks">Total number of ticks.</param>
        /// <returns>Returns the message handler .</returns>
        public static MessageResult ResetProgress(Session session, int totalTicks)
        {
            var record = new Record(4);
            record[1] = "0"; // / Set reset progress bar and set the total number of ticks of the bar

            record[2] = totalTicks.ToString(); // Total ticks

            //Move progress bar
            // 0: Moving forward from left to right . 1: Way back from right to left )
            record[3] = "0";

            // 0: Execution in progress, the UI calculates and displays the time remaining 
            // 1: Creating script execution . The UI displays a message while the installer finishes preparing the installation.
            record[4] = "0";

            return session.Message(InstallMessage.Progress, record);
        }

        /// <summary>
        ///     Set the amount to increase the progress bar for each message sent ActionData .
        /// </summary>
        /// <param name="session">  Windows Installer Session.</param>
        /// <param name="ticks"> Number of ticks to be added to the progress bar.</param>
        /// <param name="increaseForActionData">
        ///     Indicates whether the progress bar increases with each message sent by ActionData:
        ///     <para>False: : Indicates send the current message to the progress report.</para>
        ///     <para>
        ///         True: Indicates increase the progress bar in each ActionData in the number of ticks <paramref name="ticks" />
        ///         .
        ///     </para>
        /// </param>
        /// <returns>Returns message handler.</returns>
        public static MessageResult NumberOfTicksPerActionData(Session session, int ticks, bool increaseForActionData)
        {
            var record = new Record(3);
            record[1] = "1"; // Provides information related to the progress message sent by the current action

            // Ticks ActionData increase in each message sent by the custom action , this value is ignored if field 3 is 0
            record[2] = ticks.ToString();

            if (increaseForActionData)
            {
                record[3] = "1";
                // Set the progress bar increasing in each ActionData in the number of ticks in field 2 espeficicados
            }
            else
            {
                record[3] = "0"; // / Set the current message to send progress report 
            }
            return session.Message(InstallMessage.Progress, record);
        }

        /// <summary>
        ///     Presents a dialog ActionData message and increases the progress bar .
        /// </summary>
        /// <param name="session">  Windows Installer Session.</param>
        /// <param name="message">message to be presented.</param>
        /// <returns>returns message handler .</returns>
        public static MessageResult DisplayActionData(Session session, string message)
        {
            var record = new Record(1);
            record[1] = message;
            return session.Message(InstallMessage.ActionData, record);
        }

        /// <summary>
        ///     Presents a dialog ActionData message and increases the progress bar.
        ///     The template is set to the <see cref="DisplayStatusActionStart()" /> method should look like:
        ///     "Script [1] / [2]"
        /// </summary>
        /// <param name="session">Windows Installer Session .</param>
        /// <param name="parameter1">Parameter  1.</param>
        /// <param name="parameter2">Parameter  2.</param>
        /// <returns> Returns message handler.</returns>
        public static MessageResult DisplayActionData2(Session session, string parameter1, string parameter2)
        {
            var record = new Record(2);
            record[1] = parameter1;
            record[2] = parameter2;
            //record[3] = "Executing job  [1] de [2]";
            return session.Message(InstallMessage.ActionData, record);
        }

        /// <summary>
        ///     Presents a dialog ActionData message and increases the progress bar.
        ///     The template is set to the <see cref="DisplayStatusActionStart()" /> method should look like:
        ///     "Script [1] / [2]: [3]"
        /// </summary>
        /// <param name="session">  Windows Installer Session.</param>
        /// <param name="parameter1">Parameter  1.</param>
        /// <param name="parameter2">Parameter  2.</param>
        /// <param name="parameter3">Parameter  3.</param>
        /// <returns> Returns message handler.</returns>
        public static MessageResult DisplayActionData3(Session session, string parameter1, string parameter2,
            string parameter3)
        {
            var record = new Record(3);
            record[1] = parameter1;
            record[2] = parameter2;
            record[3] = parameter3;
            return session.Message(InstallMessage.ActionData, record);
        }

        /// <summary>
        ///     Updates the status message displayed to the user
        /// </summary>
        /// <param name="session">Windows Installer Session.</param>
        /// <param name="actionName">Name of the action to be executed.</param>
        /// <param name="status">
        ///     text with the state. The text is presented in the control that subscribes to the event
        ///     "ActionText".
        /// </param>
        /// <param name="template">
        ///     Text Template advance process.
        ///     ElThe text is presented in a control that subscribes to the event: ActionData
        ///     Example: " Running the task [1 ] of [2 ]"
        /// </param>
        /// <returns>Returns message handler .</returns>
        public static MessageResult DisplayStatusActionStart(Session session, string actionName, string status,
            string template)
        {
            var record = new Record(3);
            record[1] = actionName;
            record[2] = status;
            record[3] = template;
            return session.Message(InstallMessage.ActionStart, record);
        }

        /// <summary>
        ///     / Increase the current value of all ticks in the specified amount.
        ///     You can use this method the UI phase , a custom action with attribute Execute="immediate"
        ///     When executing prepared scripts.
        /// </summary>
        /// <param name="session">Windows Installer Session .</param>
        /// <param name="totalTicks">number of ticks to be added .</param>
        /// <returns>message handler .</returns>
        public static MessageResult IncrementTotalTicks(Session session, int totalTicks)
        {
            var record = new Record(2);
            record[1] = "3"; // Indicates increase the total number of ticks of the progress bar.
            record[2] = totalTicks.ToString(); // Number of ticks to be added to the current total of the progress bar.
            return session.Message(InstallMessage.Progress, record);
        }

        /// <summary>
        ///     Increases the progress bar indicated the number of ticks.
        /// </summary>
        /// <param name="session">Windows Installer Session .</param>
        /// <param name="tickIncrement">Número de ticks de incremento.</param>
        /// <returns>message handler .</returns>
        public static MessageResult IncrementProgress(Session session, int tickIncrement)
        {
            var record = new Record(2);
            record[1] = "2"; // Set the progress bar increasing
            record[2] = tickIncrement.ToString(); // Number of ticks to be increased at the progress bar .
            //record[3] = 0;
            return session.Message(InstallMessage.Progress, record);
        }

        public static void ThrowInstallError(Session session, string errorMessage)
        {
            var record = new Record(1);
            record[1] = errorMessage;

            session.Message(InstallMessage.FatalExit, record);
        }
    }
}