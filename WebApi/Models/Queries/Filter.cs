using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Queries
{
    public class Filter
    {
        /// <summary>
        /// Specific order on Where
        /// </summary>
        public int Order { get; set; } = 1;

        /// <summary>
        /// Column Name from the table
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// Unary operator to validate the condition (equals, different, mayor, minor...)
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Exact value to place
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Condition to validate the filter, (AND, OR)
        /// </summary>
        public string Condition { get; set; } = "";
    }
}
