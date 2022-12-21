using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Product : Entity, INotifyPropertyChanged
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


        public event PropertyChangedEventHandler? PropertyChanged;

        private void Set<T>(T value, out T output, [CallerMemberName] string propertyName = "")
        {
            output = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
