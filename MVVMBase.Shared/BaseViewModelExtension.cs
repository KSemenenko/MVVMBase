using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace MVVMBase
{
    public static class BaseViewModelExtension
    {
        public static void BindToPropertyChange<T>(this BaseViewModel viewModel, Expression<Func<T>> action, params Expression<Func<T>>[] actions)
        {
          
        }

        public static void BindToPropertyChange(this BaseViewModel viewModel, string action, params string[] actions)
        {

        }

        public static void BindToPropertyChange<T>(this BaseViewModel viewModel, Expression<Func<T>> action, params string[] actions)
        {

        }

        public static void BindToPropertyChange<T>(this BaseViewModel viewModel, string action, params Expression<Func<T>>[] actions)
        { 

        }

    }
}
