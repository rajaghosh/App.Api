using App.Api.Models;
using App.Api.Service.Implementation;
using App.Api.Service.Interface;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace App.Api.Testing
{
    public class HackerTesting
    {
        [Fact]
        public async Task CheckIfHackerServiceHasMultipleItems()
        {
            IHackerService _hackerService = new HackerService();
            List<ItemMaster> data = await _hackerService.GetAllItems();
            Assert.True(data.Count > 0);
        }

        [Fact]
        public async Task CheckIfHackerServiceHasMultipleItems2()
        {
            Mock<IHackerService> hackerMockService = new Mock<IHackerService>();

            List<ItemMaster> itemMasterList = new List<ItemMaster>();
            hackerMockService.Setup(p => p.GetAllItems().Result).Returns(itemMasterList);
        }

        [Fact]
        public async Task CheckIfHackerServiceHasValue()
        {
            IHackerService _hackerService = new HackerService();
            ItemDetails data = await _hackerService.CallEventDetailsById((39081948).ToString());

            ItemDetailsDTO dataCheck = new ItemDetailsDTO()
            {
                Id = data.Id,
                Title = data.Title,
                Type = data.Type,
                Url = data.Url
            };


            ItemDetailsDTO itemDetails = new ItemDetailsDTO()
            {
                Id = 39081948,
                Title = "C23: A Slightly Better C",
                Type = "story",
                Url = "https://lemire.me/blog/2024/01/21/c23-a-slightly-better-c/"
            };

            Assert.Equal(JsonConvert.SerializeObject(dataCheck), JsonConvert.SerializeObject(itemDetails));
        }

        [Fact]
        public async Task CheckIfHackerServiceHasValueFromRange()
        {
            IHackerService _hackerService = new HackerService();

            List<string> itemIds = new List<string>() { new string((39081948).ToString()) };
            List<ItemMaster> itemList = await _hackerService.GetAllItems();

            List<ItemDetails> data = await _hackerService.GetEventDetailsList(itemIds, itemList);

            var dataTest = data.FirstOrDefault();
            ItemDetailsDTO dataCheck = new ItemDetailsDTO()
            {
                Id = dataTest.Id,
                Title = dataTest.Title,
                Type = dataTest.Type,
                Url = dataTest.Url
            };


            ItemDetailsDTO itemDetails = new ItemDetailsDTO()
            {
                Id = 39081948,
                Title = "C23: A Slightly Better C",
                Type = "story",
                Url = "https://lemire.me/blog/2024/01/21/c23-a-slightly-better-c/"
            };

            Assert.Equal(JsonConvert.SerializeObject(dataCheck), JsonConvert.SerializeObject(itemDetails));
        }

    }
}
