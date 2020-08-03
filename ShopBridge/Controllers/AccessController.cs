using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShopBridge.Models;

namespace ShopBridge.Controllers
{
 
    public class AccessController : ApiController
    {
        public AccessController()
        {

        }
        [HttpGet]
        //api to aceess all records
        public IHttpActionResult Index()
        {
            IList<ItemDTO> Item = null;
            using (var ctx = new DatabEntities())
            {
                Item = ctx.items.Select(s => new ItemDTO()
                            {
                                id = s.id,
                                name=s.name,
                                description=s.description,
                                price=s.price
                            })
                .ToList<ItemDTO>();
            }
            if (Item.Count == 0)
            {
                return NotFound();
            }

            return Ok(Item);
        }
        [HttpGet]
        //api to aceess records with id specified 
        public IHttpActionResult Index(int id)
        {
            IList<ItemDTO> Item = null;
            using (var ctx = new DatabEntities())
            {
                Item = ctx.items.Select(s => new ItemDTO()
                {
                    id = s.id,
                    name = s.name,
                    description = s.description,
                    price = s.price
                })
                .Where(s => s.id==id).ToList<ItemDTO>();
            }
            if (Item.Count == 0)
            {
                return NotFound();
            }

            return Ok(Item);
        }

        [HttpPost]
        //api to add new item
        public IHttpActionResult AddItem(item item)
        {

            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");
            using (var ctx = new DatabEntities())
            {
                int maxId;
                if (ctx.items.Count() <= 0)
                {
                    maxId = 0;
                }
                else
                {
                    maxId = ctx.items.Max(x => x.id);
                }
                 
               
                ctx.items.Add(new item()
                {   
                    id=maxId+1,
                    name=item.name,
                    description=item.description,
                    price=item.price
                });
              
                ctx.SaveChanges();
               
                
            }

            return Ok();
        }
        [HttpDelete]
        //api to delete an item
        public IHttpActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest("Not a valid item id");

            using (var ctx = new DatabEntities())
            {
                var item = ctx.items
                    .Where(s => s.id == id)
                    .FirstOrDefault();

                ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }

    }
}
