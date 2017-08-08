using System;
using System.Collections.Generic;

namespace MVVMBase
{
    /// <summary>
    /// Depends on attribute.
    /// </summary>
    public class NotifyPropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the source property.
        /// </summary>
        /// <value>The source property.</value>
        public IEnumerable<String> SourceProperties { get; set; }

        /// <summary>
        /// Initializes a new instance of the NotifyAttribute class.
        /// </summary>
        public NotifyPropertyAttribute(params string[] propertyNames)
        {
            SourceProperties = propertyNames;
        }

    }

}
