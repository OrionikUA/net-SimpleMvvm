using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleMvvm.Annotations;

namespace SimpleMvvm
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> _dictionary;
        protected BaseViewModel()
        {
            _dictionary = new Dictionary<string, object>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public T Get<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null || !_dictionary.ContainsKey(propertyName)) return default;
            return (T)_dictionary[propertyName];
        }

        public void Set<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) return;

            if (!_dictionary.ContainsKey(propertyName))
            {
                _dictionary.Add(propertyName, newValue);
            }
            else if (EqualityComparer<T>.Default.Equals((T)_dictionary[propertyName], newValue))
            {
                return;
            }
            _dictionary[propertyName] = newValue;
            OnPropertyChanged(propertyName);
        }

        public void Set<T>(T newValue, out T field, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}
