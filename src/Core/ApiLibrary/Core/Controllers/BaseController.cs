using ApiLibrary.Core.Attributes;
using ApiLibrary.Core.Entity;
using ApiLibrary.Core.Exceptions;
using ApiLibrary.Core.Extensions;
using ApiLibrary.Core.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibrary.Core.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseController<TModel, TKey, TContext> : ControllerBase where TContext : BaseDbContext where TModel : BaseModel<TKey>
    {
        protected readonly TContext _db;

        public abstract int AcceptRange { get; set; }

        public BaseController(TContext db)
        {
            _db = db;
        }

        protected PartialResult Partial(object o)
        {
            return new PartialResult(o);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<ActionResult<TModel>> GetItemByIdAsync([FromRoute] TKey id)
        {
            var item = await _db.Set<TModel>().FindAsync(id);
            //var product = await _db.Products.SingleOrDefaultAsync(x => x.ID == id);
            if (item == null)
                return NotFound();
            //return Ok(product);*/
            return Ok(item);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<object>), (int)HttpStatusCode.PartialContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public virtual async Task<ActionResult<IEnumerable<TModel>>> GetItemsAsync([FromQuery] QueryParams param)
        {
            /*DateTime dt = DateTime.Now;
            var test = dt.FirstDayOfWeek();
            var fisrt = DateTimeExtensions.FirstDayOfWeek(dt);*/

            /*var list = new List<Product>();
            list.Where(x => x.ID == 1);*/

            IQueryable<TModel> query = _db.Set<TModel>().AsQueryable<TModel>();
            
            query = query.Where(x => x.DeletedAt == null);


            

            if (param.IsSort)
            {
                try
                {
                    
                    var tab = param.Sort.Split(',');
                    
                    foreach (var item in tab)
                    {
                        var property = typeof(TModel).GetProperty(item, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        query = query.OrderBy(property.Name);
                    }
                   
                    /*foreach (var item in tab)
                    {
                        if (query is IOrderedQueryable)
                        {
                            ((IOrderedQueryable<TModel>)query).ThenBy(x => x.GetType().GetProperty(item, BindingFlags.IgnoreCase));
                        }
                        else if (query is Queryable)
                        {
                            ((IOrderedQueryable<TModel>)query).OrderBy(x => x.GetType().GetProperty(item, BindingFlags.IgnoreCase));
                        }
                    }*/
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            if (param.IsRange)
            {
                try
                {
                    var tab = param.Range.Split('-');
                    int start = Convert.ToInt32(tab[0]);
                    int end = Convert.ToInt32(tab[1]);
                    int count = end - start;
                    if (count > AcceptRange)
                    {
                        throw new RangeException($"Requested range not allowed. Max {AcceptRange}");
                    }

                    int total = query.Count();

                    query = query.Range(start, count + 1);
                    string url = $"{Request.Scheme}://{Request.Host}{Request.Path}";

                    string links = $"<{url}?range=0-{count}>; rel=\"first\"," +
                                    $"<{url}?range = {(start - count < 0 ? 0 : start - count)}-{(start - count < 0 ? 0 : start - count) + count - 1}>; rel =\"prev\"," +
                                    $"<{url}?range = {end + 1}-{(end + count > total - 1 ? total : end + count - 1)}>; rel =\"next\"," +
                                    $"<{url}?range = {total - count + 1}-{total - 1}>; rel =\"last\"";
                    Response.Headers.Add("Link", links);
                    Response.Headers.Add("Content-Range", $"{start}-{end}/{total}");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }


            IEnumerable<object> result = null;
            if (param.IsSelect)
            {
                result = query.SelectDynamic(param.Fields);

            }
            else
            {
                result = await query.ToListAsync();
            }

            Response.Headers.Add("Accept-Range", AcceptRange.ToString());

            if (param.IsRange)
                return Partial(result);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public virtual async Task<ActionResult> PostProduct([FromBody] TModel item)
        {
            //ModelState.Remove("Name");
            if (ModelState.IsValid)
            {
                _db.Set<TModel>().Add(item);
                await _db.SaveChangesAsync();

                return Created("", item);
            }
            else
                return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public virtual async Task<ActionResult> PutProduct([FromBody] TModel item, [FromRoute] TKey id)
        {
            if (id.Equals(item.ID))
                return BadRequest();
            //var properties = typeof(Product).GetProperties();
            //var p = _db.Products.Find(id);
            /*if(p == null)
                return BadRequest();*/

            if (ModelState.IsValid)
            {
                //product.Map(p);
                _db.Set<TModel>().Update(item);

                //_db.Entry(product).Property(x => x.CreatedAt).IsModified = false;

                await _db.SaveChangesAsync();

                return NoContent();
            }
            else
                return BadRequest(ModelState);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public virtual async Task<ActionResult> DeleteItemAsync([FromRoute] TKey id)
        {
            var item = await _db.FindAsync<TModel>(id);
            if (item == null)
                return NotFound();
            _db.Remove(item);
            //product.DeletedAt = DateTime.Now;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
