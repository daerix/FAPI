using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using ApiLibrary.Core.Extensions;
using ApiLibrary.Test.Mocks;

namespace ApiLibrary.Test
{
    public class IQueryableExtensionsTest
    {
        private static BaseDbContextMock _db = BaseDbContextMock.GetDbContext();

        [Fact]
        public void elements_by_range()
        {
            var query = _db.Models.AsQueryable();

            var result = query.Range(0, 1).ToList();
            Assert.Equal(2, result.Count());
            Assert.True(result.SequenceEqual(_db.Models.Skip(0).Take(2).ToList()));
        }

        [Fact]
        public void one_element_when_limit_of_range()
        {
            var query = _db.Models.AsQueryable();

            var result = query.Range(5, 6).ToList();
            Assert.Single(result);
            Assert.True(result.SequenceEqual(_db.Models.Skip(5).Take(2).ToList()));
        }

         [Fact]
        public void empty_when_out_of_range()
        {
            var query = _db.Models.AsQueryable();

            var test = query.ToList();

            var result = query.Range(7, 8).ToList();
            Assert.Empty(result);
        }

        [Fact]
        public void sorted_elements_once()
        {
            var query = _db.Models.AsQueryable();

            var result = query.Sort(new[] { "String" }).ToList();
            Assert.True(result.SequenceEqual(query.OrderBy(x => x.String).ToList()));
        }

        [Fact]
        public void sorted_elements_once_by_descending()
        {
            var query = _db.Models.AsQueryable();

            var result = query.Sort(new[] { "String desc" }).ToList();
            Assert.True(result.SequenceEqual(query.OrderByDescending(x => x.String).ToList()));
        }

        [Fact]
        public void elements_sorted_twice()
        {
            var query = _db.Models.AsQueryable();

            var result = query.Sort(new[] { "Integer", "String" }).ToList();
            Assert.True(result.SequenceEqual(query.OrderBy(x => x.Integer).ThenBy(x => x.String).ToList()));
        }

        [Fact]
        public void elements_sorted_twice_by_descending()
        {
            var query = _db.Models.AsQueryable();

            var result = query.Sort(new[] { "Integer desc", "String desc" }).ToList();
            Assert.True(result.SequenceEqual(query.OrderByDescending(x => x.Integer).ThenByDescending(x => x.String).ToList()));
        }

        [Fact]
        public void elements_sorted_twice_by_both()
        {
            var query = _db.Models.AsQueryable();

            var result = query.Sort(new[] { "Integer desc", "String" }).ToList();
            Assert.True(result.SequenceEqual(query.OrderByDescending(x => x.Integer).ThenBy(x => x.String).ToList()));
        }

        [Fact]
        public void argumentexception_when_sort_error()
        {
            var query = _db.Models.AsQueryable();
            Assert.Throws<ArgumentException>(() => query.Sort(new[] { "Toto" }).ToList());
        }


        [Fact]
        public void elements_with_specific_fields()
        {

            var query = _db.Models.AsQueryable();
            var result = query.Field(new[] { "String", "Integer" }).ToList();

            foreach(dynamic data in result)
            {
                var dataDictionary = (IDictionary<string, object>)data;
                Assert.True(dataDictionary.ContainsKey("String"));
                Assert.True(dataDictionary.ContainsKey("Integer"));
                Assert.False(dataDictionary.ContainsKey("Date"));
                Assert.False(dataDictionary.ContainsKey("Id"));
                Assert.False(dataDictionary.ContainsKey("Double"));
                Assert.False(dataDictionary.ContainsKey("Decimal"));
            }
        }

        [Fact]
        public void argumentexception_when_specific_fields_error()
        {
            var query = _db.Models.AsQueryable();
            Assert.Throws<ArgumentException>(() => query.Field(new[] { "Toto" }).ToList());
        }

        [Fact]
        public void elements_when_search_start_with_text()
        {
            var query = _db.Models.AsQueryable();
            var result = query.Search("String", "String*").ToList();

            Assert.Equal(6, result.Count());
            Assert.True(result.SequenceEqual(query.Where(x => x.String.StartsWith("String")).ToList()));
        }

        [Fact]
        public void elements_when_search_end_with_text()
        {
            var query = _db.Models.AsQueryable();
            var result = query.Search("String", "*1").ToList();

            Assert.Single(result);
            Assert.True(result.SequenceEqual(query.Where(x => x.String.EndsWith("1")).ToList()));
        }

        [Fact]
        public void elements_when_search_contains_text()
        {
            var query = _db.Models.AsQueryable();
            var result = query.Search("String", "*Test*").ToList();

            Assert.Single(result);
            Assert.True(result.SequenceEqual(query.Where(x => x.String.Contains("Test")).ToList()));
        }

        [Fact]
        public void argumentexception_when_search_error()
        {
            var query = _db.Models.AsQueryable();
            Assert.Throws<ArgumentException>(() => query.Search("Toto","Test*").ToList());
        }

        [Fact]
        public void elements_when_filter_success()
        {
            var query = _db.Models.AsQueryable();
            var result = query.Filter("Integer", new[] { "1" }).ToList();

            Assert.Equal(2, result.Count());
            Assert.True(result.SequenceEqual(query.Where(x => x.Integer == 1).ToList()));
        }

        [Fact]
        public void argumentexception_when_filter_error()
        {
            var query = _db.Models.AsQueryable();
            Assert.Throws<ArgumentException>(() => query.Filter("Toto", new[] { "Test" }).ToList());
        }

        [Fact]
        public void elements_when_fork_contains_number()
        {
            var query = _db.Models.AsQueryable();
            var result = query.Fork("Integer", new[] { "2", "3" }).ToList();

            Assert.Equal(4, result.Count());
            Assert.True(result.SequenceEqual(query.Where(x => x.Integer >= 2 && x.Integer <= 3).ToList()));
        }

        [Fact]
        public void elements_when_fork_start_with_number()
        {
            var query = _db.Models.AsQueryable();
            var result = query.Fork("Integer", new[] { "1", "" }).ToList();

            Assert.Equal(6, result.Count());
            Assert.True(result.SequenceEqual(query.Where(x => x.Integer >= 1).ToList()));
        }

        [Fact]
        public void elements_when_fork_end_with_number()
        {
            var query = _db.Models.AsQueryable();
            var result = query.Fork("Integer", new[] { "", "2" }).ToList();

            Assert.Equal(4, result.Count());
            Assert.True(result.SequenceEqual(query.Where(x => x.Integer <= 2).ToList()));
        }

        [Fact]
        public void elements_when_fork_contains_date()
        {
            var query = _db.Models.AsQueryable();
            var result = query.Fork("Date", new[] { "02/02/0002", "03/03/0003" }).ToList();

            Assert.Equal(4, result.Count());
            Assert.True(result.SequenceEqual(query.Where(x => x.Date >= new DateTime(2, 2, 2) && x.Date <= new DateTime(3, 3, 3)).ToList()));
        }

        [Fact]
        public void elements_when_fork_start_with_date()
        {
            var query = _db.Models.AsQueryable();
            var result = query.Fork("Date", new[] { "01/01/0001", "" }).ToList();

            Assert.Equal(6, result.Count());
            Assert.True(result.SequenceEqual(query.Where(x => x.Date >= new DateTime(1, 1, 1)).ToList()));

        }

        [Fact]
        public void elements_when_fork_end_with_date()
        {
            var query = _db.Models.AsQueryable();
            var result = query.Fork("Date", new[] { "", "02/02/0002" }).ToList();

            Assert.Equal(4, result.Count());
            Assert.True(result.SequenceEqual(query.Where(x => x.Date <= new DateTime(2, 2, 2)).ToList()));
        }

        [Fact]
        public void argumentexception_when_fork_error()
        {
            var query = _db.Models.AsQueryable();
            Assert.Throws<ArgumentException>(() => query.Fork("Toto", new[] { "Test" }).ToList());
        }

    }
}
