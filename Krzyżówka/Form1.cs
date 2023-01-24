using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Krzyżówka
{
    public partial class Form1 : Form
    {
        FileStream slownik;
        //FileStream wyrazy;
        StreamReader slownikCzytaj;
        StreamWriter slownikZapis;
        //StreamReader wyrazyCzytaj;
        string sciezka;
        //string sciezka02;

        Dictionary<char, int> alfabet;
        string alfabetL;
        string[] wyrazySlownik;
        //string[] wyrazyDodaj;

        bool checkWzP;
        bool checkWzK;
        bool checkAnP;
        bool checkAnK;


        public Form1()
        {
            InitializeComponent();
            sciezka = "slownik.txt";
            //sciezka02 = "wyrazy.txt";
            inicjujAlfabet();
            wczytajSlownik(1);

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            //tabPageWzorzec.Text = "● Wzorzec";

            tekstWzP.Enabled = false;
            tekstWzK.Enabled = false;
            checkWzP = false;
            checkWzK = false;
            tekstAnP.Enabled = false;
            tekstAnK.Enabled = false;
            checkAnP = false;
            checkAnK = false;
        }

        private void dodajDoSlownika(int tryb)
        {
            if (tryb == 1)
            {
                string[] wyrazy = tekstDodaj.Lines;
                int p = 0;
                int q = 0;

                for (int k = 0; k < wyrazy.Length; k++)
                {
                    if(walidujLiterySpacjaMyslnik(wyrazy[k]))
                    {
                        q = 1; continue;
                    }
                    for (int i = 0; i < wyrazySlownik.Length; i++)
                    {
                        if (porownajWyrazy(wyrazy[k], wyrazySlownik[i], "") == 0)
                        {
                            break;
                        }
                        else if (porownajWyrazy(wyrazy[k], wyrazySlownik[i], "") == 1)
                        {
                            p++;
                            string[] nowySlownik = new string[wyrazySlownik.Length + 1];
                            for (int j = 0; j < i; j++)
                            {
                                nowySlownik[j] = wyrazySlownik[j];
                            }
                            nowySlownik[i] = wyrazy[k];
                            for (int j = i; j < wyrazySlownik.Length; j++)
                            {
                                nowySlownik[j + 1] = wyrazySlownik[j];
                            }

                            wyrazySlownik = new string[nowySlownik.Length];
                            for (int j = 0; j < nowySlownik.Length; j++)
                            {
                                wyrazySlownik[j] = nowySlownik[j];
                            }
                            break;
                        }
                    }
                }
                if (wyrazy.Length == 0)
                {
                    MessageBox.Show("Podaj wyrazy do dodania !");
                }
                else if (q == 1)
                {
                    MessageBox.Show("Niektóre z podanych wyrazów zawierały niedozowolone znaki. Wyrazy te zostały pominięte.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (p == 0)
                {
                    MessageBox.Show("Podane wyrazy są już w słowniku.");
                }
                if (p > 0)
                {
                    MessageBox.Show("Wyrazy dodane poprawnie.");
                }
            }
            //else if (tryb == 2)  - dodawanie wyrazów z pliku (usunięte)
            //{
            //    sciezka02 = tekstPlik.Text;
            //    if (sciezka02 == "")
            //    {
            //        MessageBox.Show("Podaj ścieżkę do pliku !");
            //        return;
            //    }
            //    try
            //    {
            //        wczytajSlownik(2);
            //    }
            //    catch
            //    {
            //        MessageBox.Show("Zła ścieżka do pliku !!!");
            //        return;
            //    }
            //    if (wyrazyDodaj.Length > 1000)
            //    {
            //        MessageBox.Show("Za duży plik z wyrazami!" + Environment.NewLine + "Plik nie może zawierać więcej niż 1000 wyrazów.", "Uwaga!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //    int p = wyrazyDodaj.Length / 20;
            //    int q = 1;
            //    for (int k = 0; k < wyrazyDodaj.Length; k++)
            //    {
            //        if (k > p * q - 20)
            //        {
            //            tekstPlik.Text = "Ukończono " + (5 * q).ToString() + "%";
            //            tekstPlik.Refresh();
            //            q++;
            //        }
            //        for (int i = 0; i < wyrazySlownik.Length; i++)
            //        {
            //            if (porownajWyrazy(wyrazyDodaj[k], wyrazySlownik[i], "") == 0)
            //            {
            //                break;
            //            }
            //            else if (porownajWyrazy(wyrazyDodaj[k], wyrazySlownik[i], "") == 1)
            //            {
            //                string[] nowySlownik = new string[wyrazySlownik.Length + 1];
            //                for (int j = 0; j < i; j++)
            //                {
            //                    nowySlownik[j] = wyrazySlownik[j];
            //                }
            //                nowySlownik[i] = wyrazyDodaj[k];
            //                for (int j = i; j < wyrazySlownik.Length; j++)
            //                {
            //                    nowySlownik[j + 1] = wyrazySlownik[j];
            //                }

            //                wyrazySlownik = new string[nowySlownik.Length];
            //                for (int j = 0; j < nowySlownik.Length; j++)
            //                {
            //                    wyrazySlownik[j] = nowySlownik[j];
            //                }
            //                break;
            //            }
            //        }
            //    }
            //    MessageBox.Show("Wyrazy z pliku zostały dodane do słownika.");
            //    tekstPlik.Text = "";
            //}
            zapiszSlownik();
        }

        private void wczytajSlownik(int tryb)
        {
            if (tryb == 1)
            {
                int ilosc = liczIloscLinii(sciezka);

                slownik = new FileStream(sciezka, FileMode.OpenOrCreate);

                wyrazySlownik = new string[ilosc];

                slownikCzytaj = new StreamReader(slownik);

                for (int i = 0; i < ilosc; i++)
                {
                    wyrazySlownik[i] = slownikCzytaj.ReadLine();
                }

                slownikCzytaj.Close();
                slownik.Close();
            }
            //else if (tryb == 2)  - dodawanie wyrazów z pliku (usunięte)
            //{
            //    int iloscWyrazow = liczIloscLinii(sciezka02);

            //    wyrazy = new FileStream(sciezka02, FileMode.Open);

            //    wyrazyDodaj = new string[iloscWyrazow];

            //    wyrazyCzytaj = new StreamReader(wyrazy);

            //    for (int i = 0; i < iloscWyrazow; i++)
            //    {
            //        wyrazyDodaj[i] = wyrazyCzytaj.ReadLine();
            //    }

            //    wyrazyCzytaj.Close();
            //    wyrazy.Close();
            //}
            else if (tryb == 3)
            {
                sciezka = tekstSlownik.Text;
                tekstSlownik.Text = "Wczytuję słownik...";
                tekstSlownik.Refresh();
                try
                {
                    int ilosc = liczIloscLinii(sciezka);

                    slownik = new FileStream(sciezka, FileMode.Open);
                    wyrazySlownik = new string[ilosc];
                    slownikCzytaj = new StreamReader(slownik);

                    int p = ilosc/20;
                    int q = 1;
                    tekstSlownik.Text = "";
                    for (int i = 0; i < ilosc; i++)
                    {  
                        wyrazySlownik[i] = slownikCzytaj.ReadLine();
                        if (i > p * q)
                        {
                            tekstSlownik.Text = "Wczytano " + (5*q).ToString() + "%";
                            tekstSlownik.Refresh();
                            q++;
                        }
                    }
                    MessageBox.Show("Słownik " + sciezka + " wczytany prawidłowo.");
                    tekstSlownik.Text = "";
                    slownikCzytaj.Close();
                    slownik.Close();
                }
                catch
                {
                    MessageBox.Show("Błędna ścieżka pliku ze słownikiem !");
                }
            }
        }

        private void zapiszSlownik()
        {
            slownik = new FileStream(sciezka, FileMode.Truncate);
            slownikZapis = new StreamWriter(slownik);

            for (int i = 0; i < wyrazySlownik.Length; i++)
            {
                slownikZapis.WriteLine(wyrazySlownik[i]);
            }

            slownikZapis.Close();
            slownik.Close();
        }

        private void szukajWyrazy()
        {
            string wzorzec = tekstWzorzec.Text;
            tekstWyniki.Text = "";
            if (wzorzec.Length >= 3)
            {
                int licz = 0;
                for (int i = 0; i < wyrazySlownik.Length; i++)
                {
                    if (checkWzP)
                    {
                        string p = tekstWzP.Text;
                        if (!porownajPoczatki(wyrazySlownik[i].ToLower(), p.ToLower())) continue;
                    }
                    if (checkWzK)
                    {
                        string k = tekstWzK.Text;
                        if (!porownajKonce(wyrazySlownik[i].ToLower(), k.ToLower())) continue;
                    }
                    if (porownajWzorzec(wzorzec, wyrazySlownik[i]))
                    {
                        tekstWyniki.Text += wyrazySlownik[i];
                        tekstWyniki.Text += Environment.NewLine;
                        licz++;
                    }
                    if (licz > 500)
                    {
                        tekstWyniki.Text += Environment.NewLine;
                        tekstWyniki.Text += "Za dużo dopasowań, wypisałem pierwsze 500.";
                        break;
                    }
                }
                if (tekstWyniki.Text == "")
                {
                    tekstWyniki.Text = Environment.NewLine;
                    tekstWyniki.Text += "           BRAK DOPASOWAŃ !!!";
                }
            }
            else
            {
                tekstWyniki.Text = Environment.NewLine;
                tekstWyniki.Text += "           ZA KRÓTKI WZORZEC !!!";
            }
        }

        private bool porownajPoczatki(string s, string p) //zwraca true jesli string s zaczyna się stringiem p
        {
            if (p.Length >= s.Length) return false;

            for (int i = 0; i < p.Length; i++)
            {
                if (p[i] != s[i]) return false;
            }

            return true;
        }

        private bool porownajKonce(string s, string p) //zwraca true jesli string s konczy się stringiem p
        {
            if (p.Length >= s.Length) return false;

            int n = s.Length - 1;
            int k = p.Length - 1;
            for (int i = 0; i < p.Length; i++)
            {
                if (p[k - i] != s[n - i]) return false;
            }

            return true;
        }

        private void ulozWyrazy()
        {
            string[] grupy = new string[8];
            string wz = tekstWzorzec2.Text;

            if (!walidujCyfry18(wz))
            {
                tekstWyniki2.Text = Environment.NewLine;
                tekstWyniki2.Text += "         BŁĘDNY WZORZEC !!!";
                return;
            }

            tekstWyniki2.Text = "";

            grupy[0] = tekstGrupa1.Text.ToLower();
            grupy[1] = tekstGrupa2.Text.ToLower();
            grupy[2] = tekstGrupa3.Text.ToLower();
            grupy[3] = tekstGrupa4.Text.ToLower();
            grupy[4] = tekstGrupa5.Text.ToLower();
            grupy[5] = tekstGrupa6.Text.ToLower();
            grupy[6] = tekstGrupa7.Text.ToLower();
            grupy[7] = tekstGrupa8.Text.ToLower();

            if (wz.Length >= 3)
            {
                for (int i = 0; i < wyrazySlownik.Length; i++)
                {
                    if (czyPasuje(wz, wyrazySlownik[i], grupy))
                    {
                        tekstWyniki2.Text += wyrazySlownik[i];
                        tekstWyniki2.Text += Environment.NewLine;
                    }
                }
                if(tekstWyniki2.Text == "")
                {
                    tekstWyniki2.Text = Environment.NewLine;
                    tekstWyniki2.Text += "         BRAK DOPASOWAŃ !!!";
                }
            }
            else
            {
                tekstWyniki2.Text = Environment.NewLine;
                tekstWyniki2.Text += "       ZA KRÓTKI WZORZEC !!!";
                return;
            }

        }

        private int[] literyNaLiczby(string s)
        {
            int[] tab = new int[s.Length];

            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    tab[i] = alfabet[s[i]];
                }
            }
            catch
            {
                MessageBox.Show("Nieobsługiwany znak w wyrazie ze słownika: " + s + Environment.NewLine + Environment.NewLine + "Dozwolone są tylko litery polskiego i łacińskiego alfabetu.", "UWAGA!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return tab;
        }

        private int[] sortuj(int[] tab)
        {
            int[] w = new int[tab.Length];
            w = tab;
            int p;

            for (int i = 0; i < tab.Length; i++)
            {
                for (int j = i + 1; j < tab.Length; j++)
                {
                    if (tab[j] < tab[i])
                    {
                        p = tab[i];
                        tab[i] = tab[j];
                        tab[j] = p;
                    } 
                }
            }

            return w;
        }

        private bool porownajTablice(int[] tab1, int[] tab2)
        {
            for (int i = 0; i < tab1.Length; i++)
            {
                if (tab1[i] != tab2[i]) return false;
            }
            return true;
        }

        private void szukajAnagramy()
        {
            string lit = tekstAnagramy.Text;
            string litery = lit.ToLower();

            int a = 0, b = 0;


            for (int j = 0; j < litery.Length; j++)
            {
                if (litery[j] == '.') continue;
                b = 0;
                for (int i = 0; i < alfabetL.Length; i++)
                {
                    if (litery[j] == alfabetL[i]) { b = 1; break; }
                }
                if (b == 1) continue; else { a = 1; break; }
            }
            
            if(a == 1)
            {
                MessageBox.Show("Podaj poprawne litery bez spacji i znaków specjalnych!" + Environment.NewLine + Environment.NewLine + "Dozwolone są tylko litery polskiego i łacińskiego alfabetu." + Environment.NewLine + "Kropka (.) zastepuje dowolną literę.", "UWAGA!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //int[] tab1 = new int[litery.Length];
            //int[] tab2 = new int[litery.Length];
            //int[] tab3 = new int[litery.Length];
            //int[] tab4 = new int[litery.Length];
            string s;

            tekstWyniki3.Text = "";
            int licz = 0;
            for (int i = 0; i < wyrazySlownik.Length; i++)
            {
                if (wyrazySlownik[i].Length != litery.Length)
                    continue;
                else
                {
                    //tab1 = literyNaLiczby(litery); - wersja z sortowaniem alfabetycznym
                    //tab2 = literyNaLiczby(wyrazySlownik[i]);
                    //tab3 = sortuj(tab1);
                    //tab4 = sortuj(tab2);
                    //if (porownajTablice(tab3, tab4))
                    //{
                    //    tekstWyniki3.Text += wyrazySlownik[i];
                    //    tekstWyniki3.Text += Environment.NewLine;
                    //}
                    s = wyrazySlownik[i].ToLower();

                    if (checkAnP)
                    {
                        string p = tekstAnP.Text;
                        if (!porownajPoczatki(s, p.ToLower())) continue;
                    }
                    if (checkAnK)
                    {
                        string k = tekstAnK.Text;
                        if (!porownajKonce(s, k.ToLower())) continue;
                        //MessageBox.Show(k + " " + wyrazySlownik[i]);
                    }

                    for (int j = 0; j < litery.Length; j++)
                    {
                        if (litery[j] == '.') continue;

                        for (int k = 0; k < litery.Length; k++)
                        {
                            if (litery[j] == s[k])
                            {
                                s = podstawMyslnik(s, k);
                                break;
                            }
                        }
                    }
                    int x = 0, y = 0;
                    for (int j = 0; j < litery.Length; j++)
                    {
                        if(litery[j] == '.') x++;
                        if (s[j] != '-') y++;
                    }
                    if (x == y)
                    {
                        licz++;
                        tekstWyniki3.Text += wyrazySlownik[i];
                        tekstWyniki3.Text += Environment.NewLine;
                    }
                    if (licz > 500)
                    {
                        tekstWyniki3.Text += Environment.NewLine;
                        tekstWyniki3.Text += "Za dużo dopasowań. Wypisałem pierwsze 500.";
                        break;
                    }
                }
            }
            if (tekstWyniki3.Text == "")
            {
                tekstWyniki3.Text = Environment.NewLine;
                tekstWyniki3.Text += "           BRAK ANAGRAMÓW !!!";
            }

        }

        string podstawMyslnik(string s, int k)
        {
            string wynik = "";
            for (int i = 0; i < k; i++)
            {
                wynik += s[i];
            }
            wynik += '-';
            while (k + 1 < s.Length)
            {
                wynik += s[k + 1];
                k++;
            }

            return wynik;
        }

        private bool czyPasuje(string wz, string wy, string[] tab)
        {
            if (wz.Length != wy.Length) return false;

            string wyy = wy.ToLower();

            int[] wzo = new int[wz.Length];

            for (int i = 0; i < wz.Length; i++)
            {
                wzo[i] = Int32.Parse(wz[i].ToString());
            }

            int p = 0;
            for (int i = 0; i < wy.Length; i++)
            {
                for (int j = 0; j < tab[wzo[i] - 1].Length; j++)
                {
                    if (wyy[i] == tab[wzo[i] - 1][j])
                    {
                        p++;
                        break;
                    }
                }
            }

            if (p == wy.Length) return true;

            return false;
        }

        private bool porownajWzorzec(string wz, string wy)
        {
            string wzz = wz.ToLower();
            string wyy = wy.ToLower();
            if (wz.Length != wy.Length) return false;
            for (int i = 0; i < wzz.Length; i++)
            {
                if (wzz[i] != '.')
                {
                    if (wzz[i] != wyy[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void szkotka()
        {
            int d;
            string wz = comboBoxSzkotka.Text;
            if (wz == "")
            {
                MessageBox.Show("Wybierz długość szukanych wyrazów!", "Uwaga!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else d = Int32.Parse(wz);

            wz = tekstSzkotka.Text;
            if (walidujLitery(wz))
            {
                MessageBox.Show("Podane słowo zawiera niedozwolone znaki.", "Uwaga!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (wz.Length < 2)
            {
                MessageBox.Show("Za krótki wzorzec!" + Environment.NewLine + "Szukane słowo musi mieć co najmniej dwie litery.", "Uwaga!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (wz.Length>=d)
            {
                MessageBox.Show("Wzorzec musi być krótszy od szukanych wyrazów!", "Uwaga!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string s;
            int licz = 0;
            tekstWynikSz.Text = "";
            for (int i = 0; i < wyrazySlownik.Length; i++)
            {
                s = wyrazySlownik[i];
                if (s.Length != d) continue;
                if (czyZawiera(wz, s))
                {
                    tekstWynikSz.Text += s;
                    tekstWynikSz.Text += Environment.NewLine;
                    licz++;
                }
                if (licz == 500)
                {
                    tekstWynikSz.Text += Environment.NewLine;
                    tekstWynikSz.Text += "Za dużo dopasowań.";
                    tekstWynikSz.Text += Environment.NewLine;
                    tekstWynikSz.Text += "Wypisałem pierwsze 500.";
                    break;
                }
            }

            if (tekstWynikSz.Text == "")
            {
                tekstWynikSz.Text = Environment.NewLine;
                tekstWynikSz.Text += "          BRAK DOPASOWAŃ !!!";
            }

        }

        private void metagramy()
        {
            string wz = tekstMetagramy.Text.ToLower();
            string s;
            tekstWynikiMt.Text = "";

            if (walidujLitery(wz))
            {
                tekstWynikiMt.Text = Environment.NewLine + "        PODANY WYRAZ ZAWIERA ";
                tekstWynikiMt.Text += Environment.NewLine + "          NIEDOZWOLONY ZNAK!!!";
                return;
            }

            for (int i = 0; i < wyrazySlownik.Length; i++)
            {
                s = wyrazySlownik[i];
                if(czyMetagramy(s.ToLower(),wz))
                {
                    tekstWynikiMt.Text += s;
                    tekstWynikiMt.Text += Environment.NewLine;
                }        
            }

            if(tekstWynikiMt.Text == "")
            {
                tekstWynikiMt.Text = Environment.NewLine;
                tekstWynikiMt.Text += "          BRAK METAGRAMÓW!!!";
            }
        }

        private bool czyMetagramy(string s, string w) //zwraca true jeśli s i w są metagramami, false w przeciwnym razie
        {
            if (s.Length != w.Length) return false;
            int p = 0;

            for(int i = 0; i < s.Length; i++)
            {
                if (s[i] == w[i])
                    p++;
            }

            if (p == s.Length - 1) return true;
            else return false;
        }

        //funkcja sprawdza czy string w zawiera się w stringu s
        private bool czyZawiera(string w, string s)
        {
            if (s.Length <= w.Length) return false;
            string p;
            for (int i = 0; i < s.Length - w.Length + 1; i++)
            {
                p = s.Substring(i, w.Length);
                if (porownajWyrazy(w, p, s) == 0) return true;
            }

            return false;
        }

        //funkcja zwraca 1 jesli s1<s2, -1 jesli s2<s1, 0 jeśli s1=s2, 2 - błąd
        private int porownajWyrazy(string s1, string s2, string s)
        {
            int i = 0;
            int n;
            if (s1.Length <= s2.Length) n = s1.Length;
            else n = s2.Length;

            string zly;
            if (s == "") zly = s2; else zly = s;
            while(i < n)
            {
                try
                {
                    if (alfabet[s1[i]] < alfabet[s2[i]]) return 1;
                    else if (alfabet[s1[i]] > alfabet[s2[i]]) return -1;
                    else i++;
                }
                catch
                {
                    MessageBox.Show("W słowniku znaleziono wyraz zawierający niepolski znak."+Environment.NewLine+Environment.NewLine+"Znaleziony wyraz, to: "+zly, "Uwaga!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 2;
                }
            }

            if (s1.Length == s2.Length) return 0;
            else if (s1.Length < s2.Length) return 1;
            else return -1;
        }

        private bool walidujCyfry18(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] < 49 || s[i] > 56) return false;
            }

            return true;
        }


        private int liczIloscLinii(string sciez)
        {
            try
            {
                return File.ReadAllLines(sciez).Length;
            }
            catch
            {
                return -1;
            }           
        }

        private bool walidujLiterySpacjaMyslnik(string s)
        {
            string abc = "aąbcćdeęfghijklłmnńoópqrsśtuvwxyzźż -";
            string ss = s.ToLower();

            int licz = 0;

            for (int i = 0; i < ss.Length; i++)
            {
                for (int j = 0; j < abc.Length; j++)
                {
                    if (abc[j] == ss[i]) { licz++; break; }
                }
            }
            //MessageBox.Show(licz.ToString());
            if (licz == ss.Length) return false; else return true;
        }

        private bool walidujLitery(string s)//zwraca false jeśli s zawiera nisame litery
        {
            string abc = "aąbcćdeęfghijklłmnńoópqrsśtuvwxyzźż";
            string ss = s.ToLower();

            int licz = 0;

            for (int i = 0; i < ss.Length; i++)
            {
                for (int j = 0; j < abc.Length; j++)
                {
                    if (abc[j] == ss[i]) { licz++; break; }
                }
            }
            if (licz == ss.Length) return false; else return true;
        }

        private void inicjujAlfabet()
        {
            alfabet = new Dictionary<char, int>();
            alfabetL = "aąbcćdeęfghijklłmnńoópqrsśtuvwxyzźż";

            alfabet.Add('a', 1); alfabet.Add('A', 1);
            alfabet.Add('ą', 2); alfabet.Add('Ą', 2);
            alfabet.Add('b', 3); alfabet.Add('B', 3);
            alfabet.Add('c', 4); alfabet.Add('C', 4);
            alfabet.Add('ć', 5); alfabet.Add('Ć', 5);
            alfabet.Add('d', 6); alfabet.Add('D', 6);
            alfabet.Add('e', 7); alfabet.Add('E', 7);
            alfabet.Add('ę', 8); alfabet.Add('Ę', 8);
            alfabet.Add('f', 9); alfabet.Add('F', 9);
            alfabet.Add('g', 10); alfabet.Add('G', 10);
            alfabet.Add('h', 11); alfabet.Add('H', 11);
            alfabet.Add('i', 12); alfabet.Add('I', 12);
            alfabet.Add('j', 13); alfabet.Add('J', 13);
            alfabet.Add('k', 14); alfabet.Add('K', 14);
            alfabet.Add('l', 15); alfabet.Add('L', 15);
            alfabet.Add('ł', 16); alfabet.Add('Ł', 16);
            alfabet.Add('m', 17); alfabet.Add('M', 17);
            alfabet.Add('n', 18); alfabet.Add('N', 18);
            alfabet.Add('ń', 19); alfabet.Add('Ń', 19);
            alfabet.Add('o', 20); alfabet.Add('O', 20);
            alfabet.Add('ó', 21); alfabet.Add('Ó', 21);
            alfabet.Add('p', 22); alfabet.Add('P', 22);
            alfabet.Add('q', 23); alfabet.Add('Q', 23);
            alfabet.Add('r', 24); alfabet.Add('R', 24);
            alfabet.Add('s', 25); alfabet.Add('S', 25);
            alfabet.Add('ś', 26); alfabet.Add('Ś', 26);
            alfabet.Add('t', 27); alfabet.Add('T', 27);
            alfabet.Add('u', 28); alfabet.Add('U', 28);
            alfabet.Add('v', 29); alfabet.Add('V', 29);
            alfabet.Add('w', 30); alfabet.Add('W', 30);
            alfabet.Add('x', 31); alfabet.Add('X', 31);
            alfabet.Add('y', 32); alfabet.Add('Y', 32);
            alfabet.Add('z', 33); alfabet.Add('Z', 33);
            alfabet.Add('ź', 34); alfabet.Add('Ź', 34);
            alfabet.Add('ż', 35); alfabet.Add('Ż', 35);
            alfabet.Add(' ', 36); alfabet.Add('-', 37);

        }

        private void wzorzec_Click(object sender, EventArgs e)
        {
            szukajWyrazy();
        }

        private void ulozSam_Click(object sender, EventArgs e)
        {
            ulozWyrazy();
        }

        private void dodajWyrazy_Click(object sender, EventArgs e)
        {
            dodajDoSlownika(1);
        }

        //private void dodajWyrazyPlik_Click(object sender, EventArgs e)
        //{
        //    dodajDoSlownika(2);
        //}

        private void wczytajSlownik_Click(object sender, EventArgs e)
        {
            wczytajSlownik(3);
        }
        
        private void anagramy_Click(object sender, EventArgs e)
        {
            szukajAnagramy();
        }

        private void szkotka_Click(object sender, EventArgs e)
        {
            szkotka();
        }


        private void buttonMt_Click(object sender, EventArgs e)
        {
            metagramy();
        }

        private void wzorzec_Klawisz(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                wzorzec_Click(sender, e);
        }

        private void anagramy_Klawisz(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                anagramy_Click(sender, e);
        }

        private void ulozSam_Klawisz(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ulozSam_Click(sender, e);
        }

        private void szkotka_Klawisz(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                szkotka_Click(sender, e);
        }


        private void metagramy_Klawisz(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonMt_Click(sender, e);
        }

        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            TabPage tp = tabControl.TabPages[e.Index];

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            RectangleF headerRect = new RectangleF(e.Bounds.X - 2, e.Bounds.Y + 3, e.Bounds.Width + 2, e.Bounds.Height - 2);

            SolidBrush sb = new SolidBrush(Color.Transparent);

            if (tabControl.SelectedIndex == e.Index)
                if (e.Index == 5)
                    sb.Color = Color.DimGray;
                else
                    sb.Color = Color.DarkSeaGreen;

            g.FillRectangle(sb, e.Bounds);

            g.DrawString(tp.Text, tabControl.Font, new SolidBrush(Color.Black), headerRect, sf);
        }

        private void wybierzWzorzec_Click(object sender, EventArgs e)
        {
            tabPageWzorzec.Text = "Wzorzec";
            tabPageAnagramy.Text = "Anagramy";
            tabPageUluzSam.Text = "Ułuż Sam";
            tabPageSzkotka.Text = "Szkotka";
            tabPageSlownik.Text = "Słownik";

            string s = tabControl.SelectedTab.Text;
            tabControl.SelectedTab.Text = "● " + s;
        }

        private void checkBoxWzP_zmiana(object sender, EventArgs e)
        {
            if (checkWzP)
            {
                tekstWzP.Enabled = false;
                checkWzP = false;
            }
            else
            {
                tekstWzP.Enabled = true;
                checkWzP = true;
            }
            
        }

        private void checkBoxWZK_zmiana(object sender, EventArgs e)
        {
            if (checkWzK)
            {
                tekstWzK.Enabled = false;
                checkWzK = false;
            }
            else
            {
                tekstWzK.Enabled = true;
                checkWzK = true;
            }
        }

        private void checkBoxAnP_zmiana(object sender, EventArgs e)
        {
            if (checkAnP)
            {
                tekstAnP.Enabled = false;
                checkAnP = false;
            }
            else
            {
                tekstAnP.Enabled = true;
                checkAnP = true;
            }
        }

        private void checkBoxAnK_zmiana(object sender, EventArgs e)
        {
            if (checkAnK)
            {
                tekstAnK.Enabled = false;
                checkAnK = false;
            }
            else
            {
                tekstAnK.Enabled = true;
                checkAnK = true;
            }
        }

        private void nacisnijKlawisz_tab(object sender, KeyEventArgs e)
        {
            int t = tabControl.SelectedIndex;
            if (e.KeyCode == Keys.F6)
            {
                switch (t)
                {
                    case 0: tekstWzorzec.Text = ""; tekstWzorzec.Focus(); break;
                    case 1: tekstAnagramy.Text = ""; tekstAnagramy.Focus(); break;
                    case 2: tekstMetagramy.Text = ""; tekstMetagramy.Focus(); break;
                    case 3: tekstWzorzec2.Text = ""; tekstWzorzec2.Focus(); break;
                    case 4: tekstSzkotka.Text = ""; tekstSzkotka.Focus(); break;
                    default: break;
                }
            }

            if (e.KeyCode == Keys.F5)
            {
                switch (t)
                {
                    case 0: tabControl.SelectedTab = tabPageAnagramy; break;
                    case 1: tabControl.SelectedTab = tabPageMetagramy; break;
                    case 2: tabControl.SelectedTab = tabPageUluzSam; break;
                    case 3: tabControl.SelectedTab = tabPageSzkotka; break;
                    case 4: tabControl.SelectedTab = tabPageSlownik; break;
                    case 5: tabControl.SelectedTab = tabPageWzorzec; break;
                    default: break;
                }
            }
        }
    }
}
