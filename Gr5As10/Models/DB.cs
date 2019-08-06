using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Windows;

namespace Gr5As10
{
    public class DBSQLite
    {
        SQLiteConnection conn = null;
        const string CONNECT_STRING = "data source=PopulationDB.db3";

        static DBSQLite _dbsqlite;

        private DBSQLite()
        {
            conn = new SQLiteConnection(CONNECT_STRING);
            conn.Open();
        }

        public static DBSQLite GetInstance() //singleton
        {
            if (_dbsqlite == null)
                _dbsqlite = new DBSQLite();
            return _dbsqlite;
        }

        public List<City> FetchCities()
        {
            var query = "SELECT * FROM city";
            var cmd = new SQLiteCommand(query, conn);
            SQLiteDataReader rd = cmd.ExecuteReader();

            List<City> cities = new List<City>();

            while (rd.Read())
            {
                string cityName = rd.GetString(rd.GetOrdinal("City"));
                double cityPopulation = rd.GetDouble(rd.GetOrdinal("Population"));

                City city = new City(cityName, cityPopulation);

                cities.Add(city);
            }

            rd.Close();

            return cities;
        }

        public double FetchTotalPopulation()
        {
            var query = "SELECT SUM(Population) FROM city";
            var cmd = new SQLiteCommand(query, conn);

            SQLiteDataReader rd = cmd.ExecuteReader();

            double sum = 0;
            while (rd.Read())
            {
                sum = rd.GetDouble(rd.GetOrdinal("SUM(Population)"));
            }

            rd.Close();

            return sum;
        }


        public City FetchMostPopulatedCity()
        {
            var query = "SELECT MAX(Population), City FROM City";
            var cmd = new SQLiteCommand(query, conn);
            SQLiteDataReader rd = cmd.ExecuteReader();

            rd.Read();
            
            string cityName = rd.GetString(rd.GetOrdinal("City"));
            double highestPopul = rd.GetDouble(rd.GetOrdinal("MAX(Population)"));

            City city = new City(cityName, highestPopul);

            rd.Close();

            return city;

        }

        public void DeleteCity(City city)
        {
            
            var cmdString = "DELETE FROM city " +
                                 "WHERE City = @City";

            var cmd = new SQLiteCommand(cmdString, conn);
            cmd.Parameters.AddWithValue("@City", city.Name);

            cmd.ExecuteNonQuery();
           
        }

        public bool AddCity(City city)
        {
            var query = "INSERT INTO city " +
                                    "(City, Population)" +
                                    "VALUES" +
                                    "(@City, @Population)";

            var cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@City", city.Name);
            cmd.Parameters.AddWithValue("@Population", city.Population);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public void UpdateCity(City originCity, City modifiedCity)
        {
            var query = "UPDATE city SET City = @City, Population = @Population WHERE City = @OriginCity";

            var cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@City", modifiedCity.Name);
            cmd.Parameters.AddWithValue("@Population", modifiedCity.Population);
            cmd.Parameters.AddWithValue("@OriginCity", originCity.Name);

            cmd.ExecuteNonQuery();
        }
    }
}