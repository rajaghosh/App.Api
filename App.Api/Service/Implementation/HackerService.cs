using App.Api.Models;
using App.Api.Service.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace App.Api.Service.Implementation
{
    public class HackerService : IHackerService
    {
        public async Task<List<ItemMaster>> GetAllItems()
        {

            List<ItemMaster> idList = new List<ItemMaster>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //GET Method
                    HttpResponseMessage response = await client.GetAsync("v0/topstories.json?print=pretty");
                    if (response.IsSuccessStatusCode)
                    {
                        List<int> idItem = await response.Content.ReadAsAsync<List<int>>();
                        int cnt = 0;
                        if (idItem.Count > 0)
                        {
                            //idList = new List<ItemMaster>();
                            idItem.ForEach(p =>
                            {

                                idList.Add(new ItemMaster() { Id = ++cnt, Value = p.ToString() });
                            });
                        }
                    }
                    //else
                    //{
                    //    BadRequest("Internal server Error");
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return idList;
        }

        public async Task<ItemDetails> CallEventDetailsById(string id)
        {
            ItemDetails itemDetails = new ItemDetails();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //GET Method
                    HttpResponseMessage response = await client.GetAsync($"v0/item/{id}.json?print=pretty");
                    if (response.IsSuccessStatusCode)
                    {
                        itemDetails = await response.Content.ReadAsAsync<ItemDetails>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return itemDetails;
        }

        public async Task<List<ItemDetails>> GetEventDetailsList(List<string> itemIds, List<ItemMaster> itemList)
        {
            //Note - Offset should be starting from 0
            List<ItemDetails> itemDetailsList = new List<ItemDetails>();
            try
            {
                //List<int> itemRootIds = itemList.Select(p => p.Id).Skip(offset).Take(pageSize).ToList();
                //List<string> itemIds = itemList.Where(p => itemRootIds.Contains(p.Id)).Select(p => p.Value).ToList();

                List<Task<ItemDetails>> itemDetailsTask = new List<Task<ItemDetails>>();
                foreach (var item in itemIds)
                {
                    itemDetailsTask.Add(CallEventDetailsById(item));
                }

                await Task.WhenAll(itemDetailsTask);


                foreach (var item in itemDetailsTask)
                {
                    itemDetailsList.Add(item.Result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return itemDetailsList; 
        }
    }
}
