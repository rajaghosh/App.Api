using App.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Api.Service.Interface
{
    public interface IHackerService
    {
        Task<List<ItemMaster>> GetAllItems();
        Task<ItemDetails> CallEventDetailsById(string id);
        Task<List<ItemDetails>> GetEventDetailsList(List<string> itemIds, List<ItemMaster> itemList);
    }
}
