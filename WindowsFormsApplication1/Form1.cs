using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var time = DateTime.Now;
            Task.Factory.StartNew(() =>
            {
                using (var context = new TestContext("data source=10.10.21.5;initial catalog=tycho_kc;persist security info=True;user id=sa;password=sa123;MultipleActiveResultSets=True;App=EntityFramework"))
                {
                    var test = context.Set<General>().FirstOrDefault();
                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show(DateTime.Now.ToString() + time);
                    }));
                }
            });
        }
    }
}
