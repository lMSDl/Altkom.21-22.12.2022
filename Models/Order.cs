using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Order : Entity
    {
        //Token współbieżności
        //[ConcurrencyCheck]
        public virtual DateTime DateTime { get; set; }
        public virtual IList<Product> Products { get; set; } = new ObservableCollection<Product>();

    }
}