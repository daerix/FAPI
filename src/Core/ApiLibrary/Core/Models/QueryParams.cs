using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Core.Models
{
    public class QueryParams
    {
        public string Range { get; set; }

        public string Sort { get; set; }

        public string Fields { get; set; }

        public bool IsRange
        {
            get { return !string.IsNullOrWhiteSpace(Range); }
        }

        public bool IsSort
        {
            get { return !string.IsNullOrWhiteSpace(Sort); }
        }

        public bool IsSelect
        {
            get { return !string.IsNullOrWhiteSpace(Fields); }
        }
    }
}
