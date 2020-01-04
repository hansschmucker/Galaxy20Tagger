using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Galaxy2_Tagger
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        public Galaxy2 galaxy2 = null;
        public List<Game> Games = null;
        public List<string> Tags = new List<string>();

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("This is still highly experimental. DO NOT PROCEED unless you have a backup of your C:\\ProgramData\\GOG.com\\Galaxy\\storage directory. Also make sure that Galaxy 2.0 is NOT running. Especially not in the icon bar.");
            var proc = Process.GetProcessesByName("GalaxyClient");
            if (proc.Length>0)
            {
                MessageBox.Show("I said make sure Galaxy is not running! Closing.");
                Application.Exit();
                return;
            }
            else
            {
                galaxy2 = new Galaxy2();

                Games = galaxy2.ReadGames();

                for (var i = 0; i < Games.Count; i++)
                {
                    GamesList.Items.Add(Games[i]);
                    for (var j = 0; j < Games[i].tags.Length; j++)
                        if (!Tags.Contains(Games[i].tags[j].Trim()))
                            Tags.Add(Games[i].tags[j].Trim());
                }

                Tags.Sort();

                for (var i = 0; i < Tags.Count; i++)
                {
                    var cb = new CheckBox
                    {
                        Text = Tags[i],
                        Left = 240,
                        Width = 240,
                        Top = 16 + 24 * i,
                        Appearance = Appearance.Normal,
                        ThreeState = true
                    };
                    TagBoxes.Add(Tags[i], cb);
                    Controls.Add(cb);
                }
            }
        }

        public Dictionary<string, CheckBox> TagBoxes = new Dictionary<string, CheckBox>();

        private void GamesList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (GamesList.SelectedItems == null)
                return;

            var selectedGames = new List<Game>();
            for (var i = 0; i < GamesList.SelectedItems.Count; i++)
                selectedGames.Add((Game)GamesList.SelectedItems[i]);

            var selectedCount = new Dictionary<string, int>();
            for(var i = 0; i < selectedGames.Count; i++)
            {
                for(var j = 0; j < selectedGames[i].tags.Length; j++)
                {
                    if (!selectedCount.ContainsKey(selectedGames[i].tags[j]))
                        selectedCount.Add(selectedGames[i].tags[j],0);

                    selectedCount[selectedGames[i].tags[j]]++;
                }
            }
            foreach(var i in TagBoxes)
            {
                if (!selectedCount.ContainsKey(i.Key))
                    i.Value.CheckState = CheckState.Unchecked;
                else if (selectedCount[i.Key] == selectedGames.Count)
                    i.Value.CheckState = CheckState.Checked;
                else
                    i.Value.CheckState = CheckState.Indeterminate;

                i.Value.Tag = i.Value.CheckState;
            }
        }

        private void DoSaveChanges(object sender, EventArgs e)
        {

            var selectedGames = new List<Game>();
            for (var i = 0; i < GamesList.SelectedItems.Count; i++)
                selectedGames.Add((Game)GamesList.SelectedItems[i]);

            foreach (var i in TagBoxes)
            {
                if ((CheckState)i.Value.Tag != i.Value.CheckState && i.Value.CheckState!=CheckState.Indeterminate)
                {
                    for(var j = 0; j < selectedGames.Count; j++)
                    {
                        if ( selectedGames[j].tags.Contains(i.Value.Text) && i.Value.CheckState == CheckState.Unchecked)
                        {
                            selectedGames[j].RemoveTag(i.Value.Text);
                        }
                        else if(!selectedGames[j].tags.Contains(i.Value.Text) && i.Value.CheckState == CheckState.Checked)
                        {
                            selectedGames[j].AddTag(i.Value.Text);
                        }
                    }
                }
                i.Value.Tag = i.Value.CheckState;
            }
            
        }
    }
}
