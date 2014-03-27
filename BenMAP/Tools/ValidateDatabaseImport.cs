﻿// ***********************************************************************
// Assembly         : BenMAP
// Author           :
// Created          : 03-17-2014
//
// Last Modified By : 
// Last Modified On : 03-21-2014
// ***********************************************************************
// <copyright file="ValidateDatabaseImport.cs" company="RTI International">
//     RTI International. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ESIL.DBUtility;


/// <summary>
/// The BenMAP namespace.
/// </summary>
namespace BenMAP
{
    /// <summary>
    /// Class ValidateDatabaseImport
    /// </summary>
    public partial class ValidateDatabaseImport : Form
    {

        TipFormGIF waitMess = new TipFormGIF(); bool sFlog = true;
        /// <summary>
        /// The _TBL
        /// </summary>
        private DataTable _tbl = null;
        /// <summary>
        /// The _col names
        /// </summary>
        private List<string> _colNames = null;
        /// <summary>
        /// The _dic table definition
        /// </summary>
        //private Dictionary<string,string> _dicTableDef = null;
        private Hashtable _hashTableDef = null;
        /// <summary>
        /// The _datasetname
        /// </summary>
        private string _datasetname = string.Empty;
        /// <summary>
        /// The _file
        /// </summary>
        private string _file = string.Empty;
        //private bool _bPassed = true;
        /// <summary>
        /// The errors
        /// </summary>
        private int errors = 0;
        /// <summary>
        /// The warnings
        /// </summary>
        private int warnings = 0;

        private bool _passedValidation = true;

        public bool PassedValidation
        {
            get { return _passedValidation; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateDatabaseImport"/> class.
        /// </summary>
        /// <param name="tblToValidate">The table to validate.</param>
        /// <param name="datasetName">Name of the dataset.</param>
        /// <param name="selectedFile">The selected file.</param>
        public ValidateDatabaseImport(DataTable tblToValidate, string datasetName, string selectedFile):this()
        {
            _tbl = tblToValidate;
            _tbl.CaseSensitive = false;
            _datasetname = datasetName;
            _file = selectedFile;
            txtReportOutput.SelectionTabs = new int[] {120, 200, 350};
            txtReportOutput.SelectionIndent = 10;
            Get_ColumnNames();
            Get_DatasetDefinition();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateDatabaseImport"/> class.
        /// </summary>
        public ValidateDatabaseImport()
        {
            InitializeComponent();
        }
        //Getting column names from the passed in datatable as they are displayed in the file.
        /// <summary>
        /// Get_s the column names.
        /// </summary>
        private void Get_ColumnNames()
        {
          _colNames = new List<string>();
          for (int i = 0; i < _tbl.Columns.Count; i++)
          {
              _colNames.Add(_tbl.Columns[i].ColumnName);
          }
        }
        //Getting dataset definition from the firebird database for the dataset name that was passed in.
        /// <summary>
        /// Get_s the dataset definition.
        /// </summary>
        private void Get_DatasetDefinition()
        {
            FireBirdHelperBase fb = new ESILFireBirdHelper();
            string cmdText = string.Empty;
            
            _hashTableDef = new Hashtable(StringComparer.OrdinalIgnoreCase);
           // _dicTableDef = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                cmdText = string.Format("SELECT COLUMNNAME, DATATYPE, LOWERLIMIT, UPPERLIMIT FROM DATASETDEFININTION WHERE DATASETNAME='{0}'", _datasetname);
                DataTable _obj =   fb.ExecuteDataset(CommonClass.Connection, CommandType.Text, cmdText).Tables[0] as DataTable;
                foreach(DataRow dr in _obj.Rows)
                {
                    
                    //_dicTableDef.Add(dr[0].ToString(), dr[1].ToString());
                    _hashTableDef.Add(dr[0].ToString() + "##COLUMNNAME", dr[0].ToString());
                    _hashTableDef.Add(dr[0].ToString() + "##DATATYPE", dr[1].ToString());
                    _hashTableDef.Add(dr[0].ToString() + "##LOWERLIMIT", dr[2].ToString());
                    _hashTableDef.Add(dr[0].ToString() + "##UPPERLIMIT", dr[3].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Handles the Load event of the ValidateDatabaseImport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ValidateDatabaseImport_Load(object sender, EventArgs e)
        {
            txtReportOutput.Text += string.Format("Date:\t{0}\r\n",DateTime.Today.ToShortDateString());
            FileInfo fInfo = new FileInfo(_file);
            txtReportOutput.Text += string.Format("File Name:\t{0}\r\n\r\n\r\n", fInfo.Name);
            
            this.Refresh();
            Application.DoEvents();
            //txtReportOutput.Text += "Error/Warnings\t Row\t Column Name \t Error/Warning Message\r\n";
        }

        /// <summary>
        /// Handles the Shown event of the ValidateDatabaseImport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ValidateDatabaseImport_Shown(object sender, EventArgs e)
        {
            bool bPassed = true;
            string tip = "Validating, this could take several minutes.";
            WaitShow(tip);
            Application.DoEvents();
            if(bPassed)
            {
                bPassed = VerifyColumnNames();
                txtReportOutput.Refresh();
            }
            if(bPassed)
            {
                bPassed = VerifyTableHasData();
                txtReportOutput.Refresh();
            }
            if(bPassed)
            {
                bPassed = VerifyTableDataTypes();//errors
                txtReportOutput.Refresh();
            }
            txtReportOutput.Text += "\r\n\r\n\r\nSummary\r\n";
            if(errors > 0)
            {
                txtReportOutput.Text += "-----\r\nValidation failed!\r\n";
            }
            txtReportOutput.Text += string.Format("{0} errors\r\n{1} warnings\r\n", errors,warnings);
            txtReportOutput.Refresh();
            SaveValidateResults();
            btnLoad.Enabled = bPassed;
            WaitClose();
        }
        /// <summary>
        /// Saves the validate results.
        /// </summary>
        private void SaveValidateResults()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My BenMAP-CE Files\ValidationResults";
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //file is formated as <dataset name>_year_month_day_hour_min.rtf
            //Monitor_2014_3_24_10_18.rtf
            string FileName = string.Format("{0}_{1}_{2}_{3}_{4}_{5}.rtf", 
                                            _datasetname, DateTime.Now.Year, 
                                            DateTime.Now.Month, DateTime.Now.Day, 
                                            DateTime.Now.Hour, DateTime.Now.Minute);
            txtReportOutput.Text += string.Format("Saved to: {0}", path + string.Format(@"\{0}", FileName));
            File.WriteAllText(path + string.Format(@"\{0}", FileName), txtReportOutput.Text);
        }

        /// <summary>
        /// Varifies the column names.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        private bool VerifyColumnNames()
        {
            bool bPassed = true;
            txtReportOutput.Text += "Verifying Column Names\r\n\r\n";
            txtReportOutput.Text += "Error/Warnings\tRow\tColumn Name\tError/Warning Message\r\n";
            for (int i = 0; i < _colNames.Count; i++)
            {
                //if (!_dicTableDef.ContainsKey(_colNames[i].ToString()))
                if(!_hashTableDef.ContainsValue(_colNames[i].ToString()))
                {
                    txtReportOutput.Text += string.Format("Error\t\t{0}\t is not a valid column name for dataset {1}\r\n", _colNames[i].ToString(), _datasetname);
                    errors++;
                    bPassed = false;
                }
            }
            string sKey = string.Empty;
            foreach(DictionaryEntry dEntry in _hashTableDef)
            {
                sKey = dEntry.Key.ToString();
                if(sKey.Contains("##COLUMNNAME"))
                {
                    if(!_colNames.Contains(dEntry.Value.ToString()))
                    {
                        txtReportOutput.Text += string.Format("Error\t\t{0}\t column is missing for dataset {1}\r\n", dEntry.Value.ToString(), _datasetname);
                        errors++;
                        bPassed = false;
                    }
                }

            }

            //foreach(KeyValuePair<string,string> kvpEntry in _dicTableDef)
            //{
            //    if(!_colNames.Contains(kvpEntry.Key))
            //    {
            //        txtReportOutput.Text += string.Format("Error\t\t{0}\t column is missing for dataset {1}\r\n", kvpEntry.Key.ToString(), _datasetname);
            //        errors++;
            //        bPassed = false;
            //    }
            //}

            if(!bPassed)
                txtReportOutput.Text +="\r\nValidation of columns failed.\r\nPlease check the column header and file.  The columns could be incorrect or the incorrect file was selected.\r\n";
            else
                txtReportOutput.Text += "\r\nValidation of columns passed.\r\n";

            return bPassed;
        }

        /// <summary>
        /// Varifies the table has data.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        private bool VerifyTableHasData()
        {
            bool bPassed = true;
            if(_tbl.Rows.Count < 1)
            {
                txtReportOutput.Text += string.Format("Error\t\t\t\t\t No rows found in file for dataset {1}\r\n", "", _datasetname);
                errors++;
                bPassed = false;
            }

            return bPassed;
        }

        /// <summary>
        /// Varifies the table data types.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        private bool VerifyTableDataTypes()
        {
            string errMsg = string.Empty;
            bool bPassed = true;
            int numChecked = 0;
            string dataType = string.Empty;
            string dataVal = string.Empty;
            lblProgress.Visible = true;
            pbarValidation.Visible = true;

            pbarValidation.Step = 1;
            pbarValidation.Minimum = 0;
            pbarValidation.Maximum = _tbl.Rows.Count;
            pbarValidation.Value = pbarValidation.Minimum;
            txtReportOutput.Text += "\r\n\r\nVerifying data types.\r\n\r\n";
            txtReportOutput.Text += "Error/Warnings\t Row\t Column Name \t Error/Warning Message\r\n";
           Action work = delegate
           {
                foreach(DataRow dr in _tbl.Rows)
                {
                   foreach(DataColumn dc in dr.Table.Columns)
                   {
                        dataType = _hashTableDef[dc.ColumnName + "##DATATYPE"].ToString();
                        dataVal = dr[dc.ColumnName].ToString();
                        
                        try
                        {
                            if (dataVal != string.Empty)
                            {
                                if (!VerifyDataRowValues(dataType, dc.ColumnName, dataVal, out errMsg))
                                {
                                    txtReportOutput.Text += string.Format("Error\t {0}\t {1} \t {2}\r\n", _tbl.Rows.IndexOf(dr), dc.ColumnName, errMsg);
                                    txtReportOutput.Refresh();
                                    errors++;
                                    numChecked++;
                                    bPassed = false;
                                    if(errors == 50)
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    numChecked++;
                                }
                            }
                            else
                            {
                                numChecked++;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                   }
                   
                   if(numChecked % 5 == 0)
                   {
                       pbarValidation.PerformStep();
                        lblProgress.Text = Convert.ToString((int)((double)pbarValidation.Value / pbarValidation.Maximum * 100)) + "%";
                        //txtReportOutput.Refresh();
                        lblProgress.Refresh();
                   }
                }
            };
            work();
            if(!bPassed)
                txtReportOutput.Text += "Validating Datatable values failed.";

            pbarValidation.Visible = false;
            lblProgress.Text = "";
            lblProgress.Visible = false;

            return bPassed;
        }

        /// <summary>
        /// Verifies the data row values.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="valToVerify">The value to verify.</param>
        /// <param name="errMsg">The error MSG.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        private bool VerifyDataRowValues(string dataType, string columnName, string valToVerify, out string errMsg)
        {
            
            string min = _hashTableDef[columnName + "##LOWERLIMIT"].ToString();// Get_Min(columnName, dataType);
            string max = _hashTableDef[columnName + "##UPPERLIMIT"].ToString();//Get_Max(columnName, dataType);
            Regex regx = new Regex(@"^[^~!@#%`^]+$");
            double tempVal;
            int outVal = -1;
            bool bPassed = true;

            errMsg = string.Empty;

            try
            {
                switch (dataType)
                {
                    case "string":

                        if (double.TryParse(valToVerify, out tempVal))
                        {
                            valToVerify = tempVal.ToString();
                        }

                        if (!string.IsNullOrEmpty(min) && bPassed)
                        {
                            if (valToVerify.Length < Convert.ToInt32(min))
                            {
                                errMsg = "Value has an invalid length, too short.";
                                bPassed = false;
                            }
                        }
                        if (!string.IsNullOrEmpty(max) && bPassed)
                        {
                            if (valToVerify.Length > Convert.ToInt32(max))
                            {
                                errMsg = "Value has an invalid length, too long.";
                                bPassed = false;
                            }
                        }

                        if (!regx.IsMatch(valToVerify) && bPassed)
                        {
                            errMsg = "Value has invalid characters.";
                            bPassed = false;
                        }

                        break;
                    case "integer":

                        if (int.TryParse(valToVerify, out outVal))
                        {
                            if (!string.IsNullOrEmpty(min) && bPassed)
                            {
                                if (outVal < Convert.ToInt32(min))
                                {
                                    errMsg = string.Format("Value is not within min({0}) range.", min);
                                    bPassed = false;
                                }
                            }
                            if (!string.IsNullOrEmpty(max) && bPassed)
                            {
                                if (outVal > Convert.ToInt32(max))
                                {
                                    errMsg = string.Format("Value is not within max({0}) range.", max);
                                    bPassed = false;
                                }
                            }
                        }
                        else
                        {
                            errMsg = "Value is not a valid integer.";
                            bPassed = false;
                        }
                        break;
                    case "float":
                        float outValf = -1;
                        if (float.TryParse(valToVerify, out outValf))
                        {
                            if (!string.IsNullOrEmpty(min) && bPassed)
                            {
                                if (outValf < Convert.ToInt32(min))
                                {
                                    errMsg = string.Format("Value is not within min({0}) range.", min);
                                    bPassed = false;
                                }
                            }
                            if (!string.IsNullOrEmpty(max) && bPassed)
                            {
                                if (outValf > Convert.ToInt32(max))
                                {
                                    errMsg = string.Format("Value is not within max({0}) range.", max);
                                    bPassed = false;
                                }
                            }
                        }
                        else
                        {
                            errMsg = "Value is not a valid float.";
                            bPassed = false;
                        }
                        break;
                    case "double":

                        break;
                    case "blob":
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //txtReportOutput.Refresh();
            return bPassed;
        }

        /// <summary>
        /// Get_s the minimum.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>System.String.</returns>
        private string Get_Min(string columnName, string dataType)
        {
            string cmdText = string.Format("SELECT LOWERLIMIT FROM DATASETDEFININTION where DATASETNAME='{0}' " +
                                            "and COLUMNNAME='{1}' and DATATYPE='{2}'", _datasetname, columnName, dataType);
            FireBirdHelperBase fb = new ESILFireBirdHelper();
            string obj = fb.ExecuteScalar(CommonClass.Connection, CommandType.Text, cmdText).ToString();
            if(string.IsNullOrEmpty(obj))
                obj = string.Empty;
            return obj;
        }

        /// <summary>
        /// Get_s the maximum.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>System.String.</returns>
        private string Get_Max(string columnName, string dataType)
        {
            string cmdText = string.Format("SELECT UPPERLIMIT FROM DATASETDEFININTION where DATASETNAME='{0}' " +
                                "and COLUMNNAME='{1}' and DATATYPE='{2}'", _datasetname, columnName, dataType);
            FireBirdHelperBase fb = new ESILFireBirdHelper();
            string obj = fb.ExecuteScalar(CommonClass.Connection, CommandType.Text, cmdText).ToString();
            if (string.IsNullOrEmpty(obj))
                obj = string.Empty;
            return obj;
        }

        /// <summary>
        /// Handles the Click event of the btnLoad control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public void WaitShow(string msg)
        {
            try
            {
                if (sFlog == true)
                {
                    sFlog = false;
                    waitMess.Msg = msg;
                    System.Threading.Thread upgradeThread = null;
                    upgradeThread = new System.Threading.Thread(new System.Threading.ThreadStart(ShowWaitMess));
                    upgradeThread.Start();
                    upgradeThread.IsBackground = true;
                }
            }
            catch (System.Threading.ThreadAbortException Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private delegate void CloseFormDelegate();

        public void WaitClose()
        {
            if (waitMess.InvokeRequired)
                waitMess.Invoke(new CloseFormDelegate(DoCloseJob));
            else
                DoCloseJob();
        }
        private void DoCloseJob()
        {
            try
            {
                if (!waitMess.IsDisposed)
                {
                    if (waitMess.Created)
                    {
                        sFlog = true;
                        waitMess.Close();
                    }
                }
            }
            catch (System.Threading.ThreadAbortException Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
        private void ShowWaitMess()
        {
            try
            {
                if (!waitMess.IsDisposed)
                {
                    waitMess.ShowDialog();
                }
            }
            catch (System.Threading.ThreadAbortException Err)
            {
                MessageBox.Show(Err.Message);
            }
        }
    }
}
