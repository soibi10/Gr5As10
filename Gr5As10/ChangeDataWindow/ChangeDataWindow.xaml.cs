using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Windows;

namespace Gr5As10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChangeDataWindow : Window
    {

        private bool isNewData = true; //
        private City originCity;

        public ChangeDataWindow()
        {
            InitializeComponent();
        }

        //render new data for city
        public void RenderCity(City city)
        {
            City_TextBox.Text = city.Name;
            Population_TextBox.Text = city.Population.ToString();

            this.isNewData = false;
            this.originCity = city;
        }

        //inserting new record 
        private void InsertCity(City city)
        {
            bool isAdded = DBSQLite.GetInstance().AddCity(city);

            if (isAdded)
            {
                Close();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            if(string.IsNullOrEmpty(City_TextBox.Text) || 
                string.IsNullOrEmpty(Population_TextBox.Text))
            {
                MessageBox.Show("Input values");
                return;
            }

            double population;

            if(!double.TryParse(Population_TextBox.Text, out population))
            {
                MessageBox.Show("Invalid input");
                return;
            }

            City city = new City(City_TextBox.Text, population);

            if (isNewData)
            {
                InsertCity(city);
            }
            else
            {
                DBSQLite.GetInstance().UpdateCity(originCity, city);
                Close();
            }
        }

        private void Population_TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$"); //prevents user from entering non-digit values
            bool isMatching = !regex.IsMatch(Population_TextBox.Text.Insert(Population_TextBox.SelectionStart, e.Text));

            e.Handled = isMatching;
        }
    }

}
