using PokeViewer.NET.Properties;

namespace PokeViewer.NET.SubForms
{
    public partial class StopConditions : Form
    {
        public StopConditions((Color, Color) color)
        {
            InitializeComponent();
            SetColors(color);
            LoadDefaults();
        }

        private void SetColors((Color, Color) color)
        {
            BackColor = color.Item1;
            ForeColor = color.Item2;
            SaveButton.BackColor = color.Item1;
            SaveButton.ForeColor = color.Item2;
            ResetButton.BackColor = color.Item1;
            ResetButton.ForeColor = color.Item2;
            ShinyBox.BackColor = color.Item1;
            ShinyBox.ForeColor = color.Item2;
            GenderBox.BackColor = color.Item1;
            GenderBox.ForeColor = color.Item2;
            NatureBox.BackColor = color.Item1;
            NatureBox.ForeColor = color.Item2;
            CheckBoxOf3.BackColor = color.Item1;
            CheckBoxOf3.ForeColor = color.Item2;
            ScaleBox.BackColor = color.Item1;
            ScaleBox.ForeColor = color.Item2;
            GenderFilter.BackColor = color.Item1;
            GenderFilter.ForeColor = color.Item2;
            ApplyIVFilter.BackColor = color.Item1;
            ApplyIVFilter.ForeColor = color.Item2;
            HPFilter.BackColor = color.Item1;
            HPFilter.ForeColor = color.Item2;
            AtkFilter.BackColor = color.Item1;
            AtkFilter.ForeColor = color.Item2;
            DefFilter.BackColor = color.Item1;
            DefFilter.ForeColor = color.Item2;
            SpaFilter.BackColor = color.Item1;
            SpaFilter.ForeColor = color.Item2;
            SpdFilter.BackColor = color.Item1;
            SpdFilter.ForeColor = color.Item2;
            SpeFilter.BackColor = color.Item1;
            SpeFilter.ForeColor = color.Item2;
            TargetATK.BackColor = color.Item1;
            TargetATK.ForeColor = color.Item2;
            TargetHP.BackColor = color.Item1;
            TargetHP.ForeColor = color.Item2;
            TargetDEF.BackColor = color.Item1;
            TargetDEF.ForeColor = color.Item2;
            TargetSPA.BackColor = color.Item1;
            TargetSPA.ForeColor = color.Item2;
            TargetSPD.BackColor = color.Item1;
            TargetSPD.ForeColor = color.Item2;
            TargetSPE.BackColor = color.Item1;
            TargetSPE.ForeColor = color.Item2;
            StopConditionsGroup.BackColor = color.Item1;
            StopConditionsGroup.ForeColor = color.Item2;
            HPCon.BackColor = color.Item1;
            HPCon.ForeColor = color.Item2;
            ATKCon.BackColor = color.Item1;
            ATKCon.ForeColor = color.Item2;
            DEFCon.BackColor = color.Item1;
            DEFCon.ForeColor = color.Item2;
            SPACon.BackColor = color.Item1;
            SPACon.ForeColor = color.Item2;
            SPDCon.BackColor = color.Item1;
            SPDCon.ForeColor = color.Item2;
            SPECon.BackColor = color.Item1;
            SPECon.ForeColor = color.Item2;
            MatchShinyIV.BackColor = color.Item1;
            MatchShinyIV.ForeColor = color.Item2;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Settings.Default.ShinyFilter = ShinyBox.SelectedIndex;
            Settings.Default.SegmentOrFamily = CheckBoxOf3.Checked;
            Settings.Default.GenderFilter = GenderBox.SelectedIndex;
            Settings.Default.NatureFilter = NatureBox.SelectedIndex;
            Settings.Default.HPFilter = (int)HPFilter.Value;
            Settings.Default.AtkFilter = (int)AtkFilter.Value;
            Settings.Default.DefFilter = (int)DefFilter.Value;
            Settings.Default.SpaFilter = (int)SpaFilter.Value;
            Settings.Default.SpdFilter = (int)SpdFilter.Value;
            Settings.Default.SpeFilter = (int)SpeFilter.Value;
            Settings.Default.ApplyIVFilter = ApplyIVFilter.Checked;
            Settings.Default.MinMaxOnly = ScaleBox.Checked;
            Settings.Default.ApplyIVFilter = ApplyIVFilter.Checked;
            Settings.Default.HpCon = HPCon.SelectedIndex;
            Settings.Default.AtkCon = ATKCon.SelectedIndex;
            Settings.Default.DefCon = DEFCon.SelectedIndex;
            Settings.Default.SpaCon = SPACon.SelectedIndex;
            Settings.Default.SpdCon = SPDCon.SelectedIndex;
            Settings.Default.SpeCon = SPECon.SelectedIndex;
            Settings.Default.MatchShinyIV = MatchShinyIV.Checked;

            Settings.Default.Save();
            Close();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ShinyBox.SelectedIndex = 2;
            CheckBoxOf3.Checked = false;
            GenderBox.SelectedIndex = 3;
            NatureBox.SelectedIndex = 25;
            HPFilter.Value = 0;
            AtkFilter.Value = 0;
            DefFilter.Value = 0;
            SpaFilter.Value = 0;
            SpdFilter.Value = 0;
            SpeFilter.Value = 0;
            HPCon.SelectedIndex = 0;
            ATKCon.SelectedIndex = 0;
            DEFCon.SelectedIndex = 0;
            SPACon.SelectedIndex = 0;
            SPDCon.SelectedIndex = 0;
            SPECon.SelectedIndex = 0;
            ApplyIVFilter.Checked = true;
            ScaleBox.Checked = false;
            MessageBox.Show("Stop Conditions have been reset to default values.");
        }

        private void LoadDefaults()
        {
            ShinyBox.SelectedIndex = Settings.Default.ShinyFilter;
            CheckBoxOf3.Checked = Settings.Default.SegmentOrFamily;
            GenderBox.SelectedIndex = Settings.Default.GenderFilter;
            NatureBox.SelectedIndex = Settings.Default.NatureFilter;
            HPFilter.Value = Settings.Default.HPFilter;
            AtkFilter.Value = Settings.Default.AtkFilter;
            DefFilter.Value = Settings.Default.DefFilter;
            SpaFilter.Value = Settings.Default.SpaFilter;
            SpdFilter.Value = Settings.Default.SpdFilter;
            SpeFilter.Value = Settings.Default.SpeFilter;
            HPCon.SelectedIndex = Settings.Default.HpCon;
            ATKCon.SelectedIndex = Settings.Default.AtkCon;
            DEFCon.SelectedIndex = Settings.Default.DefCon;
            SPACon.SelectedIndex = Settings.Default.SpaCon;
            SPDCon.SelectedIndex = Settings.Default.SpdCon;
            SPECon.SelectedIndex = Settings.Default.SpeCon;
            ScaleBox.Checked = Settings.Default.MinMaxOnly;
            ApplyIVFilter.Checked = Settings.Default.ApplyIVFilter;
            MatchShinyIV.Checked = Settings.Default.MatchShinyIV;
        }

        private void ShinyBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selection = ShinyBox.SelectedIndex;
            switch (selection)
            {
                case 0:
                    {
                        Settings.Default.ShinyFilter = 0; break;
                    }
                case 1:
                    {
                        Settings.Default.ShinyFilter = 1; break;
                    }
                case 2:
                    {
                        Settings.Default.ShinyFilter = 2; break;
                    }
                case 3:
                    {
                        Settings.Default.ShinyFilter = 3; break;
                    }
                case 4:
                    {
                        Settings.Default.ShinyFilter = 4; break;
                    }
            }
        }
    }
}
