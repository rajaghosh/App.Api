using App.Api.Models;
using App.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Api.Controllers
{
    public class HackerNewsController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHackerService _hackerService;
        
        public HackerNewsController(IMemoryCache memoryCache, IHackerService hackerService)
        {
            _memoryCache = memoryCache;
            _hackerService = hackerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetHacketItemList")]
        public async Task<ActionResult<List<ItemMaster>>> GetItems()
        {
            var cacheKey = "idList";

            try
            {
                if (!_memoryCache.TryGetValue(cacheKey, out List<ItemMaster> idList))
                {
                    idList = await _hackerService.GetAllItems();

                    //setting up cache options
                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(3600), // Cache for an hour
                        Priority = CacheItemPriority.High
                        //SlidingExpiration = Cache.No
                        //SlidingExpiration = TimeSpan.FromSeconds(20)
                    };

                    //setting cache entries
                    _memoryCache.Set(cacheKey, idList, cacheExpiryOptions);
                }

                //Add logic if the particular value is not present in the list 

                return Ok(idList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("GetHackerItemDetails/{id}")]
        public async Task<ActionResult<ItemDetails>> CallEvents(string id)
        {
            try
            {
                ItemDetails itemDetails = await _hackerService.CallEventDetailsById(id);
                return itemDetails;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpGet("GetHackerItemList/{offset}")]
        public async Task<ActionResult<List<ItemDetailsDTO>>> CallEvents(int offset)
        {
            var cacheKey = "itemIdDetails";
            int pageSize = 200;
            try
            {
                var eventListResult = await GetItems();
                var eventList = ((OkObjectResult)eventListResult.Result).Value as List<ItemMaster>;

                //Recalculate off set
                //Here page size is 200
                offset = offset * pageSize; 

                List<int> itemRootIds = eventList.Select(p => p.Id).Skip(offset).Take(pageSize).ToList();
                List<string> itemIds = eventList.Where(p => itemRootIds.Contains(p.Id)).Select(p => p.Value).ToList();

                //We shall retrieve the value to list using cache, even if we dont have anything in the cache to list operation we will get in
                if (_memoryCache.TryGetValue(cacheKey, out List<ItemDetails> itemIdDetails)? true:true)
                {
                    //Now check if the demand of itemIds if not present in the list
                    var check = itemIdDetails?.Where(p => itemIds.Contains(p.Id.ToString())).FirstOrDefault();
                    
                    //if new in demand items are not present in the cache list
                    if (check == null)
                    {
                        var newData = await _hackerService.GetEventDetailsList(itemIds, eventList);

                        //Invoke it on 1st occurance
                        if (itemIdDetails==null)
                        {
                            itemIdDetails = new List<ItemDetails>();
                        }

                        itemIdDetails.AddRange(newData);

                        //setting up cache options
                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddSeconds(3600), // Cache for an hour
                            Priority = CacheItemPriority.High
                        };

                        //setting cache entries
                        _memoryCache.Set(cacheKey, itemIdDetails, cacheExpiryOptions);
                    }
                }

                //Our main list will have most of the data, but we shall send only the portion which is needed
                var ret = itemIdDetails.Where(p => itemIds.Contains(p.Id.ToString()))
                                .Select(p=> new ItemDetailsDTO() { 
                                    Id= p.Id,
                                    Title = p.Title,
                                    Type = p.Type,
                                    Url = p.Url
                                }).ToList();

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

    }
}
