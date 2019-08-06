using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Gr5As10
{
    //custom radiobutton class for city
    class CityRadioButton: RadioButton
    {
        public City City { private set; get; }

        public CityRadioButton(City city)
        {
            this.City = city;
            Content = city.Name + " population is:\t\t" + String.Format("{0:n}", city.Population);
        }
    }
    
}