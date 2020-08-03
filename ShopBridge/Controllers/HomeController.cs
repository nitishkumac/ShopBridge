using Newtonsoft.Json;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShopBridge.Controllers
{
    public class HomeController : Controller
    {
        string baseurl= "https://localhost:44377/";
        //Action to access all item
        public async Task<ActionResult> Index()
        {
            List<ItemDTO> itemDTOs = new List<ItemDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Access");
                if (Res.IsSuccessStatusCode)
                {
                    var ItemResponse = Res.Content.ReadAsStringAsync().Result;
                    itemDTOs = JsonConvert.DeserializeObject<List<ItemDTO>>(ItemResponse);
                }
                return View(itemDTOs);
            }


        }
        //Action to access  item with specified id
        public async Task<ActionResult> Details(int id)
        {
            List<ItemDTO> itemDTOs = new List<ItemDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Access/Index?id="+id);
                if (Res.IsSuccessStatusCode)
                {
                    var ItemResponse = Res.Content.ReadAsStringAsync().Result;
                    itemDTOs = JsonConvert.DeserializeObject<List<ItemDTO>>(ItemResponse);
                }
                return View(itemDTOs);
            }


        }
        [HttpGet]
        //Action to create new item view
        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        //Action to create new item 
        public ActionResult Create([Bind(Exclude ="id")]ItemDTO itemDTO)
        {
            //ItemDTO itemDTOs = new ItemDTO();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var postTask = client.PostAsJsonAsync<ItemDTO>("api/Access/AddItem", itemDTO);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View(itemDTO);
            }

        }
        [HttpGet]
        //Action to delete  item 
        public ActionResult Delete(int id)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                var deleteTask = client.DeleteAsync("api/Access/Delete?id=" + id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

    }
}