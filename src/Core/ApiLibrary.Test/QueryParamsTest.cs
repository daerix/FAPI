using Xunit;
using ApiLibrary.Core.Models;

namespace ApiLibrary.Test
{
    public class QueryParamsTest
    {
        private QueryParams emptyParam = new QueryParams();
        private QueryParams lowerExistsParam = new QueryParams() { { "range", "2-3" }, { "sort", "Test" }, { "field", "Test" }, { "prop1", "Test" }, { "prop2", "Test" } };
        private QueryParams existsParam = new QueryParams() { { "Range", "2-3" }, { "Sort", "Test" }, { "Field", "Test" }, { "Prop1", "Test" }, { "Prop2", "Test" }, { "Prop3", "Test" } };

        [Fact]
        public void isrange_if_exists()
        {
            Assert.True(existsParam.IsRange);
        }

        [Fact]
        public void isrange_if_exists_to_lower()
        {
            Assert.True(lowerExistsParam.IsRange);
        }

        [Fact]
        public void isrange_if_not_exists()
        {
            Assert.False(emptyParam.IsRange);
        }

        [Fact]
        public void issort_if_exists()
        {
            Assert.True(existsParam.IsSort);
        }

        [Fact]
        public void issort_if_exists_to_lower()
        {
            Assert.True(lowerExistsParam.IsSort);
        }

        [Fact]
        public void issort_if_not_exists()
        {
            Assert.False(emptyParam.IsSort);
        }

        [Fact]
        public void isfield_if_exists()
        {
            Assert.True(existsParam.IsSelect);
        }

        [Fact]
        public void isfield_if_exists_to_lower()
        { 
            Assert.True(lowerExistsParam.IsSelect);
        }

        [Fact]
        public void isfield_if_not_exists()
        {
            Assert.False(emptyParam.IsSelect);
        }

        [Fact]
        public void isproperty_if_exists()
        {
            Assert.True(existsParam.IsProperty);
        }

        [Fact]
        public void isproperty_if_exists_to_lower()
        {
            Assert.True(lowerExistsParam.IsProperty);
        }

        [Fact]
        public void isproperty_if_not_exists()
        {
            Assert.False(emptyParam.IsProperty);
        }

        [Fact]
        public void range_if_exists()
        {
            Assert.Equal("2-3",existsParam.Range);
        }

        [Fact]
        public void range_if_exists_to_lower()
        {
            Assert.Equal("2-3", lowerExistsParam.Range);
        }

        [Fact]
        public void range_if_not_exists()
        {
            Assert.Null(emptyParam.Range);
        }

        [Fact]
        public void sort_if_exists()
        {
            Assert.Equal("Test", existsParam.Sort);
        }

        [Fact]
        public void sort_if_exists_to_lower()
        {
            Assert.Equal("Test", lowerExistsParam.Sort);
        }

        [Fact]
        public void sort_if_not_exists()
        {
            Assert.Null(emptyParam.Sort);
        }

        [Fact]
        public void field_if_exists()
        {
            Assert.Equal("Test", existsParam.Fields);
        }

        [Fact]
        public void field_if_exists_to_lower()
        {
            Assert.Equal("Test", lowerExistsParam.Fields);
        }

        [Fact]
        public void field_if_not_exists()
        {
            Assert.Null(emptyParam.Fields);
        }

        [Fact]
        public void property_if_exists()
        {
            Assert.Equal(3,existsParam.Properties.Count);
            Assert.False(existsParam.Properties.ContainsKey("Sort"));
            Assert.False(existsParam.Properties.ContainsKey("Field"));
            Assert.False(existsParam.Properties.ContainsKey("Range"));
            Assert.True(existsParam.Properties.ContainsKey("Prop1"));
            Assert.True(existsParam.Properties.ContainsKey("Prop2"));
            Assert.True(existsParam.Properties.ContainsKey("Prop3"));
        }

        [Fact]
        public void property_if_exists_to_lower()
        {
            Assert.Equal(2, lowerExistsParam.Properties.Count);
            Assert.False(lowerExistsParam.Properties.ContainsKey("sort"));
            Assert.False(lowerExistsParam.Properties.ContainsKey("field"));
            Assert.False(lowerExistsParam.Properties.ContainsKey("range"));
            Assert.True(lowerExistsParam.Properties.ContainsKey("prop1"));
            Assert.True(lowerExistsParam.Properties.ContainsKey("prop2"));
        }

        [Fact]
        public void property_if_not_exists()
        {
            Assert.Empty(emptyParam.Properties);
        }
    }
}
