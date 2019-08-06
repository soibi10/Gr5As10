using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Gr5As10
{

    public struct City
    {
        public string Name { get; private set; }
        public double Population { get; private set; }

        public City(string cityName, double cityPopulation)
        {
            Name = cityName;
            Population = cityPopulation;
        }
    }
}