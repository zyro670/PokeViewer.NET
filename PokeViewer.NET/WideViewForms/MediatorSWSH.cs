using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeViewer.NET.WideViewForms
{
    public partial class MediatorSWSH : Form
    {
        public MediatorSWSH()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using WideViewerSWSH WideForm = new();
            WideForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using MiniViewerSWSH WideForm = new();
            WideForm.ShowDialog();
        }
    }
}
