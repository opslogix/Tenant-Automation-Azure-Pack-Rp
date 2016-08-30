using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Database.CustomAction.Utilities;

namespace Database.CustomAction.UI
{
    /// <summary>
    ///     Form to configure installation paths.
    /// </summary>
    public partial class frmPaths : Form
    {
        #region Fields

        /// <summary>
        ///     Current directory.
        /// </summary>
        private readonly string m_dirCurrent;

        /// <summary>
        ///     Paths.
        /// </summary>
        private readonly List<DataBasePathTO> m_listPaths;

        #endregion Fields

        #region Constructs

        /// <summary>
        ///     Default constructor , creates a new instance.
        /// </summary>
        public frmPaths()
        {
            InitializeComponent();

            m_dirCurrent = Assembly.GetExecutingAssembly().Location;
        }

        /// <summary>
        ///     Constructor , creates a new instance.
        /// </summary>
        /// <param name="listPaths"></param>
        public frmPaths(List<DataBasePathTO> listPaths)
        {
            InitializeComponent();
            m_dirCurrent = Assembly.GetExecutingAssembly().Location;
            m_listPaths = listPaths;
            GetInstallProperties();
        }

        #endregion Constructs

        #region Private methods

        /// <summary>
        ///     Retrieves the context parameters for full installation and controls..
        /// </summary>
        private void GetInstallProperties()
        {
            try
            {
                dgvPaths.Columns[0].DataPropertyName = "Name";
                dgvPaths.Columns[1].DataPropertyName = "Description";
                dgvPaths.Columns[2].DataPropertyName = "Path";

                dgvPaths.DataSource = m_listPaths;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Retrieving installation parameters ", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Set parameters for the installation context.
        /// </summary>
        private bool SetParametersInstallContext()
        {
            try
            {
                string sPath, sPathFirts;
                sPathFirts = dgvPaths.Rows[0].Cells["dgvColPath"].Value.ToString().Trim();
                if (string.IsNullOrWhiteSpace(sPathFirts))
                {
                    throw new ArgumentOutOfRangeException(@"You must set the main path ");
                }

                if (chkSamePath.Checked)
                {
                    for (int i = 0; i < m_listPaths.Count; i++)
                    {
                        m_listPaths[i].Path = sPathFirts;
                    }
                }
                else
                {
                    for (int i = 0; i < m_listPaths.Count; i++)
                    {
                        if (i == 0)
                        {
                            sPath = sPathFirts;
                        }
                        else
                        {
                            sPath = dgvPaths.Rows[i].Cells["dgvColPath"].Value.ToString().Trim();
                            if (string.IsNullOrWhiteSpace(sPath))
                            {
                                sPath = sPathFirts;
                            }
                        }

                        m_listPaths[i].Path = sPath;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Setting installation parameters ", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion Private methods

        #region Handle events

        /// <summary>
        ///     Handles the event <seealso cref="Click" /> , fired to accept dialog.
        /// </summary>
        /// <param name="sender">object that fired the event .</param>
        /// <param name="e">Arguments with event data</param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (SetParametersInstallContext())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        ///     / Handles the event <seealso cref="Click" /> , soared to cancel the dialog.
        /// </summary>
        /// <param name="sender">object that fired the event .</param>
        /// <param name="e">Arguments with event data</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        ///     Handles the event <seealso cref="CellClick" /> to present a dialog box to select folder.
        /// </summary>
        /// <param name="sender">object that fired the event .</param>
        /// <param name="e">Event <seealso cref="DataGridViewCellEventArgs" /> Arguments.</param>
        private void dgvPaths_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string path;
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvPaths.Columns["dgvColPathDialog"].Index)
            {
                if (dgvPaths.Rows[e.RowIndex].Cells["dgvColPath"].Value == null ||
                    string.IsNullOrWhiteSpace(dgvPaths.Rows[e.RowIndex].Cells["dgvColPath"].Value.ToString()))
                {
                    path = m_dirCurrent;
                }
                else
                {
                    path = dgvPaths.Rows[e.RowIndex].Cells["dgvColPath"].Value.ToString();
                }

                dgvPaths.Rows[e.RowIndex].Cells["dgvColPath"].Value = InstallUtilities.GetFolder(path);
            }
        }

        #endregion Handle events
    }
}