﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Product : Entity
    {
        private string name;
        private float price;

        public virtual string Name { get => name; set => Set(value, out name); }
        public virtual float Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }

        //public IEnumerable<Order>? Orders { get; set; }
    }
}
