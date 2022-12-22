using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Order : Entity
    {
        private ILazyLoader _lazyLoader;
        private IList<Product> products;

        public Order(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        public Order()
        {
        }



        //Token współbieżności
        //[ConcurrencyCheck]
        public virtual DateTime DateTime { get; set; }
        //public virtual IList<Product> Products { get; set; }// = new ObservableCollection<Product>();


        public virtual IList<Product> Products { get => _lazyLoader?.Load(this, ref products) ?? products; set => products = value; }

        public OrderTypes OrderType { get; set; }
    }
}