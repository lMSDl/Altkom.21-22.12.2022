using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Product : Entity
    {
        private string name;
        //private float price;
        //private float _price;
        //private float m_price;
        private float alamakota;

        public virtual string Name { get => name; set => Set(value, out name); }

        public virtual float Price
        {
            get => alamakota;
            set
            {
                alamakota = value;
                OnPropertyChanged();
            }
        }

        public virtual IEnumerable<Order>? Orders { get; set; }

        public string Info { get; }


        //Token współbieżności za pomocą sygnatury czasowej
        //[Timestamp]
        public byte[] Timestamp { get; set; }

        //shadow property
        /*public int? ManufacturerId { get; set; }*/
        public virtual Manufacturer? Manufacturer { get; set; }

        public virtual ProductDetails? ProductDetails { get; set; }

        public string? Description { get; set; }
    }
}
