using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CALORY
{
    public class Product
    {
        [Key]
        public Int16 code { get; set; }
        public DateTime? day { get; set; }
        public string? ration { get; set; }
        public string? name { get; set; }
        public double kkal { get; set; }
        public string? loginUser { get; set; }
        public double ugl { get; set; }
        public double fats { get; set; }
        public double bel { get; set; }
        private static Int16 gramm = 100;
        public Int16 gram
        {
            get
            {
                return gramm;
            }
            set
            {
                if (value != 0)
                    gramm = value;
            }
        }
        public void Recalculate()
        {
            kkal = Math.Round(kkal * gramm / 100, 2);
            bel = Math.Round(bel * gramm / 100, 2);
            ugl = Math.Round(ugl * gramm / 100, 2);
            fats = Math.Round(fats * gramm / 100, 2);
        }

        public Product Copy()
        {
            return new Product(name, gramm.ToString(), kkal.ToString(), bel.ToString(), fats.ToString(), ugl.ToString());
        }
        public override string ToString()
        {
            return name;
        }
        public string ToStringFull()
        {
            return name + " " + gram + " г. " + " - " + kkal + " ккал., " + bel + " г. бел., " + fats + " г. жир., " + ugl + " г. угл.";
        }
        public Product(string _name, string _gram, string _kkal, string _bel, string _fat, string _ugl)
        {
            name = _name;
            kkal = double.Parse(_kkal);
            bel = double.Parse(_bel);
            fats = double.Parse(_fat);
            ugl = double.Parse(_ugl);
            gramm = Int16.Parse(_gram);
        }
        public Product() { }
    }
}
