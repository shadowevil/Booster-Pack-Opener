using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoosterPackMaker
{
    public partial class Form1 : Form
    {
        public Dictionary<string, Pack> packs;
        public DataClass dataClass;
        public List<string> globalIDList;

        public Timer connectionChecker;

        PlaceholderText plcName_short;
        PlaceholderText plcName;
        PlaceholderText plcDescription;
        PlaceholderTextNumbersOnly plcRelease;
        PlaceholderTextNumbersOnly plcCardNum;
        PlaceholderTextNumbersOnly plcRare_Chance;
        PlaceholderTextNumbersOnly plcSuperRare_Chance;
        PlaceholderTextNumbersOnly plcUltraRare_Chance;

        private string lastSavedLocation;

        public Form1()
        {
            Application.EnableVisualStyles();
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            dataGridView1.Columns.Add("cardID", "ID");
            dataGridView1.Columns.Add("cardName", "Name");
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            dataGridView1.CellMouseClick += DataGridView1_CellMouseClick;
            dataGridView1.AllowUserToAddRows = false;
            packs = new Dictionary<string, Pack>();

            globalIDList = new List<string>();
            txtSearch.KeyPress += TxtSearch_KeyPress;

            connectionChecker = new Timer();
            connectionChecker.Tick += ConnectionChecker_Tick;
            connectionChecker.Interval = 100;
            connectionChecker.Enabled = true;
            connectionChecker.Start();
            dataClass = new DataClass(".\\expansions\\cards.cdb", true);

            plcName_short = new PlaceholderText(ref txtName_short, "Boosterpack Abreviation...");
            plcName = new PlaceholderText(ref txtName, "Boosterpack Name...");
            plcDescription = new PlaceholderText(ref txtDescription, "Boosterpack Description...");
            plcRelease = new PlaceholderTextNumbersOnly(ref txtRelease, "Boosterpack Release Date...");
            plcCardNum = new PlaceholderTextNumbersOnly(ref txtCardNum, "Number of cards per boosterpack...", 99);
            plcRare_Chance = new PlaceholderTextNumbersOnly(ref txtRareChance, "Percentage of Rare card...", 100);
            plcSuperRare_Chance = new PlaceholderTextNumbersOnly(ref txtSuperRareChance, "Percentage of Super Rare Card...", 100);
            plcUltraRare_Chance = new PlaceholderTextNumbersOnly(ref txtUltraRareChance, "Percentage of Ultra Rare Card...", 100);
            lblStatus.Text = "Idle";
        }

        private void TxtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                searchCards();
            }
        }

        private void ConnectionChecker_Tick(object sender, EventArgs e)
        {
            if (dataClass == null)
            {
                lblConnection.BackColor = Color.IndianRed;
                lblConnection.Text = "Disconnected";
                return;
            }
            if(dataClass.state == ConnectionState.Connecting)
            {
                lblConnection.BackColor = Color.GreenYellow;
                lblConnection.Text = "Connecting...";
                return;
            }
            if(dataClass.state == ConnectionState.Open)
            {
                lblConnection.BackColor = Color.ForestGreen;
                lblConnection.Text = "Connected";
            }
            if (treeView1 == null) return;
            foreach(var item in packs)
            {
                if (item.Value.needsTreeUpdate)
                {
                    int index = cboActive.FindStringExact(item.Key);
                    if (index >= 0) cboActive.Items.RemoveAt(index);
                    item.Value.WriteToTreeView(ref treeView1, item.Key);
                    cboActive.Items.Add(item.Key);
                    cboActive.SelectedIndex = cboActive.FindStringExact(item.Key);
                    globalIDList.Clear();
                    globalIDList.AddRange(item.Value.Common);
                    globalIDList.AddRange(item.Value.Rare);
                    globalIDList.AddRange(item.Value.SuperRare);
                    globalIDList.AddRange(item.Value.UltraRare);
                }
            }
        }

        private void DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                var hitTest = dataGridView1.HitTest(PointToClient(MousePosition).X, PointToClient(MousePosition).Y - 50).RowIndex;
                if (hitTest == -1) return;
                dataGridView1.ClearSelection();
                dataGridView1.Rows[hitTest].Selected = true;
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(new MenuItem("Add to Common list (" + GetSelectedCardCount(0) + ")", AddToCommonList_Click));
                contextMenu.MenuItems.Add(new MenuItem("Add to Rare list(" + GetSelectedCardCount(1) + ")", AddToRareList_Click));
                contextMenu.MenuItems.Add(new MenuItem("Add to Super Rare list(" + GetSelectedCardCount(2) + ")", AddToSuperRareList_Click));
                contextMenu.MenuItems.Add(new MenuItem("Add to Ultra Rare list(" + GetSelectedCardCount(3) + ")", AddToUltraRareList_Click));
                contextMenu.Show(dataGridView1, new Point(PointToClient(MousePosition).X, PointToClient(MousePosition).Y - 50));
            }
        }

        private void AddToUltraRareList_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection _rowCollection = dataGridView1.SelectedRows;
            if (_rowCollection.Count <= 0) return;
            DataGridViewCellCollection _row = _rowCollection[0].Cells;
            AddToList(_row[0].Value.ToString(), _row[1].Value.ToString(), 3);
        }

        private void AddToSuperRareList_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection _rowCollection = dataGridView1.SelectedRows;
            if (_rowCollection.Count <= 0) return;
            DataGridViewCellCollection _row = _rowCollection[0].Cells;
            AddToList(_row[0].Value.ToString(), _row[1].Value.ToString(), 2);
        }

        private void AddToRareList_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection _rowCollection = dataGridView1.SelectedRows;
            if (_rowCollection.Count <= 0) return;
            DataGridViewCellCollection _row = _rowCollection[0].Cells;
            AddToList(_row[0].Value.ToString(), _row[1].Value.ToString(), 1);
        }

        private void AddToCommonList_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection _rowCollection = dataGridView1.SelectedRows;
            if (_rowCollection.Count <= 0) return;
            DataGridViewCellCollection _row = _rowCollection[0].Cells;
            AddToList(_row[0].Value.ToString(), _row[1].Value.ToString(), 0);
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection _rowCollection = dataGridView1.SelectedRows;
            if (_rowCollection.Count <= 0) return;
            DataGridViewCellCollection _row = _rowCollection[0].Cells;
            if (_row[0].Value == null) return;
            string cardID = _row[0].Value.ToString();
            string path = ".\\pics\\" + cardID + ".jpg";
            if (File.Exists(path))
            {
                Bitmap bmp = new Bitmap(path);
                pictureBox1.Image = bmp;
            }
            lblStatus.Text = "Loaded image from (" + path + ")";
        }

        public void PopulateList(Dictionary<int, KeyValuePair<string, string>> entries)
        {
            dataGridView1.Rows.Clear();
            foreach (var entry in entries)
            {
                dataGridView1.Rows.Add(entry.Value.Key.ToString(), entry.Value.Value.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewSelectedRowCollection _rowCollection = dataGridView1.SelectedRows;
            if (_rowCollection.Count <= 0) return;
            DataGridViewCellCollection _row = _rowCollection[0].Cells;
            string cardID = _row[0].Value.ToString();
            string path = ".\\pics\\" + cardID + ".jpg";
            if (File.Exists(path))
            {
                Bitmap bmp = new Bitmap(path);
                pictureBox1.Image = bmp;
            }
        }

        private void searchCards()
        {
            if (dataClass == null) return;
            Dictionary<int, KeyValuePair<string, string>> tmpEntry = new Dictionary<int, KeyValuePair<string, string>>();
            string str = txtSearch.Text.Replace("\'", "\'\'");
            string query = "PRAGMA case_sensitive_like = false;\nSELECT id, name FROM texts WHERE name LIKE ('%" + str + "%');";
            DataTable tb = dataClass.selectQuery(query);
            int count = 0;
            foreach (DataRow row in tb.Rows)
            {
                string ID = null;
                string Name = null;
                int step = 0;
                foreach (object obj in row.ItemArray)
                {
                    if (obj == null) continue;
                    switch (step)
                    {
                        case 0: // ID
                            ID = obj.ToString();
                            break;
                        case 1: // Name
                            Name = obj.ToString();
                            break;
                    }
                    step++;
                }
                tmpEntry.Add(count, new KeyValuePair<string, string>());
                tmpEntry[count] = new KeyValuePair<string, string>(ID, Name);
                count++;
            }
            lblStatus.Text = "Searched for \"" + txtSearch.Text + "\" Results: (" + tmpEntry.Count + ")";
            PopulateList(tmpEntry);
            tmpEntry.Clear();
        }

        public bool AddToList(string ID, string Name, int rarity)
        {
            string statusUpdate = "Card added to ";
            foreach (string id in globalIDList)
            {
                if (id == ID)
                {
                    lblStatus.Text = "ERROR: \"" + Name + "\"(" + ID + ") - Already exists in the global list.";
                    return false;
                }
            }

            if (cboActive.SelectedIndex == -1) return false;
            string selected = cboActive.Items[cboActive.SelectedIndex].ToString();

            globalIDList.Add(ID);
            switch (rarity)
            {
                case 3:     // UltraRare
                    statusUpdate += "[" + selected + "] Ultra Rare List (" + ID + ") " + Name;
                    packs.Where(x => x.Key == selected).First().Value.UltraRare.Add(ID);
                    break;
                case 2:     // SuperRare
                    statusUpdate += "[" + selected + "] Super Rare List (" + ID + ") " + Name;
                    packs.Where(x => x.Key == selected).First().Value.SuperRare.Add(ID);
                    break;
                case 1:     // Rare
                    statusUpdate += "[" + selected + "] Rare List (" + ID + ") " + Name;
                    packs.Where(x => x.Key == selected).First().Value.Rare.Add(ID);
                    break;
                case 0:     // Common
                default:
                    statusUpdate += "[" + selected + "] Common List (" + ID + ") " + Name;
                    packs.Where(x => x.Key == selected).First().Value.Common.Add(ID);
                    break;
            }
            lblStatus.Text = statusUpdate;
            packs.Where(x => x.Key == selected).First().Value.needsTreeUpdate = true;
            return true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "json files (*.json)|*.json|All Files (*.*)|*.*";
            ofd.Multiselect = false;
            ofd.Title = "Open a saved json file associated with BoosterPacks";
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            DialogResult dr = ofd.ShowDialog();
            if (dr != DialogResult.OK) return;
            try
            {
                treeView1.Nodes.Clear();
                tsmQuickSave.Enabled = true;
                tsmSave.Enabled = true;
                tsmSaveAs.Enabled = true;
                txtSearch.Enabled = true;
                cboActive.Enabled = true;
                txtName_short.Enabled = true;
                txtName.Enabled = true;
                txtDescription.Enabled = true;
                txtRelease.Enabled = true;
                txtCardNum.Enabled = true;
                txtRareChance.Enabled = true;
                txtSuperRareChance.Enabled = true;
                txtUltraRareChance.Enabled = true;
                btnNew.Enabled = true;
                packs = JsonConvert.DeserializeObject<Dictionary<string, Pack>>(File.ReadAllText(ofd.FileName));
                foreach(var item in packs)
                {
                    item.Value.needsTreeUpdate = true;
                    //item.Value.WriteToTreeView(ref treeView1, item.Key);
                }
            }
            catch(Exception)
            {
                lblStatus.Text = "ERROR reading JSON file, ensure it is compatible.";
                return;
            }
            lblStatus.Text = "File loaded: " + Path.GetFileName(ofd.FileName);
            lastSavedLocation = ofd.FileName;
        }

        private void cboActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
            int index = cboActive.SelectedIndex;
            string indexText = cboActive.GetItemText(cboActive.Items[index]);
            TreeNode indexNode = treeView1.Nodes.Find(indexText, false)[0];
            indexNode.Expand();
            lblStatus.Text = "Booster Pack loaded [" + indexText + "]";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (plcName_short.isPlaceholder(txtName_short))
            {
                lblStatus.Text = "ERROR Pack short name either empty or invalid";
                return;
            }
            if (treeView1.Nodes.Find(txtName_short.Text, false).Length > 0)
            {
                lblStatus.Text = "ERROR Pack short name already exists";
                return;
            }
            if (plcName.isPlaceholder(txtName))
            {
                lblStatus.Text = "ERROR Pack name is empty or Invalid";
                return;
            }
            if (plcDescription.isPlaceholder(txtDescription))
            {
                lblStatus.Text = "ERROR Description is empty or invalid";
                return;
            }
            if (plcRelease.isPlaceholder(txtRelease))
            {
                lblStatus.Text = "ERROR Release date is empty or invalid";
                return;
            }
            if (plcCardNum.isPlaceholder(txtCardNum))
            {
                lblStatus.Text = "ERROR Card number is empty or invalid";
                return;
            }
            if (plcRare_Chance.isPlaceholder(txtRareChance))
            {
                lblStatus.Text = "ERROR Rare Chance is empty or invalid";
                return;
            }
            if (plcSuperRare_Chance.isPlaceholder(txtSuperRareChance))
            {
                lblStatus.Text = "ERROR Super Rare Chance is empty or invalid";
                return;
            }
            if (plcUltraRare_Chance.isPlaceholder(txtUltraRareChance))
            {
                lblStatus.Text = "ERROR Ultra Rare Chance is empty or invalid";
                return;
            }

            Pack p = new Pack();
            p.name = txtName.Text.Split(',').ToList();
            p.description = txtDescription.Text;
            p.release = txtRelease.Text;
            p.Cards = Convert.ToInt32(txtCardNum.Text);
            p.Common = new List<string>();
            p.Rare = new List<string>();
            p.SuperRare = new List<string>();
            p.UltraRare = new List<string>();
            p.R = Convert.ToInt32(txtRareChance.Text);
            p.SR = Convert.ToInt32(txtSuperRareChance.Text);
            p.UR = Convert.ToInt32(txtUltraRareChance.Text);
            p.needsTreeUpdate = true;
            packs.Add(txtName_short.Text.ToUpper(), p);

            ConnectionChecker_Tick(sender, e);
            cboActive.SelectedIndex = cboActive.FindStringExact(txtName_short.Text.ToUpper());
            lblStatus.Text = "Booster pack [" + txtName_short.Text.ToUpper() + "] created.";
        }

        private int GetSelectedCardCount(int rarity)
        {
            if (cboActive.SelectedIndex == -1) return -1;
            string selected = cboActive.Items[cboActive.SelectedIndex].ToString();

            switch(rarity)
            {
                case 3:     // Ultra Rare
                    return packs.Where(x => x.Key == selected).First().Value.UltraRare.Count;
                case 2:     // Super Rare
                    return packs.Where(x => x.Key == selected).First().Value.SuperRare.Count;
                case 1:     // Rare
                    return packs.Where(x => x.Key == selected).First().Value.Rare.Count;
                case 0:     // Common
                default:
                    return packs.Where(x => x.Key == selected).First().Value.Common.Count;
            }
        }

        private void tsmQuickSave_Click(object sender, EventArgs e)
        {
            if (!tsmQuickSave.Enabled) return;
            string json = JsonConvert.SerializeObject(packs, Formatting.Indented);
            File.WriteAllText(lastSavedLocation, json);
            lblStatus.Text = "Quick saved to " + lastSavedLocation;
        }
    }

    public class DataClass
    {
        private SQLiteConnection sqlite;
        private bool _keepAlive;
        public bool error { get; private set; }

        public DataClass(string path, bool keepAlive = false)
        {
            if (!File.Exists(path))
            {
                error = true;
                return;
            }
            sqlite = new SQLiteConnection("Data Source=" + path);
            _keepAlive = keepAlive;
            sqlite.Open();
        }

        public ConnectionState state
        {
            get
            {
                if (sqlite == null) return ConnectionState.Broken;
                return sqlite.State;
            }
        }

        public DataTable selectQuery(string query)
        {
            if (sqlite == null) return null;
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                cmd = sqlite.CreateCommand();
                cmd.CommandText = query;
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt);
            }
            catch (SQLiteException)
            {
                //Add your exception code here.
            }
            if (!_keepAlive) sqlite.Close();
            return dt;
        }

        public void Close()
        {
            if (sqlite == null) return;
            sqlite.Close();
        }
    }

    public class PlaceholderText
    {
        private string _placeholderTxt;
        public int MAXNUMBER = -1;

        public PlaceholderText(ref TextBox txtBox, string placeholderText)
        {
            txtBox.ForeColor = Color.DarkSlateGray;
            txtBox.Text = placeholderText;
            _placeholderTxt = placeholderText;

            txtBox.GotFocus += _txtBox_GotFocus;
            txtBox.LostFocus += TxtBox_LostFocus;
        }

        private void TxtBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
            {
                (sender as TextBox).ForeColor = Color.DarkSlateGray;
                (sender as TextBox).Text = _placeholderTxt;
                return;
            }
            if (MAXNUMBER == -1) return;
            if (Convert.ToInt32((sender as TextBox).Text) > MAXNUMBER)
            {
                (sender as TextBox).Text = Convert.ToString(MAXNUMBER);
            }
        }

        private void _txtBox_GotFocus(object sender, EventArgs e)
        {
            if((sender as TextBox).Text == _placeholderTxt)
            {
                (sender as TextBox).ForeColor = Color.Black;
                (sender as TextBox).Text = "";
            }
        }

        public bool isPlaceholder(object sender)
        {
            return (sender as TextBox).Text == _placeholderTxt;
        }
    }

    public class PlaceholderTextNumbersOnly : PlaceholderText
    {
        public PlaceholderTextNumbersOnly(ref TextBox txtBox, string placeholderText, int MaxNumber = -1) : base(ref txtBox, placeholderText)
        {
            MAXNUMBER = MaxNumber;
            txtBox.KeyPress += TxtBox_KeyPress;
        }

        private void TxtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }
    }

    public class Pack
    {
        public List<string> name { get; set; }
        public string description { get; set; }
        public string release { get; set; }

        [JsonProperty("#cards")]
        public int Cards { get; set; }
        public List<string> Common { get; set; }
        public List<string> Rare { get; set; }

        [JsonProperty("Super Rare")]
        public List<string> SuperRare { get; set; }

        [JsonProperty("Ultra Rare")]
        public List<string> UltraRare { get; set; }

        [JsonProperty("%R")]
        public int R { get; set; }

        [JsonProperty("%SR")]
        public int SR { get; set; }

        [JsonProperty("%UR")]
        public int UR { get; set; }

        public bool needsTreeUpdate { get; set; } = false;
        public void WriteToTreeView(ref TreeView tree, string key)
        {
            if (tree.Nodes.Find(key, false).Length > 0) tree.Nodes.Remove(tree.Nodes.Find(key, false)[0]);
            TreeNode keyNode = tree.Nodes.Add(key, key);
            TreeNode nameNode = keyNode.Nodes.Add("name");
            TreeNode descriptionNode = keyNode.Nodes.Add("description");
            TreeNode releaseNode = keyNode.Nodes.Add("release");
            TreeNode cardsNode = keyNode.Nodes.Add("#cards");
            TreeNode commonNode = keyNode.Nodes.Add("Common");
            TreeNode rareNode = keyNode.Nodes.Add("Rare");
            TreeNode superRareNode = keyNode.Nodes.Add("Super Rare");
            TreeNode ultraRareNode = keyNode.Nodes.Add("Ultra Rare");
            TreeNode rareChanceNode = keyNode.Nodes.Add("Rare chance");
            TreeNode superRareChanceNode = keyNode.Nodes.Add("Super Rare Chance");
            TreeNode ultraRareChanceNode = keyNode.Nodes.Add("Ultra Rare Chance");
            foreach (string name in name)
                nameNode.Nodes.Add(name);
            descriptionNode.Text += ": \"" + description + "\"";
            releaseNode.Text += ": \"" + release + "\"";
            cardsNode.Text += ": " + Cards;
            int i = 0;
            TreeNode _tmpNode = null;
            foreach (string id in Common)
            {
                if (i % 9 == 0) _tmpNode = commonNode.Nodes.Add(id);
                else _tmpNode.Text += ", " + id;
                i++;
            }
            i = 0; _tmpNode = null;
            foreach (string id in Rare)
            {
                if (i % 9 == 0) _tmpNode = rareNode.Nodes.Add(id);
                else _tmpNode.Text += ", " + id;
                i++;
            }
            i = 0; _tmpNode = null;
            foreach (string id in SuperRare)
            {
                if (i % 9 == 0) _tmpNode = superRareNode.Nodes.Add(id);
                else _tmpNode.Text += ", " + id;
                i++;
            }
            i = 0; _tmpNode = null;
            foreach (string id in UltraRare)
            {
                if (i % 9 == 0) _tmpNode = ultraRareNode.Nodes.Add(id);
                else _tmpNode.Text += ", " + id;
                i++;
            }
            rareChanceNode.Text += ": " + R;
            superRareChanceNode.Text += ": " + SR;
            ultraRareChanceNode.Text += ": " + UR;
            needsTreeUpdate = false;
        }
    }
}
