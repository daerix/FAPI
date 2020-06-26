using ApiLibrary.Core.Attributes;
using System.Collections.Generic;
using System.Reflection;

namespace ApiLibrary.Core.Models
{

    [SortQueryKey("Sort")]
    [FieldQueryKey("Field")]
    [RangeQueryKey("Range")]
    public class QueryParams : Dictionary<string, string>
    {

        public Dictionary<string, string> Properties
        {
            get
            {
                var rangeKey = this.GetType().GetCustomAttribute<RangeQueryKeyAttribute>().RangeQueryKey;
                var sortKey = this.GetType().GetCustomAttribute<SortQueryKeyAttribute>().SortQueryKey;
                var fieldKey = this.GetType().GetCustomAttribute<FieldQueryKeyAttribute>().FieldQueryKey;
                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (var key in this.Keys)
                {
                    if ((key != rangeKey && key != rangeKey.ToLower()) && (key != sortKey && key != sortKey.ToLower()) && (key != fieldKey && key != fieldKey.ToLower()))
                    {
                        string value;
                        this.TryGetValue(key, out value);
                        values.Add(key, value);
                    }
                }
                return values;
            }
        }

        public string Range
        {
            get
            {
                var rangeKey = this.GetType().GetCustomAttribute<RangeQueryKeyAttribute>().RangeQueryKey;
                string value;
                this.TryGetValue(rangeKey, out value);
                if (value == null)
                {
                    this.TryGetValue(rangeKey.ToLower(), out value);
                }
                return value;
            }
        }

        public string Sort
        {
            get
            {
                var sortKey = this.GetType().GetCustomAttribute<SortQueryKeyAttribute>().SortQueryKey;
                string value;
                this.TryGetValue(sortKey, out value);
                if (value == null)
                {
                    this.TryGetValue(sortKey.ToLower(), out value);
                }
                return value;
            }
        }

        public string Fields
        {
            get
            {
                var fieldKey = this.GetType().GetCustomAttribute<FieldQueryKeyAttribute>().FieldQueryKey;
                string value;
                this.TryGetValue(fieldKey, out value);
                if (value == null)
                {
                    this.TryGetValue(fieldKey.ToLower(), out value);
                }
                return value;
            }
        }


        public bool IsRange
        {
            get
            {
                if (Range is null)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsSort
        {
            get
            {
                if (Sort is null)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsSelect
        {
            get
            {
                if (Fields is null)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsProperty
        {
            get
            {
                if (Properties.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
