namespace App.Api.Models
{
    public class ItemMaster
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class ItemDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }

    }

    public class ItemDetails : ItemDetailsDTO
    {
        public string By { get; set; }
        public int Descendants { get; set; }
        public int[] Kids { get; set; }
        public int Score { get; set; }
        public int Time { get; set; }
    }

}
