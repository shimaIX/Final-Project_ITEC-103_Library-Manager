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
    public partial class MainUI : Form
    {
        public MainUI()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MainUI_Load(object sender, EventArgs e)
        {
            home1.BringToFront();
            hideAndShowPanel(true, false, false, false, false);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            home1.BringToFront();
            hideAndShowPanel(true, false, false, false, false);

            home1.dailyQuotes();
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            //books1.BringToFront();
            listOfBooks1.BringToFront();
            hideAndShowPanel(false, true, false, false, false);
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            transaction1.BringToFront();
            hideAndShowPanel(false, false, true, false, false);
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            shift1.BringToFront();
            hideAndShowPanel(false, false, false, true, false);
        }

        private void btnLibrarians_Click(object sender, EventArgs e)
        {
            librarian1.BringToFront();
            hideAndShowPanel(false, false, false, false, true);

        }

        private void hideAndShowPanel(bool home, bool books, bool transaction, bool schedule, bool librarians)
        {
            panelNavBarHome.Hide();
            panelNavBarBooks.Hide();
            panelNavBarTransaction.Hide();
            panelNavBarSchedule.Hide();
            panelNavBarHelp.Hide();

            if(home == true) {
                panelNavBarHome.Show();
            } else if (books == true) {
                panelNavBarBooks.Show();
            } else if (transaction == true) {
                panelNavBarTransaction.Show();
            } else if (schedule == true) {
                panelNavBarSchedule.Show();
            } else if (librarians == true) {
                panelNavBarHelp.Show();
            }
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to log out?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SignIn signIn = new SignIn();   
                signIn.Show();
                this.Hide();
            }
        }
    }
}
