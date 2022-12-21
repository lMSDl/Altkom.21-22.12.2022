using System.Collections.ObjectModel;

namespace Models
{
    public class Order : Entity
    {
        public virtual DateTime DateTime { get; set; }
        public virtual IList<Product> Products { get; set; } = new ObservableCollection<Product>();
    }
}