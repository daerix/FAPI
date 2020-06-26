using ApiLibrary.Core.Attributes;
using ApiLibrary.Core.Entities;
using ApiLibrary.Core.Exceptions;
using ApiLibrary.Core.Extensions;
using ApiLibrary.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;


namespace ApiLibrary.Core.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [AcceptRanges(25)]
    [SortQueryKey("Sort")]
    [FieldQueryKey("Field")]
    [RangeQueryKey("Range")]
    public abstract class BaseController<TModel, TKey, TContext> : ControllerBase
        where TContext : BaseDbContext
        where TModel : BaseModel<TKey>
    {
        protected readonly TContext _db;

        public BaseController(TContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        //TODO: Revoir QeryParams!
        public virtual async Task<ActionResult<IEnumerable<TModel>>> GetItemsAsync([FromQuery] QueryParams param)
        {
            IQueryable<TModel> query = _db.Set<TModel>().AsQueryable<TModel>();
            var acceptRange = this.GetType().GetCustomAttribute<AcceptRangesAttribute>().AcceptRange;

            query = query.Where(x => x.DeletedAt == null);

            if (param.IsProperty)
            {
                foreach (var item in param.Properties)
                {
                    try
                    {
                        var property = typeof(TModel).GetProperty(item.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        var values = item.Value.Split(',');
                        if (values.Length == 1 && values[0].Contains("*"))
                        {
                            if (property.PropertyType != typeof(string))
                            {
                                throw new SearchException($"The requested research is not allowed on this type of property ({ property.PropertyType }). Accepted type: String");
                            }
                            query = query.Search(property.Name, values[0]);
                        }
                        else if (values.Length == 2 && values[0].Contains("[") && values[1].Contains("]"))
                        {
                            if (property.PropertyType == typeof(string))
                            {
                                throw new ForkException($"The requested fork is not allowed on this type of property ({ property.PropertyType }). Accepted type: Integer, Decimal, DateTime");
                            }
                            query = query.Fork(property.Name, values);
                        }
                        else
                        {
                            query = query.Filter(property.Name, values);
                        }
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e);
                    }
                }
            }

            if (param.IsSort)
            {
                try
                {
                    var field = param.Sort.Split(',');
                    query = query.Sort(field);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            if (Response != null)
            {
                Response.Headers.Add("Accept-Range", acceptRange.ToString());
            }

            if (param.IsRange)
            {
                try
                {
                    var tab = param.Range.Split('-');
                    var start = Convert.ToInt32(tab[0]);
                    var end = Convert.ToInt32(tab[1]);

                    if (start > end)
                    {
                        var temp = start;
                        start = end;
                        end = temp;
                    }

                    var count = end - start;

                    if (count > acceptRange)
                    {
                        throw new RangeException($"Requested range not allowed. Max {acceptRange}");
                    }

                    var total = query.Count();
                    query = query.Range(start, end);

                    string url = $"{Request.Scheme}://{Request.Host}{Request.Path}";


                    string links = $"<{url}?range=0-{count}>; rel=\"first\"," +
                                    $"<{url}?range = {(start - count < 0 ? 0 : start - count)}-{(start - count < 0 ? 0 : start - count) + count}>; rel =\"prev\"," +
                                    $"<{url}?range = {(end + 1 > total - 1 ? total - 1 : end + 1)}-{(end + count > total - 1 ? total - 1 : end + count + 1)}>; rel =\"next\"," +
                                    $"<{url}?range = {total - count }-{total - 1}>; rel =\"last\"";
                    Response.Headers.Add("Link", links);
                    Response.Headers.Add("Content-Range", $"{start}-{end}/{total}");

                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }

            IEnumerable<object> result = null;
            if (param.IsSelect)
            {
                try
                {
                    var field = param.Fields.ToString().Split(',');
                    result = query.Field(field);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                try
                {
                    result = await query.ToListAsync();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            if (param.IsRange)
            {
                return Partial(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<Object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<ActionResult<TModel>> GetItemByIdAsync([FromRoute] TKey id, [FromQuery] bool deepFetch = false)
        {
            IQueryable<TModel> query = _db.Set<TModel>().AsQueryable<TModel>();
            query = query.Where(x => x.Id.Equals(id));
            if (deepFetch)
            {
                var fetchProperties = typeof(TModel).GetProperties().Where(x => x.GetCustomAttribute(typeof(ForeignKeyAttribute)) != null);
                foreach (var property in fetchProperties)
                {
                    query = query.Include(property.Name);
                }
            }

            if (query.Count() != 0)
            {
                var item = await query.FirstAsync();
                return Ok(item);
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> PostItemAsync([FromBody]TModel item)
        {
            if (ModelState.IsValid)
            {
                _db.Set<TModel>().Add(item);
                await _db.SaveChangesAsync();
                return Created("", item);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public virtual async Task<ActionResult> PutItemAsync([FromBody]TModel item, [FromRoute] TKey id)
        {
            if (!id.Equals(item.Id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _db.Update<TModel>(item);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<ActionResult> DeleteItemAsync([FromRoute]TKey id)
        {
            var item = await _db.FindAsync<TModel>(id);
            if (item != null)
            {
                _db.Remove<TModel>(item);
                await _db.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        protected PartialResult Partial(object o)
        {
            return new PartialResult(o);
        }

    }
}
