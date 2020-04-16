using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace Battleships
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string getIpThis() {
            //return IPAddress.Parse(textBox1.Text).ToString();     
            return ipthis.ToString();
        }
        public string getIpOther() {
            //return IPAddress.Parse(textBox2.Text).ToString();
            return ipother.ToString();
        }

        public static IPAddress ipthis;
        public static IPAddress ipother;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ipthis = IPAddress.Parse(textBox1.Text);
                ipother = IPAddress.Parse(textBox2.Text);
                label1.Text = "BATTLESHIPS";
                label1.Location = new Point(this.ClientRectangle.Width / 2 - label1.Width / 2, 200);
                Game startgame = new Game();
                startgame.Show();

            }
            catch(Exception exception)
            {
                label1.Text = exception.StackTrace;
                label1.Text = "WRONG IP FORMAT!";
                label1.Location = new Point(this.ClientRectangle.Width / 2 - label1.Width / 2, 200);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "this PC's ip here";
            textBox2.Text = "enemy PC's ip here";
            label1.Location = new Point(this.ClientRectangle.Width / 2 - label1.Width/2, 200);
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if(textBox1.Text == "this PC's ip here")
                textBox1.Text = "";
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox2.Text == "enemy PC's ip here")
                textBox2.Text = "";
        }
    }
}
