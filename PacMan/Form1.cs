using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacMan
{
    public partial class Form1 : Form
    {        
        public static int vyskaFormulare = 600;
        public static int sirkaFormulare = 1000; 
        public static char tlacitko;

        public Form1()
        {
            InitializeComponent();
            Mapa.form = this;        
        }

        private void button1_Click(object sender, EventArgs e) //new game
        {
            
            button1.Hide();
            button2.Hide();
            pictureBox1.Hide();
            Mapa.stav = Stav.hrajese;     
            Mapa.NactiMapu();
            Mapa.NactiIkonky();
            Mapa.VykresliMapu();
            timer1.Enabled = true;
            Mapa.skore = label1;
            Mapa.lives = label2;
            Mapa.timer = timer1;
            Mapa.pacmanTimer = timer2;
            Mapa.lbl = label3;
            label1.Visible = true;
            label2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e) //exit
        {
            Form1.ActiveForm.Close();
        }

        private void BackToMenu()
        {
            Mapa.VymazVse();
            label1.Hide();
            label2.Hide();
            label1.Text = "High Score: 0";
            label2.Text = "Lives: 3";
            button1.Visible = true;
            button2.Visible = true;
            pictureBox1.Visible = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (Mapa.stav)
            {
                case Stav.hrajese:
                    Mapa.PohniVsemiPrvky();
                    Mapa.VykresliMapu();
                    Refresh();                   
                    break;
                case Stav.vyhra:
                    timer1.Enabled = false;
                    MessageBox.Show("You win!");
                    BackToMenu();
                    break;
                case Stav.prohra:
                    timer1.Enabled = false;
                    MessageBox.Show("Game over");
                    BackToMenu();
                    break;
                default:
                    break;
            }

        }

        public  StisknutaSipka stisknutaSipka = StisknutaSipka.zadna;

        

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
          
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            stisknutaSipka = StisknutaSipka.zadna;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space) e.Handled = true;
            if (e.KeyData == Keys.Up && Mapa.JeVolnoNeboDuch(Mapa.souradnicePacmana.y - 1, Mapa.souradnicePacmana.x))
            {
                stisknutaSipka = StisknutaSipka.nahoru;

            }
            if (e.KeyData == Keys.Down && Mapa.JeVolnoNeboDuch(Mapa.souradnicePacmana.y + 1, Mapa.souradnicePacmana.x))
            {
                stisknutaSipka = StisknutaSipka.dolu;

            }
            if (e.KeyData == Keys.Left && Mapa.JeVolnoNeboDuch(Mapa.souradnicePacmana.y, Mapa.souradnicePacmana.x - 1))
            {
                stisknutaSipka = StisknutaSipka.doleva;

            }
            if (e.KeyData == Keys.Right && Mapa.JeVolnoNeboDuch(Mapa.souradnicePacmana.y, Mapa.souradnicePacmana.x + 1))
            {
                stisknutaSipka = StisknutaSipka.doprava;
              
            }
        }
    }
}
