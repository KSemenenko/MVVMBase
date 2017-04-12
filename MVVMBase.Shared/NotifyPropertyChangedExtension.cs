using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MVVMBase
{
    /// <summary>
    /// BaseViewModelExtension
    /// </summary>
    public static class NotifyPropertyChangedExtension
    {
        private static void RaisePropertyChanged(PropertyChangedEventHandler eventHandler, INotifyPropertyChanged notifyPropertyChanged, string propertyName)
        {
            Volatile.Read(ref eventHandler)?.Invoke(notifyPropertyChanged, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Changes the property and call the PropertyChanged even
        /// </summary>
        /// <param name="notifyPropertyChanged">INotifyPropertyChanged</param>
        /// <param name="eventHandler">PropertyChangedEventHandler</param>
        /// <param name="propertyName">Property Name</param>
        public static void RaiseOnPropertyChanged(this INotifyPropertyChanged notifyPropertyChanged, PropertyChangedEventHandler eventHandler, string propertyName)
        {
            RaisePropertyChanged(eventHandler, notifyPropertyChanged, propertyName);
        }

        /// <summary>
        ///     Changes the property and call the PropertyChanged event for this property name.
        /// </summary>
        [SuppressMessage("ReSharper", "MethodOverloadWithOptionalParameter")]
        public static void RaiseOnPropertyChanged(this INotifyPropertyChanged notifyPropertyChanged, PropertyChangedEventHandler eventHandler, [CallerMemberName] string hiddenPropertyName = null, bool hiddenCallMemberName = true)
        {
            RaisePropertyChanged(eventHandler, notifyPropertyChanged, hiddenPropertyName);
        }

        /// <summary>
        ///     Changes the property and call the PropertyChanged event for this property name.
        /// </summary>
        public static void RaiseOnPropertyChanged(this INotifyPropertyChanged notifyPropertyChanged, PropertyChangedEventHandler eventHandler, params string[] propertyNames)
        {
            foreach (var name in propertyNames)
            {
                RaisePropertyChanged(eventHandler, notifyPropertyChanged, name);
            }
        }
    }
}