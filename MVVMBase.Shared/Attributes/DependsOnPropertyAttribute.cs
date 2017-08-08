using System;
using System.Collections.Generic;

namespace MVVMBase
{
    /// <summary>
    /// Depends on attribute.
    /// </summary>
    public class DependsOnPropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the source property.
        /// </summary>
        /// <value>The source property.</value>
        public IEnumerable<String> SourceProperties { get; set; }

        /// <summary>
        /// Initializes a new instance of the DependsOnAttribute class.
        /// </summary>
        public DependsOnPropertyAttribute(params string[] propertyNames)
        {
            SourceProperties = propertyNames;
        }

    }

}
