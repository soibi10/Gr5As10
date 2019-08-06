using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gr5As10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private City selectedCity;

        public MainWindow()
        {
            InitializeComponent();

            ReloadCitiesData();
        }

        //Population of the listbox and labels with data
        private void ReloadCitiesData()
        {
            LoadCities();
            PrepareTotalPopulation();
            PrepareHighestPopulation();
        }

        //creating list of data from database with radiobuttons
        private void LoadCities()
        {
            DataBase_ListBox.Items.Clear();

            List<City> cities = DBSQLite.GetInstance().FetchCities();

            foreach (var city in cities)
            {
                //using custom radiobutton
                CityRadioButton cityRadioButton = new CityRadioButton(city);
                cityRadioButton.Click += CityRadioButton_Click;
                DataBase_ListBox.Items.Add(cityRadioButton);
            }

        }

        private void PrepareTotalPopulation()
        {
            double totalPopulation = DBSQLite.GetInstance().FetchTotalPopulation();
            TotalPopulation_Label.Content = "Total population:\t" + String.Format("{0:n}", totalPopulation);
        }

        private void PrepareHighestPopulation()
        {
            City city = DBSQLite.GetInstance().FetchMostPopulatedCity();
            HighestPopulation_Label.Content = "Highest population in " + city.Name + " - " + String.Format("{0:n}", city.Population);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DBSQLite.GetInstance().DeleteCity(selectedCity);
            ReloadCitiesData();
        }

        private void CityRadioButton_Click(object sender, RoutedEventArgs e)
        {
            CityRadioButton radioButton = sender as CityRadioButton;
            selectedCity = radioButton.City;

            Delete_Button.IsEnabled = true;
            ChangeData_Button.IsEnabled = true;
        }

        private void AddData_Click(object sender, RoutedEventArgs e)
        {
            ChangeDataWindow changeDataWindow = new ChangeDataWindow();
            changeDataWindow.Closed += ChangeData_Closed;
            changeDataWindow.Show();
        }

        private void ChangeData_Click(object sender, RoutedEventArgs e)
        {
            ChangeDataWindow changeDataWindow = new ChangeDataWindow();
            changeDataWindow.RenderCity(selectedCity);
            changeDataWindow.Closed += ChangeData_Closed;
            changeDataWindow.Show();
        }

        //reloading cities data when change data window closed
        private void ChangeData_Closed(object sender, EventArgs e)
        {
            ReloadCitiesData();
        }
    }
}
