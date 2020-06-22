using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ApiLibrary.Core.Extentions;
using ApiLibrary.Core.Entities;
using ApiLibrary.Core.Models;
using ApiLibrary.Core.Exceptions;
using ApiLibrary.Core.Attributes;
using System.Reflection;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;

namespace ApiLibrary.Core.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [AcceptRanges(25)]
    public abstract class BaseController<TModel, TKey, TContext> : ControllerBase 
        where TContext : BaseDbContext 
        where TModel : BaseModel<TKey>
    { 
        protected readonly TContext _db;
        public string sortKey = "sort";
        public string rangeKey = "range";
        public string fieldKey = "field";

        public BaseController(TContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<ActionResult> GetItemsAsync([FromQuery] Dictionary<string, string> param)
        {
            string paramsValue;
            IQueryable<TModel> query = _db.Set<TModel>().AsQueryable<TModel>();
            var acceptRange = this.GetType().GetCustomAttribute<AcceptRangesAttribute>().AcceptRange;
            var propeties = typeof(TModel).GetProperties();
            
            query = query.Where(x => x.DeletedAt == null);

            foreach (var property in propeties)
            {
                if (param.TryGetValue(property.Name.ToLower(), out paramsValue) || param.TryGetValue(property.Name, out paramsValue))
                {
                    var values = paramsValue.Split(',');
                    if (values.Count() == 1 && values[0].Contains("*"))
                    {
                        query = query.Search(property.Name, values[0]);
                    }
                    else
                    {
                        query = query.Filter(property.Name, values);
                    }
                }
            }

            if (param.TryGetValue(sortKey, out paramsValue))
            {
                try
                {
                    var field = paramsValue.Split(',');
                    query = query.Sort(field);
                } catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            if(Response != null)
            {
                Response.Headers.Add("Accept-Range", acceptRange.ToString());
            }

            if (param.TryGetValue(rangeKey, out paramsValue))
            {
                try
                {
                    var tab = paramsValue.Split('-');
                    var start = Convert.ToInt32(tab[0]);
                    var end = Convert.ToInt32(tab[1]);
                    var count = end - start;

                    if (count > acceptRange)
                    {
                        throw new RangeException($"Requested range not allowed. Max {acceptRange}");
                    }

                    var total = query.Count();
                    query = query.Range(start, end);

                    string url = $"{Request.Scheme}://{Request.Host}{Request.Path}";

                    string links = $"<{url}?range=0-{(end - start)}>; rel=\"first\"," +
                                    $"<{url}?range = {(start - (end - start) < 0 ? 0 : start - (end - start))}-{(start - (end - start) < 0 ? 0 : start - (end - start)) + (end - start) - 1}>; rel =\"prev\"," +
                                    $"<{url}?range = {end + 1}-{(end + (end - start) > total - 1 ? total : end + (end - start) - 1)}>; rel =\"next\"," +
                                    $"<{url}?range = {total - (end - start) + 1}-{total - 1}>; rel =\"last\"";
                    if (Response != null)
                    {
                        Response.Headers.Add("Link", links);
                        Response.Headers.Add("Content-Range", $"{start}-{end}/{total}");
                        Response.Headers.Add("Content-Length", $"{(count)}");
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            IEnumerable<object> result = null;
            if (param.TryGetValue(fieldKey, out paramsValue))
            {
                try
                {
                    var field = paramsValue.ToString().Split(',');
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

            if (param.ContainsKey(rangeKey))
            {
                return Partial(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<Object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<ActionResult> GetItemByIdAsync([FromRoute] object id)
        {
            var item = await _db.FindAsync<TModel>(id);
            if (item != null)
            {
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
            if (id.Equals(item.Id))
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
            }
            return NotFound();
        }


        protected PartialResult Partial(object o)
        {
            return new PartialResult(o);
        }

    }
}
