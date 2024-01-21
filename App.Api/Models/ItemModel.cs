namespace App.Api.Models
{
    public class ItemMaster
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class ItemDetails 
    { 
        public string By { get; set; }
        public int Descendants { get; set; }
        public int Id { get; set; }
        public int[] Kids { get; set; }
        public int Score { get; set; }
        public int Time { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    
    }




    //{
    //  "by" : "gourmetcode",
    //  "descendants" : 75,
    //  "id" : 38679453,
    //  "kids" : [ 38680231, 38680026, 38681578, 38680477, 38680513, 38687299, 38680636, 38684270, 38680160, 38680083, 38680335, 38680460, 38680392, 38679943, 38679919, 38679757, 38680021 ],
    //  "score" : 221,
    //  "time" : 1702876419,
    //  "title" : "Show HN: Microagents: Agents capable of self-editing their prompts / Python code",
    //  "type" : "story",
    //  "url" : "https://github.com/aymenfurter/microagents"
    //}
}
