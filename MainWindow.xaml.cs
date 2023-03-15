using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Schema;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<String> csaladiNevek = new ObservableCollection<string>();
        private ObservableCollection<String> utoNevek = new ObservableCollection<string>();
        private ObservableCollection<String> letrehozottNevek = new ObservableCollection<string>();
        private ObservableCollection<String> kukazandoCsaladnevek = new ObservableCollection<string>();
        private ObservableCollection<String> kukazandoUtonevek = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();

        }

        private void btnBetoltCsaladnev_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                foreach (var nev in File.ReadAllLines(ofd.FileName).ToList())
                {
                    csaladiNevek.Add(nev);
                    lbCsaladnevek.Items.Add(nev);
                }

            }
            lblCsaladnevekSzama.Content = csaladiNevek.Count;
            sldSlider.Maximum = csaladiNevek.Count;
            lblMax.Content = csaladiNevek.Count;
        }
        private void btnBetoltUtonev_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                foreach (var nev in File.ReadAllLines(ofd.FileName).ToList())
                {
                    utoNevek.Add(nev);
                    lbUtonevek.Items.Add(nev);
                }

            }
            lblUtonevekSzama.Content = utoNevek.Count;
        }
        private void sldSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtNevekSzam.Text = Convert.ToString((int)Math.Floor(sldSlider.Value));
        }
        private void txtNevekSzam_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (txtNevekSzam.Text == "")
            {
                sldSlider.Value = 0;
            }
            sldSlider.Value = Convert.ToDouble(txtNevekSzam.Text);
        }
        private void btnNevetGeneral_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            int sliderErtek = Convert.ToInt32(sldSlider.Value);
            if (rbEgy.IsChecked == true)
            {
                for (int index = 0; index < sliderErtek; index++)
                {
                    int randomCsalad = rnd.Next(0, csaladiNevek.Count);
                    int randomUto = rnd.Next(0, utoNevek.Count);

                    letrehozottNevek.Add(csaladiNevek[randomCsalad] + " " + utoNevek[randomUto]);
                    kukazandoUtonevek.Add(utoNevek[randomUto]);
                    kukazandoCsaladnevek.Add(csaladiNevek[randomCsalad]);
                    csaladiNevek.RemoveAt(randomCsalad);
                    utoNevek.RemoveAt(randomUto);
                }
                foreach (var elem in letrehozottNevek)
                {
                    lbGeneraltNevek.Items.Add(elem);
                }
                letrehozottNevek.Clear();

            }
            else if (rbKetto.IsChecked == true)
            {
                for (int i = 0; i < sliderErtek; i++)
                {
                    string csaladNev, utoNev, utoNev_2;
                    int randomCsalad = rnd.Next(0, lbCsaladnevek.Items.Count);
                    int randomUto = rnd.Next(0, lbUtonevek.Items.Count);
                    int randomUto_2 = rnd.Next(0, lbUtonevek.Items.Count - 1);
                    csaladNev = Convert.ToString(lbCsaladnevek.Items[randomCsalad]);
                    lbCsaladnevek.Items.RemoveAt(randomCsalad);
                    csaladiNevek.RemoveAt(randomCsalad);
                    utoNev = Convert.ToString(lbUtonevek.Items[randomUto]);
                    lbUtonevek.Items.RemoveAt(randomUto);
                    utoNevek.RemoveAt(randomUto);
                    utoNev_2 = Convert.ToString(lbUtonevek.Items[randomUto_2]);
                    lbUtonevek.Items.RemoveAt(randomUto_2);
                    utoNevek.RemoveAt(randomUto_2);
                    letrehozottNevek.Add(csaladNev + " " + utoNev + " " + utoNev_2);
                    kukazandoCsaladnevek.Add(csaladNev);
                    kukazandoUtonevek.Add(utoNev + " " + utoNev_2);

                }

                foreach (var elem in letrehozottNevek)
                {
                    lbGeneraltNevek.Items.Add(elem);
                }
                letrehozottNevek.Clear();
            }
            stbRendezes.Content = "";
            NevlistaVegereUgras();
            sldSlider.Maximum = csaladiNevek.Count;
            lblMax.Content = csaladiNevek.Count;
            lblCsaladnevekSzama.Content = csaladiNevek.Count;
            lblUtonevekSzama.Content = utoNevek.Count;
        }
        private void btnNevekMentese_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "txt";
            sfd.Filter = "Szöveges fájl (*.txt) | *.txt |CSV fájl (*.csv) | *.csv|Összes fájl (*.*) | *.*";
            sfd.Title = "Adja meg a névsor nevét!";
            if (sfd.ShowDialog() == true)
            {
                StreamWriter mentes = new StreamWriter(sfd.FileName);
                if (sfd.FilterIndex == 2)
                {
                    string csvNeve = "";
                    foreach (var elem in lbGeneraltNevek.Items)
                    {
                        string[] teljesGeneraltNev = elem.ToString().Split(' ');
                        if (teljesGeneraltNev.Length == 2)
                        {
                            csvNeve = teljesGeneraltNev[0] + ";" + teljesGeneraltNev[1];
                        }
                        else if (teljesGeneraltNev.Length == 3)
                        {
                            csvNeve = teljesGeneraltNev[0] + ";" + teljesGeneraltNev[1] + ";" + teljesGeneraltNev[2];
                        }
                        mentes.WriteLine(csvNeve);
                    }
                    mentes.Close();
                }
                else if (sfd.FilterIndex == 1)
                {
                    foreach (var elem in lbGeneraltNevek.Items)
                    {
                        mentes.WriteLine(elem.ToString());
                    }
                    mentes.Close();
                }
                MessageBox.Show("Sikeres a mentés!");
                mentes.Dispose();
            }
        }
        private void btnNevekRendezese_Click(object sender, RoutedEventArgs e)
        {
            lbGeneraltNevek.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            stbRendezes.Content = "Rendezett névsor!";
        }
        private void NevlistaVegereUgras()
        {
            lbCsaladnevek.Items.MoveCurrentToLast();
            lbCsaladnevek.ScrollIntoView(lbCsaladnevek.Items.CurrentItem);
            lbUtonevek.Items.MoveCurrentToLast();
            lbUtonevek.ScrollIntoView(lbUtonevek.Items.CurrentItem);
            lbGeneraltNevek.Items.MoveCurrentToLast();
            lbGeneraltNevek.ScrollIntoView(lbGeneraltNevek.Items.CurrentItem);

        }
        private void btnNevekTorlese_Click(object sender, RoutedEventArgs e)
        {
            letrehozottNevek.Clear();
            lbGeneraltNevek.Items.Clear();

            foreach (var elem in kukazandoCsaladnevek)
            {
                csaladiNevek.Add(elem.ToString());
                lbCsaladnevek.Items.Add(elem);
            }
            foreach (var elem in kukazandoUtonevek)
            {
                string[] utonevekSplit = elem.Split(' ');
                if (utonevekSplit.Length > 1)
                {
                    utoNevek.Add(utonevekSplit[0]);
                    utoNevek.Add(utonevekSplit[1]);
                    lbUtonevek.Items.Add(utonevekSplit[0]);
                    lbUtonevek.Items.Add(utonevekSplit[1]);
                }
                else
                {
                    utoNevek.Add(utonevekSplit[0]);
                    lbUtonevek.Items.Add(utonevekSplit[0]);
                }



            }
            stbRendezes.Content = "";
            kukazandoCsaladnevek.Clear();
            kukazandoUtonevek.Clear();
            NevlistaVegereUgras();
            sldSlider.Maximum = csaladiNevek.Count;
            lblUtonevekSzama.Content = utoNevek.Count;
            lblMax.Content = csaladiNevek.Count;
            lblCsaladnevekSzama.Content = csaladiNevek.Count;
        }
        private void ElemTorlese(object sender, MouseButtonEventArgs e)
        {
            string valasztottNev = Convert.ToString(lbGeneraltNevek.SelectedItem);

            if (lbGeneraltNevek.SelectedItem != null)
            {
                string[] nevTomb = valasztottNev.Split(' ');
                letrehozottNevek.Remove(valasztottNev);

                if (nevTomb.Length == 3)
                {

                    kukazandoCsaladnevek.Remove(nevTomb[0]);
                    kukazandoUtonevek.Remove(nevTomb[1] + " " + nevTomb[2]);
                    lbGeneraltNevek.Items.Remove(valasztottNev);
                }

                else if (nevTomb.Length == 2)
                {

                    kukazandoCsaladnevek.Remove(nevTomb[0]);
                    kukazandoUtonevek.Remove(nevTomb[1]);
                    lbGeneraltNevek.Items.Remove(valasztottNev);
                }
            }

            string[] visszarak = valasztottNev.Split(' ');


            if (visszarak.Length == 2)
            {
                csaladiNevek.Add(visszarak[0]);
                utoNevek.Add(visszarak[1]);

            }
            else if (visszarak.Length == 3)
            {
                csaladiNevek.Add(visszarak[0]);
                utoNevek.Add(visszarak[1]);
                utoNevek.Add(visszarak[2]);
            }

            lblCsaladnevekSzama.Content = csaladiNevek.Count;
            lblUtonevekSzama.Content = utoNevek.Count;
            lblMax.Content = csaladiNevek.Count;
            sldSlider.Maximum = csaladiNevek.Count;
            NevlistaVegereUgras();

        }

        private void btnNevekMegforditasa_Click(object sender, RoutedEventArgs e)
        {
            List<object> lista = new List<object>();
            for (int i = lbGeneraltNevek.Items.Count - 1; i >= 0; i--)
            {
                lista.Add(lbGeneraltNevek.Items[i]);
            }
            lbGeneraltNevek.Items.Clear();
            foreach (object item in lista)
            {
                lbGeneraltNevek.Items.Add(item);
            }
        }

        private void sliAthelyez_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblSzamlalas.Content = Math.Floor(sliAthelyez.Value);
            
        }

        private void btnNevekAthelyezese_Click(object sender, RoutedEventArgs e)
        {
            int athelyez = Convert.ToInt32(Math.Floor(sliAthelyez.Value));

            switch (athelyez)
            {
                case 1:
                    foreach (var elem in lbCsaladnevek.Items)
                    {
                        lbGeneraltNevek.Items.Add(elem);
                    }
                    lbCsaladnevek.Items.Clear();
                    lblCsaladnevekSzama.Content = lbCsaladnevek.Items.Count;
                    break;

                case 2:
                    int indexKetto = 1;
                    ObservableCollection<string> listaKetto = new ObservableCollection<string>();
                    while (indexKetto <= lbCsaladnevek.Items.Count)
                    {
                        listaKetto.Add(lbCsaladnevek.Items[indexKetto].ToString());
                        lbGeneraltNevek.Items.Add(lbCsaladnevek.Items[indexKetto]);
                        indexKetto += 2;
                    }
                    foreach (var elem in listaKetto)
                    {
                        lbCsaladnevek.Items.Remove(elem);
                        lblCsaladnevekSzama.Content = lbCsaladnevek.Items.Count;
                    }
                    break;
                case 3:
                    int indexHarom = 2;
                    ObservableCollection<string> listaHarom = new ObservableCollection<string>();
                    while (indexHarom <= lbCsaladnevek.Items.Count)
                    {
                        listaHarom.Add(lbCsaladnevek.Items[indexHarom].ToString());
                        lbGeneraltNevek.Items.Add(lbCsaladnevek.Items[indexHarom]);
                        indexHarom += 3;
                    }
                    foreach (var elem in listaHarom)
                    {
                        lbCsaladnevek.Items.Remove(elem);
                        lblCsaladnevekSzama.Content = lbCsaladnevek.Items.Count;
                    }
                    break;

                case 4:
                    int indexNegy = 3;
                    ObservableCollection<string> listaNegy = new ObservableCollection<string>();
                    while (indexNegy <= lbCsaladnevek.Items.Count)
                    {
                        listaNegy.Add(lbCsaladnevek.Items[indexNegy].ToString());
                        lbGeneraltNevek.Items.Add(lbCsaladnevek.Items[indexNegy]);
                        indexNegy += 4;
                    }
                    foreach (var elem in listaNegy)
                    {
                        lbCsaladnevek.Items.Remove(elem);
                        lblCsaladnevekSzama.Content = lbCsaladnevek.Items.Count;
                    }
                    break;
                case 5:
                    int indexOt = 4;
                    ObservableCollection<string> listaOt = new ObservableCollection<string>();
                    while (indexOt <= lbCsaladnevek.Items.Count)
                    {
                        listaOt.Add(lbCsaladnevek.Items[indexOt].ToString());
                        lbGeneraltNevek.Items.Add(lbCsaladnevek.Items[indexOt]);
                        indexOt += 5;
                    }
                    foreach (var elem in listaOt)
                    {
                        lbCsaladnevek.Items.Remove(elem);
                        lblCsaladnevekSzama.Content = lbCsaladnevek.Items.Count;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
