using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    public abstract class Entity : INotifyPropertyChanged
    {
        public virtual int Id { get; set; }

        public DateTime CreatedAt { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Set<T>(T value, out T output, [CallerMemberName] string propertyName = "")
        {
            output = value;
            OnPropertyChanged(propertyName);
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}