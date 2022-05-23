namespace PromFutureTestWork.Models
{
    public class ResponseModel
    {
        public Item item { get; set; }

        public class Item
        {
            public int terminal_id { get; set; }
            public DateTime time_created { get; set; }
            public int command_id { get; set; }
            public int parameter1 { get; set; }
            public int parameter2 { get; set; }
            public int parameter3 { get; set; }
            public string state_name { get; set; }
        }
    }
}
