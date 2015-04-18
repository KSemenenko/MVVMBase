using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace MVVMBase
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> action)
        {
            string propertyName = GetPropertyName(action);
            this.OnPropertyChanged(propertyName);
        }

        private static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            string propertyName = expression.Member.Name;
            return propertyName;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}