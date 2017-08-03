using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMBase
{
    /// <summary>
    /// Depends on attribute.
    /// </summary>
    public class DependsOnAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the source property.
        /// </summary>
        /// <value>The source property.</value>
        public IEnumerable<String> SourceProperties { get; set; }

        /// <summary>
        /// Initializes a new instance of the DependsOnAttribute class.
        /// </summary>
        public DependsOnAttribute(params string[] propertyNames)
        {
            SourceProperties = propertyNames;
        }

    }

}
