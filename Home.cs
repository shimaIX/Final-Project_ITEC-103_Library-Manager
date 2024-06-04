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
    public partial class Home : UserControl
    {
        private static Home _instance;
        public static Home _Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Home();
                }
                return _instance;
            }
        }
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            timer1.Start();
            dailyQuotes();
            UpdateFormTitle();
        }

        public void dailyQuotes()
        {
            Random rnd = new Random();
            int num = rnd.Next(0, 10);

            string[] quotes =
            {
                "\"In the library, you'll find not just books but the whispered dreams of humanity, waiting to be discovered.\"",
                "\"A library is not just a place for books, it's a sanctuary where the mind finds solace and inspiration.\"",
                "\"Every book in the library holds a universe of wisdom, waiting patiently for those who seek it.\"",
                "\"Amidst the shelves of a library, one can hear the echoes of countless journeys, both real and imagined.\"",
                "\"Libraries are the silent champions of knowledge, standing tall to remind us of the power of ideas.\"",

                "\"In the library, time stands still as the pages turn, and worlds unfurl with each whispered word.\"",
                "\"The library is a treasure trove where the past meets the future, and every book is a key to unlock endless possibilities.\"",
                "\"Within the walls of a library, imagination takes flight, and dreams find their wings.\"",
                "\"A library is a haven for the restless soul, offering refuge in the embrace of boundless stories.\"",
                "\"In the quiet corners of a library, one finds not just books but the gentle whispers of inspiration, urging us to explore, to learn, and to grow.\""
            };

            switch (num)
            {
                case 0:
                    lblQuote.AutoSize = true;
                    lblQuote.Text = quotes[num];
                    lblQuote.Location = new Point(60, 18);
                    lblQuote.Font = new Font(lblQuote.Font.FontFamily, 12);
                    break;
                case 1:
                    lblQuote.AutoSize = true;
                    
                    lblQuote.Text = quotes[num];
                    lblQuote.Location = new Point(20, 16);
                    lblQuote.Font = new Font(lblQuote.Font.FontFamily, 14);
                    break;
                case 2:
                    lblQuote.AutoSize = true;
                    
                    lblQuote.Text = quotes[num];
                    lblQuote.Location = new Point(38, 16);
                    lblQuote.Font = new Font(lblQuote.Font.FontFamily, 14);
                    break;
                case 3:
                    lblQuote.AutoSize = true;
                    
                    lblQuote.Text = quotes[num];
                    lblQuote.Location = new Point(60, 16);
                    lblQuote.Font = new Font(lblQuote.Font.FontFamily, 12);
                    break;
                case 4:
                    lblQuote.AutoSize = true;
                    
                    lblQuote.Text = quotes[num];
                    lblQuote.Location = new Point(15, 16);
                    lblQuote.Font = new Font(lblQuote.Font.FontFamily, 14);
                    break;
                case 5:
                    lblQuote.AutoSize = true;
                    
                    lblQuote.Text = quotes[num];
                    lblQuote.Location = new Point(35, 16);
                    lblQuote.Font = new Font(lblQuote.Font.FontFamily, 14);
                    break;
                case 6:
                    lblQuote.AutoSize = true;
                    
                    lblQuote.Text = quotes[num];
                    lblQuote.Location = new Point(8, 16);
                    lblQuote.Font = new Font(lblQuote.Font.FontFamily, 12);
                    break;
                case 7:
                    lblQuote.AutoSize = true;
                    
                    lblQuote.Text = quotes[num];
                    lblQuote.Location = new Point(100, 16);
                    lblQuote.Font = new Font(lblQuote.Font.FontFamily, 14);
                    break;
                case 8:
                    lblQuote.AutoSize = true;
                    
                    lblQuote.Text = quotes[num];
                    lblQuote.Location = new Point(43, 16);
                    lblQuote.Font = new Font(lblQuote.Font.FontFamily, 14);
                    break;
                case 9:
                    lblQuote.Text = quotes[num];
                    lblQuote.AutoSize = false;
                    lblQuote.Size = new Size(868, 51);
                    lblQuote.TextAlign = ContentAlignment.MiddleCenter;
                    lblQuote.Location = new Point(28, 0);
                    lblQuote.Font = new Font(lblQuote.Font.FontFamily, 12);
                    break;
            }
        }


        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            DateTime currentDate = monthCalendar1.SelectionStart;

            this.Text = "Calendar - " + currentDate.ToString("MMMM dd, yyyy");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel21_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelDivider1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel20_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel18_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
