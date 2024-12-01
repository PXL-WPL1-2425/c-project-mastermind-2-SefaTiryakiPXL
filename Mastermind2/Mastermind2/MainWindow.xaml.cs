using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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

namespace Mastermind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string TitelAppears1;
        private string TitelAppears2;
        private string TitelAppears3;
        private string TitelAppears4;

        string Titel;
        int attempts = 0;

        string[,] Historiek = new string[10, 5];
        // 10 =  rijgen van 5 kolemmen 4 van de kolommen zijn de kleuren en de laatste is de feedback
        public MainWindow()
        {
            InitializeComponent();
            TitelAppearsAbove();


            // array 
            string[] colors = { "rood", "geel", "groen", "oranje", "wit", "blauw" };

            foreach (var comboBox in new[] { ComboBox1, ComboBox2, ComboBox3, ComboBox4 })
            {
                foreach (var color in colors)
                {
                    // voegt voor elke combobox de naam color
                    comboBox.Items.Add(color);
                }
            }



        }

        private void TitelAppearsAbove()
        {
            Random rnd = new Random();
            string[] TitelAppears = new string[] { "rood", "geel", "groen", "oranje", "wit", "blauw" };
            TitelAppears1 = TitelAppears[rnd.Next(0, TitelAppears.Length)];
            TitelAppears2 = TitelAppears[rnd.Next(0, TitelAppears.Length)];
            TitelAppears3 = TitelAppears[rnd.Next(0, TitelAppears.Length)];
            TitelAppears4 = TitelAppears[rnd.Next(0, TitelAppears.Length)];

            Titel = TitelAppears1 + "," + TitelAppears2 + "," + TitelAppears3 + "," + TitelAppears4;


            this.Title = $"Mastermind ({Titel})";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string kleur1 = ComboBox1.SelectedItem?.ToString();
            string kleur2 = ComboBox2.SelectedItem?.ToString();
            string kleur3 = ComboBox3.SelectedItem?.ToString();
            string kleur4 = ComboBox4.SelectedItem?.ToString();

            attempts++;
            this.Title = $"Mastermind ({Titel}, poging: {attempts})";



            if (kleur1 == TitelAppears1 && kleur2 == TitelAppears2 && kleur3 == TitelAppears3 && kleur4 == TitelAppears4)
            {
                MessageBox.Show($"Code is juist!! In {attempts} pogingen");
                return; 
            }

            if (attempts >= 10)
            {
                MessageBox.Show($"Gefaald!! De code is: {Titel}");
                Close();
                
            }
            

            string[] correcteCode = { TitelAppears1, TitelAppears2, TitelAppears3, TitelAppears4 };
            string[] gokken = { kleur1, kleur2, kleur3, kleur4 };
            string feedback = "";


            // reset als er iets niet meer klopt
            ResetBorder();


            for (int i = 0; i < 4; i++)
            {
                if (gokken[i] == correcteCode[i]) 
                { 
                    SetBorderColor(i, Brushes.DarkRed); feedback += "J "; 
                }
                else if (correcteCode.Contains(gokken[i])) 
                { 
                SetBorderColor(i, Brushes.Wheat); feedback += "FP "; 
                }
                else 
                { 
                    feedback += "F "; 
                }
            }

            if (attempts <= 10)
            {
                Historiek[attempts - 1, 0] = kleur1;
                Historiek[attempts - 1, 1] = kleur2;
                Historiek[attempts - 1, 2] = kleur3;
                Historiek[attempts - 1, 3] = kleur4;
                Historiek[attempts - 1, 4] = feedback;
            }

            ListBoxHistoriek.Items.Clear();

            for (int i = 0; i < attempts; i++)
            {
                string feedbackString = $"{Historiek[i, 0]} ,{Historiek[i, 1]} ,{Historiek[i, 2]} ,{Historiek[i, 3]} -> {Historiek[i, 4]}";
                ListBoxHistoriek.Items.Add(feedbackString);
            }

        }

        private void ResetBorder()
        {
            kleur1Border.BorderBrush = Brushes.Gray;
            kleur2Border.BorderBrush = Brushes.Gray;
            kleur3Border.BorderBrush = Brushes.Gray;
            kleur4Border.BorderBrush = Brushes.Gray;
        }

        private void SetBorderColor(int index, Brush color)
        {
            switch (index)
            {
                case 0:
                    kleur1Border.BorderBrush = color;
                    break;
                case 1:
                    kleur2Border.BorderBrush = color;
                    break;
                case 2:
                    kleur3Border.BorderBrush = color;
                    break;
                case 3:
                    kleur4Border.BorderBrush = color;
                    break;
            }
        }
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // kijkt of het een combobox is
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            string kleur = comboBox.SelectedItem.ToString();
            if (kleur == null) return;

            if (comboBox.Name == "ComboBox1")
            {
                Kleur1.Background = GetColor(kleur);
                TextBlock1.Text = $"Gekozen kleur: {kleur}";
            }
            else if (comboBox.Name == "ComboBox2")
            {
                Kleur2.Background = GetColor(kleur);
                TextBlock2.Text = $"Gekozen kleur: {kleur}";
            }
            else if (comboBox.Name == "ComboBox3")
            {
                Kleur3.Background = GetColor(kleur);
                TextBlock3.Text = $"Gekozen kleur: {kleur}";
            }
            else if (comboBox.Name == "ComboBox4")
            {
                Kleur4.Background = GetColor(kleur);
                TextBlock4.Text = $"Gekozen kleur: {kleur}";
            }
        }

        private Brush GetColor(string kleur)
        {
            switch (kleur.ToLower())
            {
                case "rood": 
                return Brushes.Red;

                case "geel": 
                return Brushes.Yellow;

                case "groen": 
                return Brushes.Green;

                case "oranje": 
                return Brushes.Orange;

                case "wit": 
                return Brushes.White;

                case "blauw": 
                return Brushes.Blue;

                default: 
                return Brushes.Transparent;

            }
        }

    }

}

