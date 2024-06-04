using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Manager
{
    public partial class Librarian : UserControl
    {
        public Librarian()
        {
            InitializeComponent();
        }

        private void Librarian_Load(object sender, EventArgs e)
        {
            aboutUs1.BringToFront();
            hideAndShowPanel(false, false, false);
        }

        private void btn_AboutUs1_Click(object sender, EventArgs e)
        {
            hernandez1.BringToFront();
            hideAndShowPanel(true, false, false);
        }

        private void btn_AboutUs2_Click(object sender, EventArgs e)
        {
            tolentino1.BringToFront();
            hideAndShowPanel(false, true, false);
        }

        private void btn_AboutUs3_Click(object sender, EventArgs e)
        {
            gandia1.BringToFront();
            hideAndShowPanel(false, false, true);
        }

        private void hideAndShowPanel(bool hernandez, bool tolentino, bool gandia)
        {
            panelNavBarHernandez.Visible = false;
            panelNavBarTolentino.Visible = false;
            panelNavBarGandia.Visible = false;


            if(hernandez == true) 
            {
                panelNavBarHernandez.Visible = true;
            }
            else if(tolentino == true) 
            { 
                panelNavBarTolentino.Visible = true;
            }
            else if(gandia == true) 
            {
                panelNavBarGandia.Visible = true;
            }
        }

        private void pnlTitle_MouseClick(object sender, MouseEventArgs e)
        {
            aboutUs1.BringToFront();
            hideAndShowPanel(false, false, false);
        }

        private void lblAboutUs_MouseClick(object sender, MouseEventArgs e)
        {
            aboutUs1.BringToFront();
            hideAndShowPanel(false, false, false);
        }
    }
}
