using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schwimmbad
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool wochenende;
        bool ferien;
        bool eingegeben = false;
        bool ausgewertet = false;
        double über16;
        double unter16;
        double gutscheine;
        double unter4;
        double gesamtpreis;
        List<string> karten = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void cbWoche_Checked(object sender, RoutedEventArgs e)
        {
            wochenende = true;
        }

        private void cbWoche_Unchecked(object sender, RoutedEventArgs e)
        {
            wochenende = false;
        }

        private void cbFerien_Checked(object sender, RoutedEventArgs e)
        {
            ferien = true;
        }

        private void cbFerien_Unchecked(object sender, RoutedEventArgs e)
        {
            ferien = false;
        }

        private void bAusgeben_Click(object sender, RoutedEventArgs e)
        {
            if (eingegeben == true && ausgewertet == true)
            {
                lbGesamtpreis.Items.Add(gesamtpreis);
                foreach (string karte in karten)
                {
                    lbKarten.Items.Add(karte);
                    lbLogs.Items.Add(karte);
                }
            }
            else
            {
                lbLogs.Items.Add("Bitte lesen sie zuerst Daten ein und werten sie diese aus!");
            }
        }

        private void bEingeben_Click(object sender, RoutedEventArgs e)
        {
            if (tbU16.Text == "" || tbU4.Text == "" || tbÜ16.Text == "" || tbGutscheine.Text == "")
            {
                lbLogs.Items.Add("Bitte Füllen die jedes Feld aus!");
            }
            else
            {
                unter16 = Convert.ToInt32(tbU16.Text);
                über16 = Convert.ToInt32(tbÜ16.Text);
                unter4 = Convert.ToInt32(tbU4.Text);
                gutscheine = Convert.ToInt32(tbGutscheine.Text);
                eingegeben = true;
                lbLogs.Items.Add("Eingabe erfolgreich!");
            }
        }

        private void bAuswerten_Click(object sender, RoutedEventArgs e)
        {
            if (eingegeben == true)
            {
                if (unter4 >= 1 && über16 <= 0)
                {
                    lbLogs.Items.Add("Kinder unter 4 Jahren müssen in Begleitung einer Person über 16 Jahren sein!");
                }

                if (ferien == false && (über16 + unter16) == gutscheine)
                {
                    gesamtpreis = 0;
                    lbLogs.Items.Add("Personenanzahl und gutscheine gleichen sich aus.");
                    lbKarten.Items.Add("Es müssen keine karten gekauft werden!");
                    lbGesamtpreis.Items.Add(gesamtpreis);
                }
                else
                {
                    Berechnungen();
                    if (unter16 == 0 && über16 == 0)
                    {
                        lbLogs.Items.Add("Berechnungen abgeschlossen!");
                    }
                }
                ausgewertet = true;
            }
            else
            {
                lbLogs.Items.Add("Bitte lesen Sie zuerst Daten ein!");
            }
        }

        public void Berechnungen()
        {
            ///Familienkarten
            if (über16 >= 2 && unter16 >= 2)
            {
                über16 -= 2;
                unter16 -= 2;
                karten.Add("Familienkarte2(8)");
                gesamtpreis += 8;
                Berechnungen();
            }

            if (über16 >= 1 && unter16 >= 3)
            {
                über16 -= 1;
                unter16 -= 3;
                karten.Add("Familienkarte1(8)");
                gesamtpreis += 8;
                Berechnungen();
            }

            if (ferien == true)
            {

                if (wochenende == true)
                {
                    ///Einzelkarten
                    double tempü16 = über16;
                    for (int i = 0; i < tempü16; i++)
                    {
                        gesamtpreis += 3.5;
                        karten.Add("Einzelkarte(3,5)");
                        über16 -= 1;
                    }

                    double tempu16 = unter16;
                    for (int i = 0; i < tempu16; i++)
                    {
                        gesamtpreis += 2.5;
                        karten.Add("Einzelkarte(2,5)");
                        unter16 -= 1;
                    }
                }
                else if (wochenende == false)
                {
                    ///Tageskarten
                    if ((über16 * 2.8) + (unter16 * 2) >= 11)
                    {
                        int zähler = 0;
                        double tempÜ16 = über16;
                        double tempU16 = unter16;
                        while (zähler < 6)
                        {
                            for (int i = 0; i < tempÜ16; i++)
                            {
                                if (zähler < 6)
                                {
                                    über16 -= 1;
                                    zähler += 1;
                                }
                            }

                            for (int i = 0; i < tempU16; i++)
                            {
                                if (zähler < 6)
                                {
                                    unter16 -= 1;
                                    zähler += 1;
                                }
                            }
                            if (unter16 == 0 && über16 == 0)
                            {
                                zähler = 7;
                            }
                        }
                        karten.Add("Tageskarte(11)");
                        gesamtpreis += 11;
                        Berechnungen();
                    }

                    /// Einzelkarten -20%
                    double tempü16 = über16;
                    for (int i = 0; i < tempü16; i++)
                    {
                        gesamtpreis += 2.8;
                        karten.Add("Einzelkarte(2,8)");
                        über16 -= 1;
                    }

                    double tempu16 = unter16;
                    for (int i = 0; i < tempu16; i++)
                    {
                        gesamtpreis += 2;
                        karten.Add("Einzelkarte(2)");
                    }
                }
            }
            else if (ferien == false)
            {
                if (wochenende == true)
                {
                    if ((über16 + unter16) > 10)
                    {
                        double tempu = 0;
                        double tempü = 0;
                        gutscheine -= 1;
                        for (int i = 0; i < gutscheine; i++)
                        {
                            if (unter16 > 0)
                            {
                                unter16 -= 1;
                                tempu += 1;
                            }
                            else if (über16 > 0)
                            {
                                über16 -= 1;
                                tempü += 1;
                            }
                        }
                        gesamtpreis += (((über16 * 3.5) + (unter16 * 2.5)) * 0.9);
                        double temppreis = (((über16 * 3.5) + (unter16 * 2.5)) * 0.9);
                        über16 -= über16;
                        unter16 -= unter16;
                        unter16 = tempu;
                        über16 = tempü;
                        string tempAus = "Gruppe(" + temppreis + ")";
                        karten.Add(tempAus);
                    }

                    for (int i = 0; i < gutscheine; i++)
                    {
                        if (über16 > 0)
                        {
                            über16 -= 1;
                            karten.Add("Gutschein");
                        }
                        else if (unter16 > 0)
                        {
                            unter16 -= 1;
                            karten.Add("Gutschein");
                        }
                    }

                    ///Einzelkarten
                    double tempü16 = über16;
                    for (int i = 0; i < tempü16; i++)
                    {
                        gesamtpreis += 3.5;
                        karten.Add("Einzelkarte(3,5)");
                        über16 -= 1;
                    }

                    double tempu16 = unter16;
                    for (int i = 0; i < tempu16; i++)
                    {
                        gesamtpreis += 2.5;
                        karten.Add("Einzelkarte(2,5)");
                        unter16 -= 1;
                    }
                }
                else if (wochenende == false)
                {
                    if ((über16 + unter16) > 10)
                    {
                        double tempu = 0;
                        double tempü = 0;
                        double temppreis2 = 0;
                        gutscheine -= 1;
                        for (int i = 0; i < gutscheine; i++)
                        {
                            if (unter16 > 0)
                            {
                                unter16 -= 1;
                                tempu += 1;
                            }
                            else if (über16 > 0)
                            {
                                über16 -= 1;
                                tempü += 1;
                            }
                        }
                        gesamtpreis += (((über16 * 2.8) + (unter16 * 2)) * 0.9);
                        temppreis2 = (((über16 * 2.8) + (unter16 * 2)) * 0.9);
                        über16 -= über16;
                        unter16 -= unter16;
                        unter16 = tempu;
                        über16 = tempü;
                        string tempAus = "Gruppe(" + temppreis2 + ")";
                        karten.Add(tempAus);
                    }

                    for (int i = 0; i < gutscheine; i++)
                    {
                        if (über16 > 0)
                        {
                            über16 -= 1;
                            karten.Add("Gutschein");
                        }
                        else if (unter16 > 0)
                        {
                            unter16 -= 1;
                            karten.Add("Gutschein");
                        }
                    }

                    ///Tageskarten
                    if ((über16 * 2.8) + (unter16 * 2) >= 11)
                    {
                        int zähler = 0;
                        double tempÜ16 = über16;
                        double tempU16 = unter16;
                        while (zähler < 7)
                        {

                            for (int i = 0; i < tempÜ16; i++)
                            {
                                if (zähler < 7)
                                {
                                    über16 -= 1;
                                    zähler += 1;
                                }
                            }

                            for (int i = 0; i < tempU16; i++)
                            {
                                if (zähler < 7)
                                {
                                    unter16 -= 1;
                                    zähler += 1;
                                }
                            }
                            if (unter16 == 0 && über16 == 0)
                            {
                                zähler = 7;
                            }
                        }
                        karten.Add("Tageskarte(11)");
                        gesamtpreis += 11;
                        Berechnungen();
                    }

                    /// Einzelkarten -20%
                    double tempü16 = über16;
                    double temppreis = 0;
                    for (int i = 0; i < tempü16; i++)
                    {
                        gesamtpreis += 2.8;
                        temppreis += 2.8;
                        karten.Add("Einzelkarte(2,8)");
                        über16 -= 1;
                    }

                    double tempu16 = unter16;
                    for (int i = 0; i < tempu16; i++)
                    {
                        gesamtpreis += 2;
                        temppreis += 2;
                        karten.Add("Einzelkarte(2)");
                        unter16 -= 1;
                    }
                }
            }
        }
    }
}

