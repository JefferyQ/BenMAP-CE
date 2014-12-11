﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
//using Microsoft.Office.Interop.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet; 



namespace BenMAP
{
    public partial class GBDRollback : Form
    {

        private Dictionary<string, string> checkedCountries = new Dictionary<string, string>();
        private List<System.Drawing.Color> colorPalette = new List<System.Drawing.Color>();
        private List<GBDRollbackItem> rollbacks = new List<GBDRollbackItem>();
        private System.Data.DataTable dtCountries;
        private SpreadsheetDocument spreadsheetDocument;
        private Microsoft.Office.Interop.Excel.Application xlApp;
        private bool selectMapFeaturesOnNodeCheck = true;

        private const int POLLUTANT_ID = 1;
        private const double BACKGROUND = 5.8;
        private const int YEAR = 2010;
        private const string FORMAT_DECIMAL_2_PLACES = "#,###.00";
        private const string FORMAT_DECIMAL_0_PLACES = "N0";

        private System.Data.DataTable dtConcCountry = null;
        private System.Data.DataTable dtConcEntireRollback = null;

        Dictionary<String,IPolygonCategory> selectedButNotSavedIPCs = new Dictionary<String,IPolygonCategory>();

        private class CountryItem 
        {
            string _id;
            string _name;

            public CountryItem(string Id, string Name)
            {
                _id = Id;
                _name = Name;
            }

            public string Id
            {
                get { return _id; }
                set { _id = value; }
            }

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public override string ToString()
            {
                return _name;
            }
        }
      

        public GBDRollback()
        {
            InitializeComponent();

            //set up locations,form size, visibility

            listCountries.Location = new System.Drawing.Point(tvRegions.Location.X, tvRegions.Location.Y);
            //increase height of list countries to better match that of tvRegions
            //to compensate for a rendering bug in the controls
            listCountries.Size = new Size(tvRegions.Size.Width, tvRegions.Size.Height + 2);

            gbCountrySelection.Location = new System.Drawing.Point(gbName.Location.X, gbName.Location.Y);
            gbParameterSelection.Location = new System.Drawing.Point(gbName.Location.X, gbName.Location.Y);
            SetActivePanel(0);
            Size = new Size(906, 777); //form size

            

            //parameter options in gbParameterSelection
            char micrograms = '\u00B5';
            char super3 = '\u00B3';
            lblIncrement.Text = "Increment (" + micrograms.ToString() + "g/m" + super3.ToString() + "):";

            lblIncrementBackground.Text = "Background (" + micrograms.ToString() + "g/m" + super3.ToString() + "):";
            lblPercentageBackground.Text = lblIncrementBackground.Text;

            gbOptionsPercentage.Location = new System.Drawing.Point(gbOptionsIncremental.Location.X, gbOptionsIncremental.Location.Y);
            gbParameterSelection.Controls.Add(gbOptionsPercentage);
            gbOptionsStandard.Location = new System.Drawing.Point(gbOptionsIncremental.Location.X, gbOptionsIncremental.Location.Y);
            gbParameterSelection.Controls.Add(gbOptionsStandard);            
            cboRollbackType.SelectedIndex = 0;
            SetActiveOptionsPanel(0);
            rbRegions.Checked = true;

            txtFilePath.Text = CommonClass.ResultFilePath + @"\GBD";

            LoadCountries();
            LoadTreeView();
            LoadCountryList();
            LoadMap();
            LoadColorPalette();
            LoadStandards();

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();           
        }



        private void LoadColorPalette()
        { 
            
            colorPalette.Add(System.Drawing.Color.FromArgb(165,0,38));
            colorPalette.Add(System.Drawing.Color.FromArgb(215, 48, 39));
            colorPalette.Add(System.Drawing.Color.FromArgb(244, 109, 67));
            colorPalette.Add(System.Drawing.Color.FromArgb(253, 174, 97));
            colorPalette.Add(System.Drawing.Color.FromArgb(254, 224, 144));
            colorPalette.Add(System.Drawing.Color.FromArgb(255, 255, 191));
            colorPalette.Add(System.Drawing.Color.FromArgb(224, 243, 248));
            colorPalette.Add(System.Drawing.Color.FromArgb(171, 217, 233));
            colorPalette.Add(System.Drawing.Color.FromArgb(116, 173, 209));
            colorPalette.Add(System.Drawing.Color.FromArgb(69, 117, 180));
            colorPalette.Add(System.Drawing.Color.FromArgb(49, 54, 149));
        
        }

        private void LoadMap()
        {
            //new map layer
            string mapFile = AppDomain.CurrentDomain.BaseDirectory + @"\Data\Shapefiles\GBDRollback\gadm_worldsimplify.shp";
            IMapPolygonLayer impl = null;
            if (File.Exists(mapFile))
            {
                IFeatureSet fs = (FeatureSet)FeatureSet.Open(mapFile);
                //mapGBD.Layers.Add(fs);
                //IMapFeatureLayer[] mfl = mapGBD.GetFeatureLayers();
                //IFeatureSet fs = (FeatureSet)FeatureSet.Open(mapFile);
                //mapGBD.Layers.Add(fs);
                //IMapFeatureLayer[] mfl = mapGBD.GetFeatureLayers();
                //mfl[0].Symbolizer = new PolygonSymbolizer(Color.Chocolate);
                //mfl[0].SelectionSymbolizer = new PolygonSymbolizer(Color.AliceBlue);
                impl = new MapPolygonLayer(FeatureSet.OpenFile(mapFile));
                //impl.Reproject(_mapArgs.Map.Projection);
                impl.LegendText = "Countries";
                impl.Symbolizer.SetFillColor(System.Drawing.Color.White);
                impl.Symbolizer.SetOutlineWidth(1);
                impl.Symbolizer.OutlineSymbolizer.SetFillColor(System.Drawing.Color.Black);
                mapGBD.Layers.Add(impl);
            }
        }

        private void LoadStandards()
        {
            System.Data.DataSet ds = GBDRollbackDataSource.GetStandardList();
            System.Data.DataTable dtStandards = ds.Tables[0].Copy();//new DataTable();

            //load standard drop down
            cboStandard.DisplayMember = "STANDARD_NAME";
            cboStandard.ValueMember = "STD_ID";
            cboStandard.DataSource = dtStandards;
        }

        private void LoadCountries()
        {
            System.Data.DataSet ds = GBDRollbackDataSource.GetRegionCountryList(YEAR);
            dtCountries = ds.Tables[0].Copy();//new DataTable();
        }

        private void LoadTreeView()
        {
            if (dtCountries != null)
            {
                string region = String.Empty;
                string country = String.Empty;
                string countryid = String.Empty;
                tvRegions.BeginUpdate();
                foreach (DataRow dr in dtCountries.Rows)
                {
                    //new region?
                    if (!region.Equals(dr["REGIONNAME"].ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        region = dr["REGIONNAME"].ToString();
                        tvRegions.Nodes.Add(region, region);
                    }

                    countryid = dr["COUNTRYID"].ToString();
                    country = dr["COUNTRYNAME"].ToString();
                    tvRegions.Nodes[region].Nodes.Add(countryid, country);
                }
                tvRegions.EndUpdate();
            }
        
        }


        private void LoadCountryList()
        {
            if (dtCountries != null)
            {
                System.Data.DataTable dtTemp = dtCountries.DefaultView.ToTable(true, "COUNTRYID", "COUNTRYNAME");
                DataView dv = new DataView(dtTemp);
                dv.Sort = "COUNTRYNAME ASC";
                System.Data.DataTable dtAlph = dv.ToTable();

                string country = String.Empty;
                string countryid = String.Empty;
                foreach (DataRow dr in dtAlph.Rows)
                {
                    countryid = dr["COUNTRYID"].ToString();
                    country = dr["COUNTRYNAME"].ToString();

                    listCountries.Items.Add(new CountryItem(countryid, country));                   
                }
            }

        }

        


        private void cboRollbackType_SelectedIndexChanged(object sender, EventArgs e)
        {

            SetActiveOptionsPanel(cboRollbackType.SelectedIndex);
            switch (cboRollbackType.SelectedIndex)
            {
                case 0:
                    gbOptionsIncremental.Visible = false;
                    gbOptionsPercentage.Visible = true;
                    gbOptionsStandard.Visible = false;
                    
                    pb_incremental.Visible = false;
                    pb_percent.Visible = true;
                    pb_standard.Visible = false;
                    break;
                case 1:
                    gbOptionsIncremental.Visible = true;
                    gbOptionsPercentage.Visible = false;
                    gbOptionsStandard.Visible = false;
                    
                    pb_incremental.Visible = true;
                    pb_percent.Visible = false;
                    pb_standard.Visible = false;

                    break;                
                case 2:
                    gbOptionsIncremental.Visible = false;
                    gbOptionsPercentage.Visible = false;
                    gbOptionsStandard.Visible = true;

                    pb_incremental.Visible = false;
                    pb_percent.Visible = false;
                    pb_standard.Visible = true;

                    break;
                default:
                    gbOptionsIncremental.Visible = false;
                    gbOptionsPercentage.Visible = false;
                    gbOptionsStandard.Visible = false;

                    pb_incremental.Visible = false;;
                    pb_percent.Visible = false;
                    pb_standard.Visible = false;

                    break;
            }

        }

        private void GBDRollback_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you wish to close?", "Confirm Close", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Name is required.");
                txtName.Focus();
                return;
            }
            if (rollbacks.Exists(x => x.Name.Equals(txtName.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                DialogResult result = MessageBox.Show("A rollback with the name " + txtName.Text.Trim() + " already exists.  Do you wish to overwrite it?","", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    txtName.Focus();
                    return;
                }            
            }

            SetActivePanel(1);
            
        }

        private void btnNext2_Click(object sender, EventArgs e)
        {
            //check for country
            if (checkedCountries.Count == 0)
            {
                MessageBox.Show("You must select at least one country.");
                tvRegions.Focus();
                return;
            }

            SetActivePanel(2);
            //cboRollbackType.SelectedIndex = -1;     
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            SetActivePanel(0);
        }

        private void btnBack2_Click(object sender, EventArgs e)
        {
            SetActivePanel(1);
        }

        private void tvRegions_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                CheckChildNodes(e.Node);
                CheckParentNode(e.Node);
            }
            //Color.FromArgb(224, 243, 248)
            //if this is checked AND has no children)
            //then, it is country and we add to listd
            IMapFeatureLayer[] mfl = mapGBD.GetFeatureLayers();
            string filter =  "[ID] = '" + e.Node.Name + "'";
            if ((e.Node.Checked) && (e.Node.Nodes.Count == 0))
            {
                if (!checkedCountries.ContainsKey(e.Node.Name))
                {
                    //add to country list
                    checkedCountries.Add(e.Node.Name,e.Node.Text);
                    //also select on map                
                    if (selectMapFeaturesOnNodeCheck)
                    {
                        //update map
                        IPolygonScheme ips = (IPolygonScheme)mfl[0].Symbology;
                        IPolygonCategory ipc = null;
                        ipc = new PolygonCategory(System.Drawing.Color.FromArgb(0, 255, 255), System.Drawing.Color.FromArgb(0, 225, 225), 1);
                        ipc.FilterExpression = "[ID]='" + e.Node.Name + "'";
                        selectedButNotSavedIPCs.Add(e.Node.Name,ipc);
                        mfl[0].Symbology.AddCategory(ipc);
                        mfl[0].ApplyScheme(mfl[0].Symbology);
                    }
                }
            }
            else
            {
                //remove from country list
                checkedCountries.Remove(e.Node.Name);
                //deselect from map
                if(selectedButNotSavedIPCs.ContainsKey(e.Node.Name))
                {
                    IPolygonCategory ipc = selectedButNotSavedIPCs[e.Node.Name];
                    mfl[0].Symbology.RemoveCategory(ipc);
                    selectedButNotSavedIPCs.Remove(e.Node.Name);
                    mfl[0].ApplyScheme(mfl[0].Symbology);
                 }
            }

            //finally check/uncheck on country-only list box
            //but only if tvRegions is visible to avoid infinite loop
            //see listCountries_ItemCheck event
            if (tvRegions.Visible)
            {
                int index = listCountries.FindStringExact(e.Node.Text);
                if (index >= 0)
                {
                    listCountries.SetItemChecked(index, e.Node.Checked);
                }
            }

        }

        private void CheckChildNodes(TreeNode node)
        {

            //this will set child nodes, if any, to 
            //same status as parent, checked or unchecked
            tvRegions.BeginUpdate();
            foreach (TreeNode item in node.Nodes)
            {
                item.Checked = node.Checked;

                if (item.Nodes.Count > 0)
                {
                    this.CheckChildNodes(item);
                }
            }
            tvRegions.EndUpdate();
        }


        private void CheckParentNode(TreeNode node)
        {
            if (node.Parent == null)
            {
                return;
            }

            //this will set parent node, if any
            //to checked if all children are checked
            //otherwise parent will be unchecked
            tvRegions.BeginUpdate();

            bool allChecked = true;

            //loop siblings of current
            foreach (TreeNode item in node.Parent.Nodes)
            {
                if (!item.Checked)
                {
                    allChecked = false;
                    break;
                }
            }

            node.Parent.Checked = allChecked;

            tvRegions.EndUpdate();
        }

        private void btnSaveRollback_Click(object sender, EventArgs e)
        {
            double d;

            //clean text boxes for numerics
            txtPercentage.Text = txtPercentage.Text.Trim();
            txtPercentageBackground.Text = txtPercentageBackground.Text.Trim();
            txtIncrement.Text = txtIncrement.Text.Trim();
            txtIncrementBackground.Text = txtIncrementBackground.Text.Trim();

            switch (cboRollbackType.SelectedIndex)
            {
                case 0: //percentage
                    if (String.IsNullOrEmpty(txtPercentage.Text))
                    {
                        MessageBox.Show("Percentage is required.");
                        txtPercentage.Focus();
                        return;
                    }
                    if (!Double.TryParse(txtPercentage.Text, out d))
                    {
                        MessageBox.Show("Percentage must be numeric.");
                        txtPercentage.Focus();
                        return;                        
                    }
                     if (d > 100)
                        {
                            MessageBox.Show("Percentage can not be > 100");
                            txtPercentageBackground.Focus();
                            return;
                        }
                        if (d < 0)
                        {
                            MessageBox.Show("Percentage can not be < 0");
                            txtPercentageBackground.Focus();
                            return;
                        }
                    if (!String.IsNullOrEmpty(txtPercentageBackground.Text))
                    {
                        if (!Double.TryParse(txtPercentageBackground.Text, out d))
                        {
                            MessageBox.Show("Background must be numeric.");
                            txtPercentageBackground.Focus();
                            return;
                        }
                       
                    }
                    break;
                case 1: //incremental
                    if (String.IsNullOrEmpty(txtIncrement.Text))
                    {
                        MessageBox.Show("Increment is required.");
                        txtIncrement.Focus();
                        return;
                    }
                    if (!Double.TryParse(txtIncrement.Text, out d))
                    {
                        MessageBox.Show("Increment must be numeric.");
                        txtIncrement.Focus();
                        return;
                    }
                    if (!String.IsNullOrEmpty(txtIncrementBackground.Text))
                    {
                        if (!Double.TryParse(txtIncrementBackground.Text, out d))
                        {
                            MessageBox.Show("Background must be numeric.");
                            txtIncrementBackground.Focus();
                            return;
                        }
                    }
                    break;
                case 2: //standard
                    if (cboStandard.SelectedIndex < 0)
                    {
                        MessageBox.Show("Standard is required.");
                        cboStandard.Focus();
                        return;
                    }
                    break;
            }


            GBDRollbackItem rollback = new GBDRollbackItem();
            rollback.Name = txtName.Text;
            rollback.Description = txtDescription.Text;
            rollback.Countries = new Dictionary<string,string>(checkedCountries);
            switch (cboRollbackType.SelectedIndex)
            {
                case 0: //percentage
                    rollback.Type = GBDRollbackItem.RollbackType.Percentage;
                    rollback.Percentage = Double.Parse(txtPercentage.Text);
                    rollback.Background = BACKGROUND;
                    //if (!String.IsNullOrEmpty(txtPercentageBackground.Text))
                    //{
                    //    rollback.Background = Double.Parse(txtPercentageBackground.Text);
                    //}
                    break;
                case 1: //incremental
                    rollback.Type = GBDRollbackItem.RollbackType.Incremental;
                    rollback.Increment = Double.Parse(txtIncrement.Text);
                    rollback.Background = BACKGROUND;
                    //if (!String.IsNullOrEmpty(txtIncrementBackground.Text))
                    //{
                    //    rollback.Background = Double.Parse(txtIncrementBackground.Text);
                    //}
                    break;
                case 2: //standard
                    rollback.Type = GBDRollbackItem.RollbackType.Standard;
                    rollback.StandardName = cboStandard.GetItemText(cboStandard.SelectedItem);
                    rollback.StandardId = (int)cboStandard.SelectedValue;
                    rollback.Standard = GBDRollbackDataSource.GetStandardValue(rollback.StandardId);
                    break;
            }
            rollback.Year = YEAR;
            rollback.Color = GetNextColor();


            //remove rollback if it already exists
            rollbacks.RemoveAll(x => x.Name.Equals(rollback.Name, StringComparison.OrdinalIgnoreCase));

            //add to rollbacks
            rollbacks.Add(rollback);

            //add to grid
            //dgvRollbacks.Rows.Clear();
            //foreach (GBDRollbackItem item in rollbacks)
            //{ 
            //    DataGridViewRow row = new DataGridViewRow();
            //    int i = dgvRollbacks.Rows.Add(row);
            //    dgvRollbacks.Rows[i].Cells["colName"].Value = item.Name;
            //    dgvRollbacks.Rows[i].Cells["colColor"].Style.BackColor = item.Color;
            //    dgvRollbacks.Rows[i].Cells["colTotalCountries"].Value = item.Countries.Count().ToString();
            //    dgvRollbacks.Rows[i].Cells["colTotalPopulation"].Value = GetRollbackTotalPopulation(item).ToString("#,###");
            //    dgvRollbacks.Rows[i].Cells["colRollbackType"].Value = GetRollbackTypeSummary(item);         
            //}

            RemoveGridRow(rollback.Name);
            DataGridViewRow row = new DataGridViewRow();
            int i = dgvRollbacks.Rows.Add(row);
            dgvRollbacks.Rows[i].Cells["colName"].Value = rollback.Name;
            dgvRollbacks.Rows[i].Cells["colColor"].Style.BackColor = rollback.Color;
            dgvRollbacks.Rows[i].Cells["colTotalCountries"].Value = rollback.Countries.Count().ToString();
            dgvRollbacks.Rows[i].Cells["colTotalPopulation"].Value = GetRollbackTotalPopulation(rollback).ToString("#,###");
            dgvRollbacks.Rows[i].Cells["colRollbackType"].Value = GetRollbackTypeSummary(rollback);
            dgvRollbacks.Rows[i].Cells["colExecute"].Value = true;
            ToggleExecuteScenariosButton();


            //update map
            IMapFeatureLayer[] mfl = mapGBD.GetFeatureLayers();
            mfl[0].ClearSelection();
            foreach (IPolygonCategory ipcToRemove in selectedButNotSavedIPCs.Values)
            {
                mfl[0].Symbology.RemoveCategory(ipcToRemove);
            }
            selectedButNotSavedIPCs.Clear();
            IPolygonScheme ips = (IPolygonScheme)mfl[0].Symbology;
            IPolygonCategory ipc = null;
            //grab existing ips and add to it
            foreach(String s in rollback.Countries.Keys){
                ipc = new PolygonCategory(rollback.Color, System.Drawing.Color.Black, 1);
                ipc.FilterExpression = "[ID]='" + s+"'";
                rollback.addIPC(ipc);
                ips.AddCategory(ipc);
            //set color of selected country features on map
            //string filter = "[ID] in (" + String.Join(",", rollback.Countries.Select(x => "'" + x.Key + "'")) + ")";
            //mfl[0].SelectByAttribute(filter, ModifySelectionMode.Subtract);
            //PolygonCategory category = new PolygonCategory(rollback.Color, Color.Black, 4);
            //category.FilterExpression = filter;
            //mfl[0].Symbology.AddCategory(ipc);        
                
            }
            mfl[0].ApplyScheme(ips);

            ClearFields();
            SetActivePanel(0);
           
        }

        private void RemoveGridRow(string name)
        {
            foreach (DataGridViewRow row in dgvRollbacks.Rows)
            {
                string s = row.Cells["colName"].Value.ToString();
                if (s.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    dgvRollbacks.Rows.Remove(row);
                    return;                    
                }            
            }        
        }

        private System.Drawing.Color GetNextColor()
        {
            foreach (System.Drawing.Color c in colorPalette)
            {
                GBDRollbackItem item = rollbacks.Find(x => x.Color.ToArgb() == c.ToArgb());
                if (item == null)
                {
                    return c;
                }             
            
            }

            return GetRandomColor();        
        }

        private System.Drawing.Color GetRandomColor()
        {
            Random random = new Random();
            return System.Drawing.Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }  

        private long GetRollbackTotalPopulation(GBDRollbackItem rollback)
        {
            //build selected list of countries, pops
            string expression = "COUNTRYID in (" + String.Join(",", rollback.Countries.Select(x=> "'" + x.Key + "'")) + ")";
            DataRow[] rows = dtCountries.Select(expression);
            System.Data.DataTable dt = rows.CopyToDataTable<DataRow>();

            long lPop = 0;

            // Declare an object variable. 
            object sumObject;
            sumObject = dt.Compute("Sum(POPULATION)","");
            lPop = Int64.Parse(sumObject.ToString());

            return lPop;



        }

        private string GetRollbackTypeSummary(GBDRollbackItem rollback)
        {
            string summary = String.Empty;
            char micrograms = '\u00B5';
            char super3 = '\u00B3';

            switch (rollback.Type)
            {
                case GBDRollbackItem.RollbackType.Percentage: //percentage
                    summary = rollback.Percentage.ToString() + "% Rollback";
                    break;
                case GBDRollbackItem.RollbackType.Incremental: //incremental
                    summary = rollback.Increment.ToString() + micrograms.ToString() + "g/m" + super3.ToString() + " Rollback";
                    break;
                case GBDRollbackItem.RollbackType.Standard:
                    summary = "Rollback to " + rollback.StandardName + " Standard";
                    break;
            }


            return summary;
        }

        private void ClearFields() 
        {
            //clear fields
            txtName.Text = String.Empty;
            txtDescription.Text = String.Empty;
            selectMapFeaturesOnNodeCheck = false;
            foreach (TreeNode node in tvRegions.Nodes)
            {
                node.Checked = false;
                foreach(TreeNode tn in node.Nodes){
                    tn.Checked=false;
                }
            }
            //IMapFeatureLayer[] mfl = mapGBD.GetFeatureLayers();
            //mfl[0].UnSelectAll();
            selectMapFeaturesOnNodeCheck = true;
            cboRollbackType.SelectedIndex = (int)GBDRollbackItem.RollbackType.Percentage; 
            txtPercentage.Text = String.Empty;
            txtPercentageBackground.Text = String.Empty;
            txtIncrement.Text = String.Empty;
            txtIncrementBackground.Text = String.Empty;
            cboStandard.SelectedIndex = -1;       

        }

        private void LoadRollback(GBDRollbackItem item)
        {
            txtName.Text = item.Name;
            txtDescription.Text = item.Description;
            IMapFeatureLayer[] mfl = mapGBD.GetFeatureLayers();
            //IPCs can be hanging if scenario is edited then a country is deselected.
            foreach (IPolygonCategory ipcOld in item.IpcList)
            {
                mfl[0].Symbology.RemoveCategory(ipcOld);
            }
            item.IpcList.Clear();
            mfl[0].ApplyScheme(mfl[0].Symbology);
            foreach (KeyValuePair<string,string> kvp in item.Countries)
            {
                string countryid = kvp.Key;
                TreeNode[] nodes = tvRegions.Nodes.Find(countryid,true);
                foreach (TreeNode node in nodes)
                {
                    node.Checked = true;
                    CheckParentNode(node);
                }                 
            }
            cboRollbackType.SelectedIndex = (int)item.Type;
            txtPercentage.Text = item.Percentage.ToString();
            txtPercentageBackground.Text = item.Background.ToString();
            txtIncrement.Text = item.Increment.ToString();
            txtIncrementBackground.Text = item.Background.ToString();
            cboStandard.SelectedIndex = (int)item.StandardId;
           
        }

        private void SetActivePanel(int index)
        {
            switch (index)
            {
                case 0:
                    gbName.Visible = true;
                    gbCountrySelection.Visible = false;
                    gbParameterSelection.Visible = false;
                    break;
                case 1:
                    gbName.Visible = false;
                    gbCountrySelection.Visible = true;
                    gbParameterSelection.Visible = false;
                    break;
                case 2:
                    gbName.Visible = false;
                    gbCountrySelection.Visible = false;
                    gbParameterSelection.Visible = true;
                    break;
            }
        }

        private void SetActiveOptionsPanel(int index)
        {
            switch (index)
            {
                case 0:
                    gbOptionsPercentage.Visible = true;
                    gbOptionsIncremental.Visible = false;                    
                    gbOptionsStandard.Visible = false;
                    break;
                case 1:
                    gbOptionsPercentage.Visible = false;
                    gbOptionsIncremental.Visible = true;                    
                    gbOptionsStandard.Visible = false;
                    break;
                case 2:
                    gbOptionsPercentage.Visible = false;
                    gbOptionsIncremental.Visible = false;                    
                    gbOptionsStandard.Visible = true;
                    break;
            }
        }


        private void btnDeleteRollback_Click(object sender, EventArgs e)
        {
            if (dgvRollbacks.SelectedRows.Count > 0)
            {
                IMapFeatureLayer[] mfl = mapGBD.GetFeatureLayers();
                DialogResult result = MessageBox.Show("Are you sure you wish to delete the selected scenario?","", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DataGridViewRow row = dgvRollbacks.SelectedRows[0];
                    string name = row.Cells["colName"].Value.ToString();
                    GBDRollbackItem item = rollbacks.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                    foreach (IPolygonCategory ipc in item.IpcList)
                    {
                        mfl[0].Symbology.RemoveCategory(ipc);
                    }
                    item.IpcList.Clear();
                    mfl[0].ApplyScheme(mfl[0].Symbology);
                    //delete rollback
                    rollbacks.RemoveAll(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                    //delete row
                    dgvRollbacks.Rows.Remove(row);
                    ToggleExecuteScenariosButton();
                }
            
            }
        }

        private void btnEditRollback_Click(object sender, EventArgs e)
        {
            if (dgvRollbacks.SelectedRows.Count > 0)
            { 
                DataGridViewRow row = dgvRollbacks.SelectedRows[0];
                string name = row.Cells["colName"].Value.ToString();
                //get rollback
                GBDRollbackItem item = rollbacks.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                ClearFields();
                LoadRollback(item);
                SetActivePanel(0);
            
            }

        }

        private void dgvRollbacks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvRollbacks.EndEdit();
            if ((e.RowIndex != -1) && (e.ColumnIndex != -1))
            {
                string columnName = dgvRollbacks.Columns[e.ColumnIndex].Name;
                
                if ((columnName.Equals("colTotalCountries", StringComparison.OrdinalIgnoreCase)) ||
                    (columnName.Equals("colTotalPopulation", StringComparison.OrdinalIgnoreCase)))
                {
                    string name = dgvRollbacks.Rows[e.RowIndex].Cells["colName"].Value.ToString();
                    //get rollback
                    GBDRollbackItem item = rollbacks.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                    GBDRollbackCountriesPopulations frm = new GBDRollbackCountriesPopulations();

                    //build selected list of countries, pops
                    string expression = "COUNTRYID in (" + String.Join(",", item.Countries.Select(x => "'" + x.Key + "'")) + ")";
                    DataRow[] rows = dtCountries.Select(expression);
                    System.Data.DataTable dt = rows.CopyToDataTable<DataRow>();
                    frm.CountryPop = dt.Copy();
                    frm.ShowDialog();
                }               
            }

        }

       

        private void btnExecuteRollbacks_Click(object sender, EventArgs e)
        {

            try
            {

                Cursor.Current = Cursors.WaitCursor;

                double beta = 0;
                double se = 0;

                //get pollutant beta, se
                GBDRollbackDataSource.GetPollutantBeta(POLLUTANT_ID, out beta, out se);

                //for each checked rollback...
                List<DataGridViewRow> list = dgvRollbacks.Rows.Cast<DataGridViewRow>().Where(k => Convert.ToBoolean(k.Cells["colExecute"].Value) == true).ToList();
                foreach (DataGridViewRow row in list)
                {                    
                    string name = row.Cells["colName"].Value.ToString();
                    //get rollback
                    GBDRollbackItem item = rollbacks.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                    ExecuteRollback(item, beta, se);                                       
                }
         //       throw new Exception("debug Test");
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Execute Scenarios successful!");
                
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.ToString());
                String user = Environment.GetEnvironmentVariable("username");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\"+user+@"\My Documents\My BenMAP-CE Files\error.txt"))
                {                              
                            file.Write(ex.ToString());
                }
            }


           
        }

        private void ExecuteRollback(GBDRollbackItem rollback, double beta, double se)
        {
            dtConcEntireRollback = null;

            //for each country in rollback...
            foreach (string countryid in rollback.Countries.Keys)
            {
                //get data
                //country incidencerate
                double incrate = GBDRollbackDataSource.GetIncidenceRate(countryid);

                //get baseline concs
                dtConcCountry = null;
                dtConcCountry = GBDRollbackDataSource.GetCountryConcs(countryid, POLLUTANT_ID, YEAR);

                //build schema of entire rollback table
                if (dtConcEntireRollback == null)
                {
                    dtConcEntireRollback = dtConcCountry.Clone();
                    dtConcEntireRollback.Columns.Add("CONCENTRATION_ADJ", dtConcCountry.Columns["CONCENTRATION"].DataType);
                    if (rollback.Type != GBDRollbackItem.RollbackType.Standard)
                    {
                        dtConcEntireRollback.Columns.Add("CONCENTRATION_ADJ_BACK", dtConcCountry.Columns["CONCENTRATION"].DataType);
                    }
                    dtConcEntireRollback.Columns.Add("CONCENTRATION_FINAL", dtConcCountry.Columns["CONCENTRATION"].DataType);
                    dtConcEntireRollback.Columns.Add("CONCENTRATION_DELTA", dtConcCountry.Columns["CONCENTRATION"].DataType);
                    dtConcEntireRollback.Columns.Add("AIR_QUALITY_DELTA", dtConcCountry.Columns["CONCENTRATION"].DataType);
                    dtConcEntireRollback.Columns.Add("KREWSKI", dtConcCountry.Columns["CONCENTRATION"].DataType);
                    dtConcEntireRollback.Columns.Add("KREWSKI_2_5", dtConcCountry.Columns["CONCENTRATION"].DataType);
                    dtConcEntireRollback.Columns.Add("KREWSKI_97_5", dtConcCountry.Columns["CONCENTRATION"].DataType);
                    dtConcEntireRollback.Columns.Add("INCIDENCE_RATE", dtConcCountry.Columns["CONCENTRATION"].DataType);
                    dtConcEntireRollback.Columns.Add("BASELINE_MORTALITY", dtConcCountry.Columns["CONCENTRATION"].DataType);
                }

                //run rollback
                DoRollback(rollback);

                //get concentration delta and population arrays
                double[] concDelta = Array.ConvertAll<DataRow, double>(dtConcCountry.Select(),
                    delegate(DataRow row) { return Convert.ToDouble(row["CONCENTRATION_DELTA"]); });
                double[] population = Array.ConvertAll<DataRow, double>(dtConcCountry.Select(),
                    delegate(DataRow row) { return Convert.ToDouble(row["POPESTIMATE"]); });

                //get results                
                GBDRollbackKrewskiFunction func = new GBDRollbackKrewskiFunction();
                GBDRollbackKrewskiResult result;
                result = func.GBD_math(concDelta, population, incrate, beta, se);
                //add results to dtConcCountry
                dtConcCountry.Columns.Add("KREWSKI", dtConcCountry.Columns["CONCENTRATION"].DataType, result.Krewski.ToString());
                dtConcCountry.Columns.Add("KREWSKI_2_5", dtConcCountry.Columns["CONCENTRATION"].DataType, result.Krewski2_5.ToString());
                dtConcCountry.Columns.Add("KREWSKI_97_5", dtConcCountry.Columns["CONCENTRATION"].DataType, result.Krewski97_5.ToString());
                dtConcCountry.Columns.Add("INCIDENCE_RATE", dtConcCountry.Columns["CONCENTRATION"].DataType, incrate.ToString());
                dtConcCountry.Columns.Add("BASELINE_MORTALITY", dtConcCountry.Columns["CONCENTRATION"].DataType, "INCIDENCE_RATE * POPESTIMATE" );


                //add records to entire rollback dataset
                dtConcEntireRollback.Merge(dtConcCountry, true, MissingSchemaAction.Ignore);

            }                

            //save rollback report using rollback output
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.DisplayAlerts = false;
            SaveRollbackReport(rollback);
            xlApp.Quit();

        }

        private void DoRollback (GBDRollbackItem rollback)
        {
            switch (rollback.Type)
            {
                case GBDRollbackItem.RollbackType.Percentage:
                    DoPercentageRollback(rollback.Percentage, rollback.Background);
                    break;
                case GBDRollbackItem.RollbackType.Incremental:
                    DoIncrementalRollback(rollback.Increment, rollback.Background);
                    break;
                case GBDRollbackItem.RollbackType.Standard:
                    DoRollbackToStandard(rollback.Standard);
                    break;            
            }
        
        }

        private void DoPercentageRollback(double percentage, double background)
        { 
            //rollback
            dtConcCountry.Columns.Add("CONCENTRATION_ADJ", dtConcCountry.Columns["CONCENTRATION"].DataType, "CONCENTRATION - (CONCENTRATION * " + (percentage / 100).ToString() + ")");
            
            //check against background
            dtConcCountry.Columns.Add("CONCENTRATION_ADJ_BACK", dtConcCountry.Columns["CONCENTRATION"].DataType, "IIF(CONCENTRATION_ADJ < " + background + ", " + background + ", CONCENTRATION_ADJ)");

            //get final, keep original values if <= background.
            dtConcCountry.Columns.Add("CONCENTRATION_FINAL", dtConcCountry.Columns["CONCENTRATION"].DataType, "IIF(CONCENTRATION <= " + background + ", CONCENTRATION, CONCENTRATION_ADJ_BACK)");

            //get delta (orig. conc - rolled back conc. (corrected for background)
            dtConcCountry.Columns.Add("CONCENTRATION_DELTA", dtConcCountry.Columns["CONCENTRATION"].DataType, "CONCENTRATION - CONCENTRATION_FINAL");

            //get air quality delta (conc delta * population)
            dtConcCountry.Columns.Add("AIR_QUALITY_DELTA", dtConcCountry.Columns["CONCENTRATION"].DataType, "CONCENTRATION_DELTA * POPESTIMATE");

        }

        private void DoIncrementalRollback(double increment, double background)
        {
            //rollback
            dtConcCountry.Columns.Add("CONCENTRATION_ADJ", dtConcCountry.Columns["CONCENTRATION"].DataType, "CONCENTRATION - " + increment);

            //check against background
            dtConcCountry.Columns.Add("CONCENTRATION_ADJ_BACK", dtConcCountry.Columns["CONCENTRATION"].DataType, "IIF(CONCENTRATION_ADJ < " + background + ", " + background + ", CONCENTRATION_ADJ)");

            //get final, keep original values if <= background.
            dtConcCountry.Columns.Add("CONCENTRATION_FINAL", dtConcCountry.Columns["CONCENTRATION"].DataType, "IIF(CONCENTRATION <= " + background + ", CONCENTRATION, CONCENTRATION_ADJ_BACK)");

            //get delta (orig. conc - rolled back conc. (corrected for background)
            dtConcCountry.Columns.Add("CONCENTRATION_DELTA", dtConcCountry.Columns["CONCENTRATION"].DataType, "CONCENTRATION - CONCENTRATION_FINAL");

            //get air quality delta (conc delta * population)
            dtConcCountry.Columns.Add("AIR_QUALITY_DELTA", dtConcCountry.Columns["CONCENTRATION"].DataType, "CONCENTRATION_DELTA * POPESTIMATE");

        }

        private void DoRollbackToStandard(double standard)
        {
            //rollback to standard
            dtConcCountry.Columns.Add("CONCENTRATION_ADJ", dtConcCountry.Columns["CONCENTRATION"].DataType, standard.ToString());

            //get final, keep original values if <= standard.
            dtConcCountry.Columns.Add("CONCENTRATION_FINAL", dtConcCountry.Columns["CONCENTRATION"].DataType, "IIF(CONCENTRATION <= " + standard + ", CONCENTRATION, CONCENTRATION_ADJ)");

            //get delta (orig. conc - rolled back conc.)
            dtConcCountry.Columns.Add("CONCENTRATION_DELTA", dtConcCountry.Columns["CONCENTRATION"].DataType, "CONCENTRATION - CONCENTRATION_FINAL");

            //get air quality delta (conc delta * population)
            dtConcCountry.Columns.Add("AIR_QUALITY_DELTA", dtConcCountry.Columns["CONCENTRATION"].DataType, "CONCENTRATION_DELTA * POPESTIMATE");
        }

        //private void SaveRollbackReport(GBDRollbackItem rollback)
        //{

        //    //get application path
        //    string appPath = AppDomain.CurrentDomain.BaseDirectory;
        //    string filePath = appPath + @"Tools\GBDRollbackOutputTemplate.xlsx";

        //    Microsoft.Office.Interop.Excel.Workbook xlBook;
        //    //open report template                
        //    xlBook = xlApp.Workbooks.Open(filePath);           

        //    //check save dir 
        //    string resultsDir = txtFilePath.Text.Trim();
        //    if(!Directory.Exists(resultsDir))
        //    {
        //        Directory.CreateDirectory(resultsDir);
        //    }

        //    //get timestamp
        //    DateTime dtNow = DateTime.Now;
        //    string timeStamp = dtNow.ToString("yyyyMMddHHmm");
        //    //get application path
        //    filePath = resultsDir + @"\GBDRollback_" + rollback.Name + "_" + timeStamp + ".xlsx";

        //    #region summary sheet
        //    //summary sheet
        //    Microsoft.Office.Interop.Excel.Worksheet xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets[1];
        //    //xlSheet.Name = "Summary";
        //    //xlSheet.Range["A2"].Value = "Date";
        //    xlSheet.Range["B2"].Value = dtNow.ToString("yyyy/MM/dd");
        //    //xlSheet.Range["A3"].Value = "Scenario Name";
        //    xlSheet.Range["B3"].Value = rollback.Name;
        //    //xlSheet.Range["A4"].Value = "Scenario Description";
        //    xlSheet.Range["B4"].Value = rollback.Description;
        //    //xlSheet.Range["A5"].Value = "GBD Year";
        //    xlSheet.Range["B5"].Value = rollback.Year.ToString();
        //    //xlSheet.Range["A6"].Value = "Pollutant";
        //    xlSheet.Range["B6"].Value = "PM 2.5";

        //    //xlSheet.Range["A7"].Value = "Background Concentration";
        //    char micrograms = '\u00B5';
        //    char super3 = '\u00B3';
        //    xlSheet.Range["B7"].Value = rollback.Background.ToString() + " " + micrograms.ToString() + "g/m" + super3.ToString();

        //    //xlSheet.Range["A8"].Value = "Rollback Type";
        //    string summary = String.Empty;
        //    switch (rollback.Type)
        //    {
        //        case GBDRollbackItem.RollbackType.Percentage: //percentage
        //            summary = rollback.Percentage.ToString() + "% Rollback";
        //            break;
        //        case GBDRollbackItem.RollbackType.Incremental: //incremental
        //            summary = rollback.Increment.ToString() + micrograms.ToString() + "g/m" + super3.ToString() + " Rollback";
        //            break;
        //        case GBDRollbackItem.RollbackType.Standard:
        //            summary = "Rollback to " + rollback.StandardName + " Standard";
        //            break;
        //    }
        //    xlSheet.Range["B8"].Value = summary;

        //    //xlSheet.Range["A9"].Value = "Regions and Countries";
        //    int rowOffset = 0;
        //    int nextRow = 0;

        //    System.Data.DataTable dtTemp = dtConcEntireRollback.DefaultView.ToTable(true,  "REGIONID", "REGIONNAME", "COUNTRYID", "COUNTRYNAME");
        //    DataView dv = new DataView(dtTemp);
        //    dv.Sort = "REGIONNAME ASC, COUNTRYNAME ASC";
        //    System.Data.DataTable dtRegionsCountries = dv.ToTable();
        //    string region = String.Empty;
        //    string country = String.Empty;
        //    foreach (DataRow dr in dtRegionsCountries.Rows)
        //    {
        //        //new region? write region
        //        if (!region.Equals(dr["REGIONNAME"].ToString(), StringComparison.OrdinalIgnoreCase))
        //        {
        //            region = dr["REGIONNAME"].ToString();
        //            nextRow = 9 + rowOffset;
        //            xlSheet.Range["B" + nextRow.ToString()].Value = region;
        //            xlSheet.Range["B" + nextRow.ToString()].Font.Italic = true;
        //            rowOffset++;
        //        }

        //        //write country
        //        country = dr["COUNTRYNAME"].ToString();
        //        nextRow = 9 + rowOffset;
        //        xlSheet.Range["B" + nextRow.ToString()].Value = country;
        //        xlSheet.Range["B" + nextRow.ToString()].ColumnWidth = 40;
        //        xlSheet.Range["B" + nextRow.ToString()].WrapText = true;
        //        xlSheet.Range["B" + nextRow.ToString()].InsertIndent(2);
        //        rowOffset++;
        //    }

        //    //format
        //    Microsoft.Office.Interop.Excel.Range xlRange;
        //    xlRange = (Microsoft.Office.Interop.Excel.Range)(xlSheet.Columns[1]);            
        //    xlRange.AutoFit();
        //    //add borders
        //    //nextRow = 9 + rowOffset;
        //    xlRange = xlSheet.Range["A2:B" + nextRow.ToString()];
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;            
        //    xlRange.Borders.Color = Color.Black;
        //    //bold, color label cells
        //    xlRange = xlSheet.Range["A2:A" + nextRow.ToString()];
        //    xlRange.Font.Bold = true;
        //    xlRange.Interior.Color = xlSheet.Range["A2"].Interior.Color;


        //    xlSheet.Range["J2"].Value = rollback.Year.ToString() + " " + xlSheet.Range["J2"].Value.ToString();


        //    #endregion

        //    //results sheet
        //    #region results sheet
        //    Microsoft.Office.Interop.Excel.Worksheet xlSheet2 = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets[2];
        //    //xlSheet2.Name = "Results";
        //    //xlSheet2.Range["A3"].Value = "Country";
        //    //xlSheet2.Range["B3"].Value = "Population Affected";
        //    //xlSheet2.Range["C3"].Value = "Avoided Deaths (Total)";
        //    //xlSheet2.Range["D3"].Value = "Avoided Deaths (% Population)";
        //    //xlSheet2.Range["E3"].Value = "Min";
        //    //xlSheet2.Range["F3"].Value = "Median";
        //    //xlSheet2.Range["G3"].Value = "Max";
        //    xlSheet2.Range["H2"].Value = rollback.Year.ToString() + " " + xlSheet2.Range["H2"].Value.ToString();
        //    //xlSheet2.Range["E2:G2"].MergeCells = true;
        //    //xlSheet2.Range["H3"].Value = "Min";
        //    //xlSheet2.Range["I3"].Value = "Median";
        //    //xlSheet2.Range["J3"].Value = "Max";
        //    //xlSheet2.Range["H2"].Value = "Control";
        //    //xlSheet2.Range["H2:J2"].MergeCells = true;
        //    //xlSheet2.Range["K3"].Value = "Air Quality Change (Population Weighted)";

        //    //format
        //    //xlSheet2.Range["E2:J2"].Font.Bold = true;
        //    //xlSheet2.Range["E2:J2"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //    //xlSheet2.Range["A3:K3"].Font.Bold = true;
        //    //xlSheet2.Range["A3:K3"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //    //xlSheet2.Range["B3:D3"].ColumnWidth = 20;
        //    //xlSheet2.Range["E3:J3"].ColumnWidth = 10;
        //    //xlSheet2.Range["K3"].ColumnWidth = 20;
        //    //xlSheet2.Range["B3:K3"].WrapText = true;
        //    ////country column
        //    //xlRange = (Microsoft.Office.Interop.Excel.Range)(xlSheet2.Columns[1]);
        //    //xlRange.ColumnWidth = 40;
        //    //xlRange.WrapText = true;

        //    //build output table
        //    System.Data.DataTable dtDetailedResults = new System.Data.DataTable();
        //    dtDetailedResults.Columns.Add("NAME", Type.GetType("System.String"));
        //    dtDetailedResults.Columns.Add("IS_REGION", Type.GetType("System.Boolean"));
        //    dtDetailedResults.Columns.Add("POP_AFFECTED", Type.GetType("System.Double"));
        //    dtDetailedResults.Columns.Add("AVOIDED_DEATHS", Type.GetType("System.Double"));
        //    dtDetailedResults.Columns.Add("CONFIDENCE_INTERVAL", Type.GetType("System.String"));
        //    dtDetailedResults.Columns.Add("PERCENT_BASELINE_MORTALITY", Type.GetType("System.Double"));
        //    dtDetailedResults.Columns.Add("DEATHS_PER_100_THOUSAND", Type.GetType("System.Double"));
        //    dtDetailedResults.Columns.Add("AVOIDED_DEATHS_PERCENT_POP", Type.GetType("System.Double"));
        //    dtDetailedResults.Columns.Add("BASELINE_MIN", Type.GetType("System.Double"));
        //    dtDetailedResults.Columns.Add("BASELINE_MEDIAN", Type.GetType("System.Double"));
        //    dtDetailedResults.Columns.Add("BASELINE_MAX", Type.GetType("System.Double"));
        //    dtDetailedResults.Columns.Add("CONTROL_MIN", Type.GetType("System.Double"));
        //    dtDetailedResults.Columns.Add("CONTROL_MEDIAN", Type.GetType("System.Double"));
        //    dtDetailedResults.Columns.Add("CONTROL_MAX", Type.GetType("System.Double"));
        //    dtDetailedResults.Columns.Add("AIR_QUALITY_CHANGE", Type.GetType("System.Double"));


        //    string regionid = String.Empty;
        //    string countryid = String.Empty;
        //    foreach (DataRow dr in dtRegionsCountries.Rows)
        //    {
        //        //new region? get region data
        //        if (!regionid.Equals(dr["REGIONID"].ToString(), StringComparison.OrdinalIgnoreCase))
        //        {
        //            regionid = dr["REGIONID"].ToString();
        //            GetResults(regionid, dr["REGIONNAME"].ToString(), true, dtDetailedResults);
        //        }

        //        //get country data
        //        countryid = dr["COUNTRYID"].ToString();
        //        GetResults(countryid, dr["COUNTRYNAME"].ToString(), false, dtDetailedResults);
        //    }


        //    //write results to spreadsheet
        //    nextRow = 4;
        //    foreach (DataRow dr in dtDetailedResults.Rows)
        //    {
        //        xlSheet2.Range["A" + nextRow.ToString()].Value = dr["NAME"].ToString();
        //        if (Convert.ToBoolean(dr["IS_REGION"].ToString()))
        //        {
        //            xlSheet2.Range["A" + nextRow.ToString()].Font.Italic = true;
        //        }
        //        else 
        //        {
        //            //xlSheet2.Range["A" + nextRow.ToString()].ColumnWidth = 40;
        //            //xlSheet2.Range["A" + nextRow.ToString()].WrapText = true;
        //            xlSheet2.Range["A" + nextRow.ToString()].InsertIndent(2);                
        //        }
        //        xlSheet2.Range["B" + nextRow.ToString()].Value = FormatDoubleStringTwoSignificantFigures(FORMAT_DECIMAL_0_PLACES, dr["POP_AFFECTED"].ToString());
        //        xlSheet2.Range["C" + nextRow.ToString()].Value = FormatDoubleStringTwoSignificantFigures(FORMAT_DECIMAL_0_PLACES, dr["AVOIDED_DEATHS"].ToString());
        //        xlSheet2.Range["D" + nextRow.ToString()].Value = "'" + dr["CONFIDENCE_INTERVAL"].ToString(); //prepend apostrophe so Excel treats this as text not date
        //        xlSheet2.Range["E" + nextRow.ToString()].Value = dr["PERCENT_BASELINE_MORTALITY"].ToString();
        //        xlSheet2.Range["F" + nextRow.ToString()].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["DEATHS_PER_100_THOUSAND"].ToString());
        //        xlSheet2.Range["G" + nextRow.ToString()].Value = dr["AVOIDED_DEATHS_PERCENT_POP"].ToString();//FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["AVOIDED_DEATHS_PERCENT_POP"].ToString());
        //        xlSheet2.Range["H" + nextRow.ToString()].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["BASELINE_MIN"].ToString());
        //        xlSheet2.Range["I" + nextRow.ToString()].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["BASELINE_MEDIAN"].ToString());
        //        xlSheet2.Range["J" + nextRow.ToString()].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["BASELINE_MAX"].ToString());
        //        xlSheet2.Range["K" + nextRow.ToString()].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["CONTROL_MIN"].ToString());
        //        xlSheet2.Range["L" + nextRow.ToString()].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["CONTROL_MEDIAN"].ToString());
        //        xlSheet2.Range["M" + nextRow.ToString()].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["CONTROL_MAX"].ToString());
        //        xlSheet2.Range["N" + nextRow.ToString()].Value = dr["AIR_QUALITY_CHANGE"].ToString();// FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["AIR_QUALITY_CHANGE"].ToString());
        //        nextRow++;
                
        //    }

        //    //center confidence interval
        //    xlRange = xlSheet2.Range["D4:D" + (nextRow - 1).ToString()];
        //    xlRange.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            
        //    //add cell borders
        //    xlRange = xlSheet2.Range["A4:N" + (nextRow - 1).ToString()];
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    xlRange.Borders.Color = Color.Black;

        //    #endregion

        //    #region back to summary sheet

        //    //get results for summary table
        //    System.Data.DataTable dtSummaryResults = dtDetailedResults.Clone();
        //    GetResults(null, "SUMMARY", false, dtSummaryResults);
        //    if (dtSummaryResults.Rows.Count > 0)
        //    {
        //        DataRow dr = dtSummaryResults.Rows[0];
        //        xlSheet.Range["D4"].Value = FormatDoubleStringTwoSignificantFigures(FORMAT_DECIMAL_0_PLACES, dr["POP_AFFECTED"].ToString());
        //        xlSheet.Range["E4"].Value = FormatDoubleStringTwoSignificantFigures(FORMAT_DECIMAL_0_PLACES, dr["AVOIDED_DEATHS"].ToString());
        //        xlSheet.Range["F4"].Value =  "'" + dr["CONFIDENCE_INTERVAL"].ToString(); //prepend apostrophe so Excel treats this as text not date
        //        xlSheet.Range["G4"].Value = dr["PERCENT_BASELINE_MORTALITY"].ToString();
        //        xlSheet.Range["H4"].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["DEATHS_PER_100_THOUSAND"].ToString());
        //        xlSheet.Range["I4"].Value = dr["AVOIDED_DEATHS_PERCENT_POP"].ToString();//FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["AVOIDED_DEATHS_PERCENT_POP"].ToString());
        //        xlSheet.Range["J4"].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["BASELINE_MIN"].ToString());
        //        xlSheet.Range["K4"].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["BASELINE_MEDIAN"].ToString());
        //        xlSheet.Range["L4"].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["BASELINE_MAX"].ToString());
        //        xlSheet.Range["M4"].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["CONTROL_MIN"].ToString());
        //        xlSheet.Range["N4"].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["CONTROL_MEDIAN"].ToString());
        //        xlSheet.Range["O4"].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["CONTROL_MAX"].ToString());
        //        xlSheet.Range["P4"].Value = dr["AIR_QUALITY_CHANGE"].ToString();// FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["AIR_QUALITY_CHANGE"].ToString());

        //    }
        //    xlRange = xlSheet.Range["D4:P4"];
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    xlRange.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    xlRange.Borders.Color = Color.Black;

        //    #endregion

        //    #region charts

        //    //summary chart
        //    //write summary chart data to hidden sheet
        //    //sheet DataSource is hidden and is the 4th sheet (Metadata is the third sheet)
        //    Microsoft.Office.Interop.Excel.Worksheet xlSheet4 = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets[4];             
        //    int nextRowForSummary = 1;
        //    foreach (DataRow dr in dtDetailedResults.Rows)
        //    {
        //        //only write countries, skip regions
        //        if (!Convert.ToBoolean(dr["IS_REGION"].ToString()))
        //        {
        //            xlSheet4.Range["A" + nextRowForSummary.ToString()].Value = dr["NAME"].ToString();
        //            xlSheet4.Range["B" + nextRowForSummary.ToString()].Value = FormatDoubleString(FORMAT_DECIMAL_2_PLACES, dr["AVOIDED_DEATHS"].ToString());
        //            nextRowForSummary++;                    
        //        }
        //    }
        //    Microsoft.Office.Interop.Excel.ChartObject xlChartObject = (Microsoft.Office.Interop.Excel.ChartObject)xlSheet.ChartObjects(1);
        //    Microsoft.Office.Interop.Excel.Chart xlChart = (Microsoft.Office.Interop.Excel.Chart)xlChartObject.Chart;
        //    Microsoft.Office.Interop.Excel.Series xlSeries = (Microsoft.Office.Interop.Excel.Series)xlChart.SeriesCollection(1);
        //    xlSeries.Values = xlSheet4.Range["B1:B" + (nextRowForSummary - 1).ToString()];
        //    xlSeries.XValues = xlSheet4.Range["A1:A" + (nextRowForSummary - 1).ToString()];
        //    //write to total avoided deaths text box on chart
        //    Microsoft.Office.Interop.Excel.Shape txtBox = (Microsoft.Office.Interop.Excel.Shape)xlSheet.Shapes.Item("TextBox 1");
        //    txtBox.TextFrame.Characters().Text = txtBox.TextFrame.Characters().Text + " " + xlSheet.Range["E4"].Text; //use .Text rather than .Value on the range here, because it is formatted


        //    //avoided deaths chart sheet
        //    xlChart = (Microsoft.Office.Interop.Excel.Chart)xlBook.Charts[1];
        //    xlSeries = (Microsoft.Office.Interop.Excel.Series)xlChart.SeriesCollection(1);
        //    xlSeries.Values = xlSheet2.Range["C4:C" + (nextRow - 1).ToString()];
        //    xlSeries.XValues = xlSheet2.Range["A4:A" + (nextRow - 1).ToString()];

        //    //deaths per 100,000
        //    xlChart = (Microsoft.Office.Interop.Excel.Chart)xlBook.Charts[2];
        //    xlSeries = (Microsoft.Office.Interop.Excel.Series)xlChart.SeriesCollection(1);
        //    xlSeries.Values = xlSheet2.Range["F4:F" + (nextRow - 1).ToString()];
        //    xlSeries.XValues = xlSheet2.Range["A4:A" + (nextRow - 1).ToString()];

        //    #endregion



        //    //save
        //    xlBook.SaveAs(filePath, FileFormat: XlFileFormat.xlOpenXMLWorkbook);
        //    xlBook.Close();       
        
        
        
        //}

        private void SaveRollbackReport(GBDRollbackItem rollback)
        {

            //get application path
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = appPath + @"Tools\GBDRollbackOutputTemplate.xlsx";

            //check save dir 
            string resultsDir = txtFilePath.Text.Trim();
            if (!Directory.Exists(resultsDir))
            {
                Directory.CreateDirectory(resultsDir);
            }

            //get timestamp
            DateTime dtNow = DateTime.Now;
            string timeStamp = dtNow.ToString("yyyyMMddHHmm");
            //get application path
            string filePathCopy = resultsDir + @"\GBDRollback_" + rollback.Name + "_" + timeStamp + ".xlsx";
            //copy template
            File.Copy(filePath, filePathCopy, true);

            //open copied report template             
            spreadsheetDocument = SpreadsheetDocument.Open(filePathCopy, true);
            WorkbookPart workBookPart = spreadsheetDocument.WorkbookPart;
            Workbook workBook = workBookPart.Workbook;
            workBook.Save();
            spreadsheetDocument.Close();

        }

        private string FormatDoubleString(string format, string str)
        {         
            return Double.Parse(str).ToString(format);
        }

        private string FormatDoubleStringTwoSignificantFigures(string format, string str)
        {
            Double dbl = Double.Parse(str);

            if ((dbl > 100) || (dbl < -100))
            {
                //absolute value, log10, floor (round down to nearest int), round function
                //original Excel function: =ROUND(T19,2-1-INT(LOG10(ABS(T19))))

                long numDecimalPlaces = Math.Abs(Convert.ToInt64(1 - Math.Floor(Math.Log10(Math.Abs(dbl)))));

                double tenFactor = Math.Pow(Convert.ToDouble(10), Convert.ToDouble(numDecimalPlaces));

                dbl = dbl / tenFactor;

                dbl = Math.Round(dbl, 0, MidpointRounding.AwayFromZero);

                dbl = dbl * tenFactor;
            }

            return dbl.ToString(format);
        }

        private double Median(IEnumerable<double> list)
        {
            List<double> orderedList = list
                .OrderBy(numbers => numbers)
                .ToList();

            int listSize = orderedList.Count;
            double result;

            if (listSize % 2 == 0) // even
            {
                int midIndex = listSize / 2;
                result = ((orderedList.ElementAt(midIndex - 1) +
                           orderedList.ElementAt(midIndex)) / 2);
            }
            else // odd
            {
                double element = (double)listSize / 2;
                element = Math.Round(element, MidpointRounding.AwayFromZero);

                result = orderedList.ElementAt((int)(element - 1));
            }

            return result;
        }


        private void GetResults(string id, string name, bool isRegion, System.Data.DataTable dt)
        {
            double popAffected;
            double avoidedDeaths;
            double krewski_2_5;
            double krewski_97_5;
            string confidenceInterval;
            double baselineMortality;
            double percentBaselineMortality;
            double deathsPer100Thousand;
            double avoidedDeathsPercentPop;
            double baselineMin;
            double baselineMedian;
            double baselineMax;
            double controlMin;
            double controlMedian;
            double controlMax;
            double airQualityChange;
            object result;

            string filter = string.Empty;
            if (!String.IsNullOrEmpty(id))
            {
                if (isRegion)
                {
                    filter = "REGIONID = " + id;
                }
                else
                {
                    filter = "COUNTRYID = '" + id + "'";
                }
            }
            else
            {
                filter = "1=1"; //no filter (i.e., all rows)
            }

            //population
            result = dtConcEntireRollback.Compute("SUM(POPESTIMATE)", filter);
            popAffected = Double.Parse(result.ToString());

            //baselineMortality
            result = dtConcEntireRollback.Compute("SUM(BASELINE_MORTALITY)", filter);
            baselineMortality = Double.Parse(result.ToString());

            System.Data.DataTable dtKrewski = dtConcEntireRollback.DefaultView.ToTable(true, "REGIONID", "REGIONNAME", "COUNTRYID", "COUNTRYNAME",
                                                                                            "KREWSKI", "KREWSKI_2_5", "KREWSKI_97_5");
            dtKrewski.DefaultView.Sort = "REGIONNAME, COUNTRYNAME";

            //avoided deaths
            result = dtKrewski.Compute("SUM(KREWSKI)", filter);
            avoidedDeaths = Double.Parse(result.ToString());
            
            //confidence interval
            result = dtKrewski.Compute("SUM(KREWSKI_2_5)", filter);
            krewski_2_5 = Double.Parse(result.ToString());
            result = dtKrewski.Compute("SUM(KREWSKI_97_5)", filter);
            krewski_97_5 = Double.Parse(result.ToString());
            confidenceInterval = FormatDoubleStringTwoSignificantFigures(FORMAT_DECIMAL_0_PLACES, krewski_2_5.ToString()) + " - " + FormatDoubleStringTwoSignificantFigures(FORMAT_DECIMAL_0_PLACES, krewski_97_5.ToString());

            //percent baseline mortality
            percentBaselineMortality = (avoidedDeaths / baselineMortality) * 100;

            //deaths per 100,000
            deathsPer100Thousand = avoidedDeaths/(popAffected/100000);

            //avoided deaths percent pop
            avoidedDeathsPercentPop = (avoidedDeaths / popAffected) * 100;
            
            //baseline min, median, max
            result = dtConcEntireRollback.Compute("MIN(CONCENTRATION)", filter);
            baselineMin = Double.Parse(result.ToString());

            double[] concBase = Array.ConvertAll<DataRow, double>(dtConcEntireRollback.Select(filter),
                            delegate(DataRow row) { return Convert.ToDouble(row["CONCENTRATION"]); });
            baselineMedian = Median(concBase.ToList<double>());

            result = dtConcEntireRollback.Compute("MAX(CONCENTRATION)", filter);
            baselineMax = Double.Parse(result.ToString());

            //control min, median, max
            result = dtConcEntireRollback.Compute("MIN(CONCENTRATION_FINAL)", filter);
            controlMin = Double.Parse(result.ToString());

            double[] concControl = Array.ConvertAll<DataRow, double>(dtConcEntireRollback.Select(filter),
                             delegate(DataRow row) { return Convert.ToDouble(row["CONCENTRATION_FINAL"]); });
            controlMedian = Median(concControl.ToList<double>());

            result = dtConcEntireRollback.Compute("MAX(CONCENTRATION_FINAL)", filter);
            controlMax = Double.Parse(result.ToString());

            //air quality delta
            result = dtConcEntireRollback.Compute("SUM(AIR_QUALITY_DELTA)", filter);
            airQualityChange = Double.Parse(result.ToString());
            airQualityChange = airQualityChange / popAffected;

            DataRow dr = dt.NewRow();
            dr["NAME"] = name;
            dr["IS_REGION"] = isRegion;
            dr["POP_AFFECTED"] = popAffected;
            dr["AVOIDED_DEATHS"] = avoidedDeaths;
            dr["CONFIDENCE_INTERVAL"] = confidenceInterval;
            dr["PERCENT_BASELINE_MORTALITY"] = percentBaselineMortality;
            dr["DEATHS_PER_100_THOUSAND"] = deathsPer100Thousand;
            dr["AVOIDED_DEATHS_PERCENT_POP"] = avoidedDeathsPercentPop;
            dr["BASELINE_MIN"] = baselineMin;
            dr["BASELINE_MEDIAN"] = baselineMedian;
            dr["BASELINE_MAX"] = baselineMax;
            dr["CONTROL_MIN"] = controlMin;
            dr["CONTROL_MEDIAN"] = controlMedian;
            dr["CONTROL_MAX"] = controlMax;
            dr["AIR_QUALITY_CHANGE"] = airQualityChange;

            dt.Rows.Add(dr);
        
        }
       

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            mapGBD.FunctionMode = FunctionMode.ZoomIn;
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            mapGBD.FunctionMode = FunctionMode.ZoomOut;
        }

        private void btnPan_Click(object sender, EventArgs e)
        {
            mapGBD.FunctionMode = FunctionMode.Pan;
        }

        private void btnFullExtent_Click(object sender, EventArgs e)
        {
            mapGBD.ZoomToMaxExtent();
            mapGBD.FunctionMode = FunctionMode.None;
        }

        private void btnIdentify_Click(object sender, EventArgs e)
        {
            mapGBD.FunctionMode = FunctionMode.Info;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.RootFolder = Environment.SpecialFolder.txtFilePath.Text.Trim();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFilePath.Text = fbd.SelectedPath;
               
            }


        }

        private void ToggleExecuteScenariosButton()
        {
            List<DataGridViewRow> list = dgvRollbacks.Rows.Cast<DataGridViewRow>().Where(k => Convert.ToBoolean(k.Cells["colExecute"].Value) == true).ToList();            
            btnExecuteRollbacks.Enabled = (list.Count > 0);
        }

        private void dgvRollbacks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvRollbacks.EndEdit();
            if ((e.RowIndex != -1) && (e.ColumnIndex != -1))
            {
                string columnName = dgvRollbacks.Columns[e.ColumnIndex].Name;

                if (columnName.Equals("colExecute", StringComparison.OrdinalIgnoreCase))
                {
                    ToggleExecuteScenariosButton();
                }
            }
        }


        private void ToggleRegionsCountries()
        {
            if (rbRegions.Checked)
            {
                tvRegions.Visible = true;
                listCountries.Visible = false;
            }
            else
            {
                tvRegions.Visible = false;
                listCountries.Visible = true;
            }
        
        }

        private void rbRegions_CheckedChanged(object sender, EventArgs e)
        {
            ToggleRegionsCountries();
        }

        private void rbCountries_CheckedChanged(object sender, EventArgs e)
        {
            ToggleRegionsCountries();
        }

        private void listCountries_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //if countries is visible then synchronize the check state
            //on the regions/countries tree view
            //otherwise ignore to avoid a feedback loop because the user is checking/unchecking on 
            //the regions/countries tree view and the check state will be sychronized from 
            //tvCountries_AfterCheck event. 
            if (listCountries.Visible)
            {
                //check country in regions tree view
                CountryItem item = (CountryItem)listCountries.Items[e.Index];
                TreeNode[] nodes = tvRegions.Nodes.Find(item.Id, true);
                bool IsChecked = (e.NewValue == CheckState.Checked);
                foreach (TreeNode node in nodes)
                {
                    node.Checked = IsChecked;
                    CheckParentNode(node);
                }
            }

        }


       



       
    }
}
