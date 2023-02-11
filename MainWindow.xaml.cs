﻿using Microsoft.Win32;
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
            if (ofd.ShowDialog()==true)
            {
                foreach (var nev in File.ReadAllLines(ofd.FileName).ToList())
                {
                    csaladiNevek.Add(nev);
                }
                lbCsaladnevek.ItemsSource = csaladiNevek;
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
                }
                lbUtonevek.ItemsSource = utoNevek;
            }
            lblUtonevekSzama.Content= utoNevek.Count;
        }
        private void sldSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtNevekSzam.Text = Convert.ToString((int)sldSlider.Value);
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
                lbGeneraltNevek.ItemsSource = letrehozottNevek;

            }
            else if (rbKetto.IsChecked==true)
            {
                for (int index = 0; index < sliderErtek; index++)
                {
                    int randomCsalad = rnd.Next(0, csaladiNevek.Count);
                    int randomUto = rnd.Next(0, utoNevek.Count);
                    kukazandoUtonevek.Add(utoNevek[randomUto]);
                    kukazandoCsaladnevek.Add(csaladiNevek[randomCsalad]);
                    utoNevek.RemoveAt(randomUto);
                    int randomUto_2 = rnd.Next(0, utoNevek.Count);

                    letrehozottNevek.Add(csaladiNevek[randomCsalad] + " " + utoNevek[randomUto] + " " + utoNevek[randomUto_2]);
                    kukazandoUtonevek.Add(utoNevek[randomUto]+ " " + utoNevek[randomUto_2]);
                    kukazandoCsaladnevek.Add(csaladiNevek[randomCsalad]);
                    csaladiNevek.RemoveAt(randomCsalad);
                    if (utoNevek[randomUto_2] == utoNevek[randomUto] || randomUto_2 > utoNevek.Count)
                    {
                        continue;
                    }
                    
                    utoNevek.RemoveAt(randomUto_2);
                }
                lbGeneraltNevek.ItemsSource = letrehozottNevek;
            }
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
            if (sfd.ShowDialog()==true)
            {
                StreamWriter mentes = new StreamWriter(sfd.FileName);
                try
                {
                    for (int index = 0; index < lbGeneraltNevek.Items.Count; index++)
                    {
                        mentes.WriteLine(Convert.ToString(lbGeneraltNevek.Items[index]));
                    }
                    mentes.Close();
                }
                catch (Exception)
                {

                    throw;
                }
                mentes.Dispose();
            }
        }
        private void btnNevekRendezese_Click(object sender, RoutedEventArgs e)
        {
            lbGeneraltNevek.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            stbRendezes.Content = "Rendezett névsor";
        }
        private void NevlistaVegereUgras()
        {
            lbCsaladnevek.Items.MoveCurrentToLast();
            lbCsaladnevek.ScrollIntoView(lbCsaladnevek.Items.CurrentItem);
            lbUtonevek.Items.MoveCurrentToLast();
            lbUtonevek.ScrollIntoView(lbUtonevek.Items.CurrentItem);
        }
    }
}