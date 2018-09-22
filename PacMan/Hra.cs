using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacMan
{
    public abstract class PohyblivyPrvek
    {
        public int x;
        public int y;
        public abstract void UdelejKrok();
    }

    class Pacman : PohyblivyPrvek
    {
        int pocetZivotu = 3;

        public Pacman (int kdey, int kdex)
        {
            this.x = kdex;
            this.y = kdey;
        }

        public void Zemri()
        {
            //Mapa.OdstranPohyblivyPrvek(this); //na souradnicich pacmana je nekdo jiny => je zabit 
            pocetZivotu--;
            Mapa.lives.Text = "Lives: " + pocetZivotu.ToString();
            if (pocetZivotu == 0)
            {
                Mapa.stav = Stav.prohra;
                Mapa.OdstranPohyblivyPrvek(this); //na souradnicich pacmana je nekdo jiny => je zabit 
            }
            else Mapa.Zpet();
         
        }
        

        public void NastavNezranitelnost()
        {
            // implementace
        }

        public override void UdelejKrok()
        {
            int nove_x = x;
            int nove_y = y;

            if (Mapa.PacmanUtika)
            {
                if (!Mapa.JeDuch(y, x))
                {
                    switch (Mapa.form.stisknutaSipka)
                    {
                        case StisknutaSipka.doprava:
                            if (Mapa.JeVolno(nove_y, nove_x + 1))
                            {
                                Mapa.PresunPacmana(nove_y, nove_x, nove_y, nove_x + 1);
                                x++;
                            }
                            break;
                        case StisknutaSipka.doleva:
                            if (Mapa.JeVolno(nove_y, nove_x - 1))
                            {
                                Mapa.PresunPacmana(nove_y, nove_x, nove_y, nove_x - 1);
                                x--;
                            }
                            break;
                        case StisknutaSipka.dolu:
                            if (Mapa.JeVolno(nove_y + 1, nove_x))
                            {
                                Mapa.PresunPacmana(nove_y, nove_x, nove_y + 1, nove_x);
                                y++;
                            }
                            break;
                        case StisknutaSipka.nahoru:
                            if (Mapa.JeVolno(nove_y - 1, nove_x))
                            {
                                Mapa.PresunPacmana(nove_y, nove_x, nove_y - 1, nove_x);
                                y--;
                            }
                            break;
                        case StisknutaSipka.zadna:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    //tady jeste rozdelit pomoci 'if' na pripad, kdy je pacman nastaven na nesmrtelneho 
                    Zemri();
                }
            }
            else
            {
                if (!Mapa.JeDuch(y,x))
                {
                    switch (Mapa.form.stisknutaSipka)
                    {
                        case StisknutaSipka.doprava:
                            if (Mapa.JeVolnoNeboDuch(nove_y, nove_x + 1))
                            {
                                Mapa.PresunPacmana(nove_y, nove_x, nove_y, nove_x + 1);
                                x++;
                            }
                            break;
                        case StisknutaSipka.doleva:
                            if (Mapa.JeVolnoNeboDuch(nove_y, nove_x - 1))
                            {
                                Mapa.PresunPacmana(nove_y, nove_x, nove_y, nove_x - 1);
                                x--;
                            }
                            break;
                        case StisknutaSipka.dolu:
                            if (Mapa.JeVolnoNeboDuch(nove_y + 1, nove_x))
                            {
                                Mapa.PresunPacmana(nove_y, nove_x, nove_y + 1, nove_x);
                                y++;
                            }
                            break;
                        case StisknutaSipka.nahoru:
                            if (Mapa.JeVolnoNeboDuch(nove_y - 1, nove_x))
                            {
                                Mapa.PresunPacmana(nove_y, nove_x, nove_y - 1, nove_x);
                                y--;
                            }
                            break;
                        case StisknutaSipka.zadna:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Zemri();
                }
            }
        }
    }

    public abstract class Duch : PohyblivyPrvek
    {
        protected Smer smer;
        public char podDuchem;
        public Policko puvSouradnice;
        const int delkaSmrti = 15;
        public int oziveniTimer;
        public bool honim;
        public char znakDuchaDefault;
        public char znakDucha;
        int paritaKroku = 0;
        public bool muzuJit;
        public bool utika;
        public bool zivy;
        public bool UzJsemVenku;
        public void CoTamBylo(int y, int x)
        {
            if (Mapa.JeJidlo(y, x)) podDuchem = 'J';
            else if (Mapa.JeBonus(y, x)) podDuchem = 'B';
            else podDuchem = Mapa.JestliJeDuchTakJaky(y, x);
        }

        public Duch(int puvY, int puvX)
        {            
            UzJsemVenku = false;
            honim = true;
            zivy = true;
            puvSouradnice = new Policko(puvY, puvX);      
        }

        /*public void AktualizujCoJePodDuchem()
        {

            char co = this.podDuchem;

            if (co == 'J' || co == 'B' || co == 'V' || co == 'G') Mapa.coByloPodDuchemModrym = co;
            else
            {
                while (co != 'J' && co != 'B' && co != 'V' && co != 'G')
                {
                    switch (co)
                    {
                        case '1':
                            co = Mapa.coByloPodDuchemOranzovym;
                            break;
                        case '2':
                            co = Mapa.coByloPodDuchemCervenym;
                            break;
                        case '3':
                            co = Mapa.coByloPodDuchemRuzovym;
                            break;
                        case '4':
                            co = Mapa.coByloPodDuchemModrym;
                            break;
                        default:
                            break;
                    }

                }
                Mapa.coByloPodDuchemModrym = co;
            }
        }*/

        public void Utec()
        {
            if (!Mapa.JePacman(y,x))
            {
                if (paritaKroku % 2 == 0)
                {
                    List<Policko> mozneTahy = new List<Policko>();
                    Policko novyTah = new Policko(0, 0);

                    if (!Mapa.JeStena(y - 1, x)) mozneTahy.Add(new Policko(y - 1, x));
                    if (!Mapa.JeStena(y + 1, x)) mozneTahy.Add(new Policko(y + 1, x));
                    if (!Mapa.JeStena(y, x - 1)) mozneTahy.Add(new Policko(y, x - 1));
                    if (!Mapa.JeStena(y, x + 1)) mozneTahy.Add(new Policko(y, x + 1));

                    if (mozneTahy.Count > 1)
                    {
                        Policko pom = Mapa.VratNejPolickoSmeremKPacmanovi(y, x);
                        if (pom != null)
                        {
                            Policko spatnePole = mozneTahy.First(p => p.x == pom.x && p.y == pom.y);
                            mozneTahy.Remove(spatnePole);
                        }
                        Random generator = new Random();
                        int nahodneCislo = generator.Next(mozneTahy.Count);
                        novyTah = mozneTahy[nahodneCislo];
                    }
                    else
                    {
                        novyTah = mozneTahy.First();
                    }
                    Mapa.PresunPriserku(this, y, x, novyTah.y, novyTah.x);
                    x = novyTah.x;
                    y = novyTah.y;
                    paritaKroku++;
                }
                else paritaKroku++;
            }
            else
            {
                Zmiz();
            }

        }

        public void Zmiz()
        {
            zivy = false;
            oziveniTimer = delkaSmrti;
            x = puvSouradnice.x;
            y = puvSouradnice.y;
            Mapa.skoreHrace += 500;
            Mapa.skore.Text = "Score: " + Mapa.skoreHrace.ToString();
            Mapa.skore.Refresh();   
                                
        }

        public void Ozivni()
        {
            zivy = true;
            honim = true;
            znakDucha = znakDuchaDefault;
            Mapa.VratDuchaNaStart(this);

        }

        public void PresunDoleva(int puvY, int puvX)
        {
            CoTamBylo(puvY, puvX - 1);
            Mapa.PresunPriserku(this,puvY, puvX, puvY, puvX - 1);
            x--;
        }

        public void PresunDoprava(int puvY, int puvX)
        {
            CoTamBylo(puvY, puvX + 1);
            Mapa.PresunPriserku(this, puvY, puvX, puvY, puvX + 1);
            x++;
        }

        public void PresunDolu(int puvY, int puvX)
        {
            CoTamBylo(puvY + 1, puvX);
            Mapa.PresunPriserku(this, puvY, puvX, puvY + 1, puvX);
            y++;
        }

        public void PresunNahoru(int puvY, int puvX)
        {
            CoTamBylo(puvY - 1, puvX);
            Mapa.PresunPriserku(this, puvY, puvX, puvY - 1, puvX);
            y--;
        }

    }

    public enum Smer { left, right, up, down}

    class DuchModry : Duch
    {
        public DuchModry(int kdey, int kdex, char smer) : base (kdey,kdex)
        {
            this.x = kdex;
            this.y = kdey;
            this.znakDucha = '4';
            znakDuchaDefault = '4'; 
            switch (smer)
            {
                case '>':
                    this.smer = Smer.right;
                    break;
                case '<':
                    this.smer = Smer.left;
                    break;
                case '^':
                    this.smer = Smer.up;
                    break;
                case 'v':
                    this.smer = Smer.down;
                    break;
                default:
                    break;
            }
        }
        public override void UdelejKrok()   //chodi podel prave steny
        {
            if (honim && zivy)
            {
                int nove_x = x;
                int nove_y = y;
                muzuJit = false;

                switch (this.smer) //otacim se podle smeru hodinovych rucicek
                {
                    case Smer.left:
                        if (Mapa.JeStena(nove_y - 1, nove_x) && (!Mapa.JeStena(nove_y, nove_x - 1))) PresunDoleva(nove_y, nove_x);
                        else if (Mapa.JeStena(nove_y, nove_x - 1) && Mapa.JeStena(nove_y - 1, nove_x))
                        {
                            smer = Smer.down;
                            if (!Mapa.JeStena(nove_y + 1, nove_x)) PresunDolu(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        else
                        {
                            smer = Smer.up;
                            if (!Mapa.JeStena(nove_y - 1, nove_x) && Mapa.JeStena(nove_y - 1, nove_x + 1)) PresunNahoru(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        break;

                    case Smer.right:
                        if (Mapa.JeStena(nove_y + 1, nove_x) && !Mapa.JeStena(nove_y, nove_x + 1)) PresunDoprava(nove_y, nove_x);
                        else if (Mapa.JeStena(nove_y, nove_x + 1) && Mapa.JeStena(nove_y + 1, nove_x)) //tam kam chci je zrovna stena a po me pravici je take stena
                        {
                            smer = Smer.up; //otocim se tedy opacnym smerem (doleva)
                            if (!Mapa.JeStena(nove_y - 1, nove_x)) PresunNahoru(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        else
                        {
                            smer = Smer.down;
                            if (!Mapa.JeStena(nove_y + 1, nove_x) && Mapa.JeStena(nove_y + 1, nove_x - 1)) PresunDolu(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        break;

                    case Smer.up:
                        if (Mapa.JeStena(nove_y, nove_x + 1) && !Mapa.JeStena(nove_y - 1, nove_x)) PresunNahoru(nove_y, nove_x);
                        else if (Mapa.JeStena(nove_y - 1, nove_x) && Mapa.JeStena(nove_y, nove_x + 1))
                        {
                            smer = Smer.left;
                            if (!Mapa.JeStena(nove_y, nove_x - 1)) PresunDoleva(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        else
                        {
                            smer = Smer.right;
                            if (!Mapa.JeStena(nove_y, nove_x + 1) && Mapa.JeStena(nove_y + 1, nove_x + 1)) PresunDoprava(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        break;

                    case Smer.down:
                        if (Mapa.JeStena(nove_y, nove_x - 1) && !Mapa.JeStena(nove_y + 1, nove_x)) PresunDolu(nove_y, nove_x);
                        else if (Mapa.JeStena(nove_y + 1, nove_x) && Mapa.JeStena(nove_y, nove_x - 1))
                        {
                            smer = Smer.right;
                            if (!Mapa.JeStena(nove_y, nove_x + 1)) PresunDoprava(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        else
                        {
                            smer = Smer.left;
                            if (!Mapa.JeStena(nove_y, nove_x - 1) && Mapa.JeStena(nove_y - 1, nove_x - 1)) PresunDoleva(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        break;
                    default:
                        break;
                }
                // AktualizujCoJePodDuchem();
            }
            else if(zivy)
            {
                Utec();
            }
            else
            {
                if (oziveniTimer == 0) Ozivni();
                else oziveniTimer--;
            }
            
        }
    }

    

    class DuchOranzovy : Duch   
    {
        public DuchOranzovy(int kdey, int kdex, char smer) : base (kdey,kdex)
        {
            this.x = kdex;
            this.y = kdey;
            this.znakDucha = '1';
            znakDuchaDefault = '1';
            switch (smer)
            {
                case '>':
                    this.smer = Smer.right;
                    break;
                case '<':
                    this.smer = Smer.left;
                    break;
                case '^':
                    this.smer = Smer.up;
                    break;
                case 'v':
                    this.smer = Smer.down;
                    break;
                default:
                    break;
            }
        }
        public override void UdelejKrok()  //chodi podel leve steny
        {
            if (honim && zivy)
            {
                int nove_x = x;
                int nove_y = y;

                switch (this.smer) //otacim se proti smeru hodinovych rucicek
                {
                    case Smer.left:
                        if (Mapa.JeStena(nove_y + 1, nove_x) && !Mapa.JeStena(nove_y, nove_x - 1)) PresunDoleva(nove_y, nove_x);
                        else if (Mapa.JeStena(nove_y, nove_x - 1) && Mapa.JeStena(nove_y + 1, nove_x))
                        {
                            smer = Smer.up;
                            if (!Mapa.JeStena(nove_y - 1, nove_x)) PresunNahoru(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        else
                        {
                            smer = Smer.down;
                            if (!Mapa.JeStena(nove_y + 1, nove_x) && Mapa.JeStena(nove_y + 1, nove_x + 1)) PresunDolu(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        break;

                    case Smer.right:
                        if (Mapa.JeStena(nove_y - 1, nove_x) && !Mapa.JeStena(nove_y, nove_x + 1)) PresunDoprava(nove_y, nove_x);
                        else if (Mapa.JeStena(nove_y, nove_x + 1) && Mapa.JeStena(nove_y - 1, nove_x)) //tam kam chci je zrovna stena a po me pravici je take stena
                        {
                            smer = Smer.down;
                            if (!Mapa.JeStena(nove_y + 1, nove_x)) PresunDolu(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        else
                        {
                            smer = Smer.up;
                            if (!Mapa.JeStena(nove_y - 1, nove_x) && Mapa.JeStena(nove_y - 1, nove_x - 1)) PresunNahoru(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        break;

                    case Smer.up:
                        if (Mapa.JeStena(nove_y, nove_x - 1) && !Mapa.JeStena(nove_y - 1, nove_x)) PresunNahoru(nove_y, nove_x);
                        else if (Mapa.JeStena(nove_y - 1, nove_x) && Mapa.JeStena(nove_y, nove_x - 1))
                        {
                            smer = Smer.right;
                            if (!Mapa.JeStena(nove_y, nove_x + 1)) PresunDoprava(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        else
                        {
                            smer = Smer.left;
                            if (!Mapa.JeStena(nove_y, nove_x - 1) && Mapa.JeStena(nove_y + 1, nove_x - 1)) PresunDoleva(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        break;

                    case Smer.down:
                        if (Mapa.JeStena(nove_y, nove_x + 1) && !Mapa.JeStena(nove_y + 1, nove_x)) PresunDolu(nove_y, nove_x);
                        else if (Mapa.JeStena(nove_y + 1, nove_x) && Mapa.JeStena(nove_y, nove_x + 1))
                        {
                            smer = Smer.left;
                            if (!Mapa.JeStena(nove_y, nove_x - 1)) PresunDoleva(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        else
                        {
                            smer = Smer.right;
                            if (!Mapa.JeStena(nove_y, nove_x + 1) && Mapa.JeStena(nove_y - 1, nove_x + 1)) PresunDoprava(nove_y, nove_x);
                            else UdelejKrok();
                        }
                        break;
                    default:
                        break;
                }
            }
            else if(zivy)
            {
                Utec();
            }
            else
            {
                if (oziveniTimer == 0) Ozivni();
                else oziveniTimer--;
            }
        }
    }

    public class Policko
    {
        public int x, y;
        public Policko(int y, int x)
        {
            this.y = y;
            this.x = x;
        }
    }


class DuchCerveny : Duch
    {
        Queue<Policko> dalsi = new Queue<Policko>();

        public DuchCerveny(int kdey, int kdex) : base (kdey,kdex)
        {
            this.y = kdey;
            this.x = kdex;
            znakDucha = '2';
            znakDuchaDefault = '2';
        }
        public override void UdelejKrok()
        {
            if (honim && zivy)
            {
                Policko novyKrok = Mapa.VratNejPolickoSmeremKPacmanovi(y, x);
                if (novyKrok != null)
                {
                    CoTamBylo(novyKrok.y, novyKrok.x);
                    Mapa.PresunPriserku(this, y, x, novyKrok.y, novyKrok.x);
                    y = novyKrok.y;
                    x = novyKrok.x;
                    //AktualizujCoJePodDuchem();
                }
            }
            else if (zivy)
            {
                Utec();
            }
            else 
            {
                if (oziveniTimer == 0) Ozivni();
                else oziveniTimer--;
            }
        }
    }

    class DuchRuzovy : Duch //nebudu implementovat
    {
        public DuchRuzovy(int puvY, int puvX) : base (puvY,puvX)
        {

        }
            
        public override void UdelejKrok()
        {
           // throw new NotImplementedException();
        }
    } 


    public enum StisknutaSipka { doprava, doleva, dolu, nahoru, zadna }
    public enum Stav { hrajese, vyhra, prohra }

    public static class Mapa
    {
        static char[,] mapa;
        static char[,] mapaJidlaABonusu;
        static char[] duchove = { '2', '4', '1' };
        static List<Bitmap> obrazky;
        static PictureBox[,] mapaVObrazcich;
        static List<PohyblivyPrvek> pohyblivePrvky;
        static int KolikZbyvaJidla = 0;
        static int KolikZbyvaBonusu = 0;
        static int vyska, sirka;
        static int dx, dy;
       
        static int typObrPacmana = 2; // na zacatku je Pacman otoceny doprava
        static Policko puvP, puvO, puvC, puvM; //puvodni souradnice duchu a pacmana

        public static int skoreHrace = 0;
        public const int delkaZameneni = 40;
        public static Form1 form;
        public static Stav stav = Stav.hrajese;
        public static Label skore;
        public static Label lives;
        public static Label lbl;        
        public static System.Windows.Forms.Timer timer;
        public static System.Windows.Forms.Timer pacmanTimer;
        public static Policko souradnicePacmana;
        public static bool PacmanUtika = true;
        public static int casovac = 0;


        public static void OdstranPohyblivyPrvek(PohyblivyPrvek prvek)
        {
            pohyblivePrvky.Remove(prvek);
        }

        public static void VymazVse()   // kdyz se vracim zpatky do menu
        {
            for (int y = 0; y < vyska; y++)
            {
                for (int x = 0; x < sirka; x++)
                {
                    mapaVObrazcich[y, x].Parent = null;
                }
            }

            skoreHrace = 0;
            KolikZbyvaBonusu = 0;
            KolikZbyvaJidla = 0;
            typObrPacmana = 2;
            /*
            coByloPodDuchemModrym = 'J';
            coByloPodDuchemOranzovym = 'J';
            coByloPodDuchemCervenym = 'J';
            coByloPodDuchemRuzovym = 'J';
            */
        }

        public static void Zpet()  //obnovi se hra po tom, co pacman zemre
        { 
            VykresliMapu();
            form.Refresh();
            foreach (PohyblivyPrvek prvek in pohyblivePrvky)
            {
                if (prvek is Pacman)
                {
                    PresunPacmana(prvek.y, prvek.x, puvP.y, puvP.x);
                    prvek.y = puvP.y;
                    prvek.x = puvP.x;
                }
                else 
                {
                    PresunPriserku((Duch)prvek, prvek.y, prvek.x, ((Duch)prvek).puvSouradnice.y, ((Duch)prvek).puvSouradnice.x);
                    prvek.x = ((Duch)prvek).puvSouradnice.x;
                    prvek.y = ((Duch)prvek).puvSouradnice.y;
                }
                ZamenRole(false);
                
            }
            form.stisknutaSipka = StisknutaSipka.zadna;
            timer.Stop();
            lbl.Visible = true;
            lbl.Text = "Get ready!";
            lbl.Refresh();
            Thread.Sleep(1000);
            lbl.Text = "3";
            lbl.Refresh();
            Thread.Sleep(1000);
            lbl.Text = "2";
            lbl.Refresh();
            Thread.Sleep(1000);
            lbl.Text = "1";
            lbl.Refresh();
            Thread.Sleep(1000);
            lbl.Hide();
            timer.Start();
        }

        public static void NactiIkonky()
        {
            obrazky = new List<Bitmap>();
            //0
            var pom = new Bitmap("PD.jpg");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //1
            pom = new Bitmap("PU.jpg");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //2
            pom = new Bitmap("PR.jpg");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //3
            pom = new Bitmap("PL.jpg");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //4
            pom = new Bitmap("D1.jpg");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //5
            pom = new Bitmap("D2.jpg");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //6
            pom = new Bitmap("D3.jpg");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //7
            pom = new Bitmap("D4.jpg");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //8
            pom = new Bitmap("ScaredGhost.png");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //9
            pom = new Bitmap("Prazdne.jpg");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //10
            pom = new Bitmap("wall.png");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //11
            pom = new Bitmap("jidloNorm.png");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //12
            pom = new Bitmap("jidloVelk.png");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //13
            pom = new Bitmap("modrOran.jpg");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
            //14
            pom = new Bitmap("Barikada.jpg");
            obrazky.Add(new Bitmap(pom, new Size(dx, dy)));
        }

        public static void NactiMapu()
        {
            pohyblivePrvky = new List<PohyblivyPrvek>();
            System.IO.StreamReader sr = new System.IO.StreamReader("mapa.txt");
            sirka = int.Parse(sr.ReadLine());
            vyska = int.Parse(sr.ReadLine());
            mapa = new char[vyska, sirka];
            mapaJidlaABonusu = new char[vyska, sirka];
            mapaVObrazcich = new PictureBox[vyska, sirka];
            sr.ReadLine();
            dx = Form1.sirkaFormulare / sirka;
            dy = Form1.vyskaFormulare / vyska - 4;

            for (int y = 0; y < vyska; y++)
            {
                string radek = sr.ReadLine();
                for (int x = 0; x < sirka; x++)
                {
                    char znak = radek[x];
                    mapa[y, x] = znak;
                    mapaVObrazcich[y, x] = new PictureBox();
                    PictureBox pomBox = mapaVObrazcich[y, x];
                    pomBox.Parent = Form.ActiveForm;
                    pomBox.Height = dy;
                    pomBox.Width = dx;
                    pomBox.Top = dy * y + 40;
                    pomBox.Left = dx * x;
                    pomBox.Visible = true;
                    pomBox.BackColor = Color.Black;
                    mapaJidlaABonusu[y, x] = 'V';

                    switch (znak)
                    {
                        case 'P': Pacman pacMan = new Pacman(y, x);
                            souradnicePacmana = new Policko(y, x);
                            puvP = new Policko(y, x);
                            pohyblivePrvky.Add(pacMan);
                            break;
                        case '1':
                            DuchOranzovy D_Oran = new DuchOranzovy(y, x, '<');
                            puvO = new Policko(y, x);
                            pohyblivePrvky.Add(D_Oran);  
                            break;
                        case '2':
                            DuchCerveny D_Cerv = new DuchCerveny(y, x);
                            puvC = new Policko(y, x);
                            pohyblivePrvky.Add(D_Cerv);
                            break;
                        case '3':
                            DuchRuzovy D_Ruz = new DuchRuzovy(y,x);
                            pohyblivePrvky.Add(D_Ruz);
                            break;
                        case '4':
                            DuchModry D_Mod = new DuchModry(y, x, '>');
                            puvM = new Policko(y, x);
                            pohyblivePrvky.Add(D_Mod);
                            break;
                        //jeste promyslet co s jidlem
                        case 'J':
                            KolikZbyvaJidla++;
                            mapaJidlaABonusu[y, x] = 'J';
                            break;
                        case 'B':
                            KolikZbyvaBonusu++;
                            mapaJidlaABonusu[y, x] = 'B';
                            break;
                        case 'G':
                            mapaJidlaABonusu[y, x] = 'G';
                            break;
                        default:
                            break;
                    }
                }
            }
            sr.Close();
        }

        public static void ZamenRole(bool PacmanBudeHonit)
        {
            if(PacmanBudeHonit)
            {
                foreach(Duch duch in pohyblivePrvky.Where(x => x is Duch))
                {
                    duch.znakDucha = 'S'; //scared ghost
                    duch.honim = false;
                }
                PacmanUtika = false;                
                casovac = delkaZameneni;
            }
            else
            {
                foreach (Duch duch in pohyblivePrvky.Where(x => x is Duch))
                {
                    if (!duch.honim)
                    {
                        duch.znakDucha = duch.znakDuchaDefault;
                        duch.honim = true;
                        duch.zivy = true;
                    }
                }
                PacmanUtika = true;
                casovac = 0;
            }
        }

        public static void VykresliMapu()
        {
            for (int y = 0; y < vyska; y++)
            {
                for (int x = 0; x < sirka; x++)
                {
                    switch (mapa[y, x])
                    {
                        case 'X': mapaVObrazcich[y, x].Image = obrazky[10];
                            break;
                        case 'J': mapaVObrazcich[y, x].Image = obrazky[11];                            
                            break;
                        case '1':
                            mapaVObrazcich[y, x].Image = obrazky[4];
                            break;
                        case '2':
                            mapaVObrazcich[y, x].Image = obrazky[5];
                            break;
                        case '3':
                            mapaVObrazcich[y, x].Image = obrazky[6];
                            break;
                        case '4':
                            mapaVObrazcich[y, x].Image = obrazky[7];
                            break;
                        case 'B':
                            mapaVObrazcich[y, x].Image = obrazky[12];                           
                            break;
                        case 'V':
                            mapaVObrazcich[y, x].Image = obrazky[9];
                            break;
                        case 'P':
                            mapaVObrazcich[y, x].Image = obrazky[typObrPacmana];
                            break;
                        case 'G':
                            mapaVObrazcich[y, x].Image = obrazky[14];
                            break;
                        case 'S':
                            mapaVObrazcich[y, x].Image = obrazky[8];
                            break;
                    }
                }
            }
        }

        public static void PohniVsemiPrvky()
        {
            if (pohyblivePrvky != null)
            {              
                List<PohyblivyPrvek> pomList = pohyblivePrvky.ToList();
                foreach (PohyblivyPrvek prvek in pomList)
                {
                    prvek.UdelejKrok();
                }
                if ((KolikZbyvaJidla == 0) && (KolikZbyvaBonusu == 0)) stav = Stav.vyhra;
            }
            if (!PacmanUtika)
            {
                if (casovac == 0) ZamenRole(false);
                else casovac--;
            }


        }

        public static bool JeVolno(int y, int x)
        {
            return mapa[y, x] == 'V' || mapa[y, x] == 'J' || mapa[y, x] == 'B';
        }

        public static bool JeDuch(int y, int x)
        {
            return mapa[y, x] == '1' || mapa[y, x] == '2' || mapa[y, x] == '3' || mapa[y, x] == '4';
        }

        public static bool JeVolnoNeboDuch(int y, int x)
        {
            return mapa[y, x] == 'V' || mapa[y, x] == 'J' || mapa[y, x] == 'B' /*|| mapa[y, x] == '1' || mapa[y, x] == '2' || mapa[y, x] == '3' || mapa[y, x] == '4' */|| mapa[y,x] == 'S';
            
        }

        public static bool JeJidlo(int y, int x)
        {
            return mapa[y, x] == 'J';
        }

        public static bool JeBonus(int y, int x)
        {
            return mapa[y, x] == 'B';
        }

        public static bool JeStena(int y, int x)
        {
            return mapa[y, x] == 'X';
        }

        public static bool JePacman(int y, int x)
        {
            return mapa[y, x] == 'P';
        }

        public static char JestliJeDuchTakJaky(int y, int x)
        {
            if ((mapa[y, x] == '1') || (mapa[y, x] == '2') || (mapa[y, x] == '3') || (mapa[y, x] == '4'))
            {
                switch (mapa[y, x])
                {
                    case '1':
                        return '1';
                    case '2':
                        return '2';
                    case '3':
                        return '3';
                    case '4':
                        return '4';
                    default: return 'V';
                }
            }
            else return 'V';
        }

        public static void PresunPacmana(int zY, int zX, int naY, int naX)
        {
            if (mapaJidlaABonusu[naY, naX] == 'J')
            {
                skoreHrace += 10;
                KolikZbyvaJidla--;
                skore.Text = "Score: " + skoreHrace.ToString();
            }

            if (mapaJidlaABonusu[naY, naX] == 'B')
            {
                ZamenRole(true);
                skoreHrace += 50;
                skore.Text = "Score" + skoreHrace.ToString();
                KolikZbyvaBonusu--;
            }
            char pom = mapa[zY, zX];
            
            if (JeVolnoNeboDuch(zY,zX)) 
            {
                typObrPacmana = 2;
                mapa[naY, naX] = 'P';
            }
            else
            {
                mapa[naY, naX] = pom;
                if (zY != naY) //pacman jde bud nahoru nebo dolu
                {
                    if (zY < naY) typObrPacmana = 0; //sel dolu
                    else typObrPacmana = 1;
                }
                else //jde doprava nebo doleva
                {
                    if (zX < naX) typObrPacmana = 2; //doprava
                    else typObrPacmana = 3;
                }
            }
            if (zY != naY || zX != naX)
            {
                mapa[zY, zX] = 'V';                
            }
            souradnicePacmana.x = naX;
            souradnicePacmana.y = naY;
            mapaJidlaABonusu[naY, naX] = 'V';
            
        }

        public static void PresunPriserku(Duch duch,int zY, int zX, int naY, int naX)
        {
            List<char> ostatniDuchove = duchove.ToList();
            ostatniDuchove.Remove(duch.znakDuchaDefault);            
            if (!ostatniDuchove.Contains(mapa[zY, zX]) || mapa[zY, zX] == duch.znakDuchaDefault) mapa[zY,zX] = mapaJidlaABonusu[zY, zX];
            mapa[naY, naX] = duch.znakDucha;

            /*
            if (mapa[zY, zX] == duch.znakDucha) //kdyz zadny duch s timto duchem nekrizi
            {
                mapa[zY, zX] = duch.podeMnou;
                if (!ostatniDuchove.Contains(mapa[naY, naX])) duch.podeMnou = mapa[naY, naX];
                
            }
            else
            {               
                if (!ostatniDuchove.Contains(mapa[naY, naX])) duch.podeMnou = mapa[naY, naX];

            }*/
        }

        public static void VratDuchaNaStart(Duch duch)
        {
            mapa[duch.puvSouradnice.y, duch.puvSouradnice.x] = duch.znakDuchaDefault;
        }

        public static Policko VratNejPolickoSmeremKPacmanovi(int zY, int zX)
        {
            //algoritmus vlny            
            int[,] pole = new int[vyska, sirka];

            for (int j = 0; j < vyska; j++)
            {
                for (int i = 0; i < sirka; i++)
                {
                    if (JeStena(j, i)) pole[j, i] = -10;
                    else pole[j, i] = -5;                          
                }
            }
            int vyslX = 0, vyslY = 0;
            int y = 0, x = 0;
            bool nasliJsme = false;
            Queue<Policko> fronta = new Queue<Policko>();
            fronta.Enqueue(new Policko(zY, zX));
            pole[zY, zX] = 0;
            while(!nasliJsme && fronta.Count > 0)
            {
                Policko novePolicko = fronta.Dequeue();
                y = novePolicko.y;
                x = novePolicko.x;
                if (JePacman(y, x))
                {
                    nasliJsme = true;
                    vyslY = y;
                    vyslX = x;
                }
                else
                {
                    if(pole[y + 1,x] == -5)
                    {
                        fronta.Enqueue(new Policko(y + 1, x));
                        pole[y + 1, x] = pole[y, x] + 1;
                    }
                    if (pole[y - 1, x] == -5)
                    {
                        fronta.Enqueue(new Policko(y - 1, x));
                        pole[y - 1, x] = pole[y, x] + 1;
                    }
                    if (pole[y, x + 1] == -5)
                    {
                        fronta.Enqueue(new Policko(y, x + 1));
                        pole[y, x + 1] = pole[y, x] + 1;
                    }
                    if (pole[y, x - 1] == -5)
                    {
                        fronta.Enqueue(new Policko(y, x - 1));
                        pole[y, x - 1] = pole[y, x] + 1;
                    }
                }                
            }
            if (nasliJsme)
            {
                //najdeme cestu z (zY,zX) do (vyslY, vyslX)
                Stack<Policko> cesta = new Stack<Policko>();
                int delkaCesty = pole[vyslY, vyslX];
                y = vyslY;
                x = vyslX;
                cesta.Push(new Policko(vyslY, vyslX));
                for (int l = delkaCesty - 1; l > 0; l--)
                {
                    if (pole[y + 1, x] == l)
                    {
                        cesta.Push(new Policko(y + 1, x));
                        y++;
                    }
                    else if (pole[y - 1, x] == l)
                    {
                        cesta.Push(new Policko(y - 1, x));
                        y--;
                    }
                    else if (pole[y, x + 1] == l)
                    {
                        cesta.Push(new Policko(y, x + 1));
                        x++;
                    }
                    else if (pole[y, x - 1] == l)
                    {
                        cesta.Push(new Policko(y, x - 1));
                        x--;
                    }
                }
                return cesta.Pop();
            }
            else
            {
                return null;
            }
        }
            

    }
}
