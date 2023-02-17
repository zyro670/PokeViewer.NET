using PokeViewer.NET.Properties;

namespace PokeViewer.NET.SubForms
{
    public partial class EggViewerConditions : Form
    {
        public EggViewerConditions()
        {
            InitializeComponent();
            LoadDefaults();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Settings.Default.StopOnShiny = StopOnShiny.Checked;
            Settings.Default.SegmentOrFamily = CheckBoxOf3.Checked;
            Settings.Default.GenderFilter = GenderBox.SelectedIndex;
            Settings.Default.HPFilter = (int)HPFilter.Value;
            Settings.Default.AtkFilter = (int)AtkFilter.Value;
            Settings.Default.DefFilter = (int)DefFilter.Value;
            Settings.Default.SpaFilter = (int)SpaFilter.Value;
            Settings.Default.SpdFilter = (int)SpdFilter.Value;
            Settings.Default.SpeFilter = (int)SpeFilter.Value;
            Settings.Default.IgnoreIVFilter = IgnoreIVFilter.Checked;
            Settings.Default.PresetIVS = PresetIVBox.SelectedIndex;

            Settings.Default.Save();
            this.Close();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            StopOnShiny.Checked = false;
            CheckBoxOf3.Checked = false;
            GenderBox.SelectedIndex = 3;
            HPFilter.Value = 0;
            AtkFilter.Value = 0;
            DefFilter.Value = 0;
            SpaFilter.Value = 0;
            SpdFilter.Value = 0;
            SpeFilter.Value = 0;
            IgnoreIVFilter.Checked = true;
            PresetIVBox.SelectedIndex = 0;
            MessageBox.Show("Stop Conditions have been reset to default values.");
        }

        private void LoadDefaults()
        {
            StopOnShiny.Checked = Settings.Default.StopOnShiny;
            CheckBoxOf3.Checked = Settings.Default.SegmentOrFamily;
            GenderBox.SelectedIndex = Settings.Default.GenderFilter;
            HPFilter.Value = Settings.Default.HPFilter;
            AtkFilter.Value = Settings.Default.AtkFilter;
            DefFilter.Value = Settings.Default.DefFilter;
            SpaFilter.Value = Settings.Default.SpaFilter;
            SpdFilter.Value = Settings.Default.SpdFilter;
            SpeFilter.Value = Settings.Default.SpeFilter;
            if (Settings.Default.IgnoreIVFilter)
                IgnoreIVFilter.Checked = true;
            PresetIVBox.SelectedIndex = Settings.Default.PresetIVS;
        }

        private void PresetIVBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selection = PresetIVBox.SelectedIndex;
            switch (selection)
            {
                case 0: break;
                case 1:
                    {
                        HPFilter.Value = 31; AtkFilter.Value = 31; DefFilter.Value = 31; SpaFilter.Value = 31; SpdFilter.Value = 31; SpeFilter.Value = 31; break;
                    }
                case 2:
                    {
                        HPFilter.Value = 31; AtkFilter.Value = 0; DefFilter.Value = 31; SpaFilter.Value = 31; SpdFilter.Value = 31; SpeFilter.Value = 0; break;
                    }
                case 3:
                    {
                        HPFilter.Value = 31; AtkFilter.Value = 0; DefFilter.Value = 31; SpaFilter.Value = 31; SpdFilter.Value = 31; SpeFilter.Value = 31; break;
                    }
                case 4:
                    {
                        HPFilter.Value = 31; AtkFilter.Value = 31; DefFilter.Value = 31; SpaFilter.Value = 31; SpdFilter.Value = 31; SpeFilter.Value = 0; break;
                    }
            }
        }
    }
}
