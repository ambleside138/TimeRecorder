using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace TimeRecorder.Helpers
{
    public static class ValidatableReactiveProperty
    {
        public static ReactiveProperty<T> Create<T>(T source)
        {
            return new ReactiveProperty<T>(source, ReactivePropertyMode.Default | ReactivePropertyMode.IgnoreInitialValidationError);
        }


        public static ReactiveProperty<TProperty> ToReactivePropertyWithIgnoreInitialValidationError<TSubject, TProperty>(this TSubject subject, Expression<Func<TSubject, TProperty>> propertySelector) where TSubject : INotifyPropertyChanged
        {
            return subject.ToReactivePropertyAsSynchronized(propertySelector, ReactivePropertyMode.Default | ReactivePropertyMode.IgnoreInitialValidationError);
        }

    }
}
