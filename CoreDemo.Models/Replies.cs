using System;

namespace CoreDemo.Models
{
    public class Replies
    {
        public int Id { get; set; }
        public string Remark { get; set; }
        public int TopicId { get; set; }
        public int ReplyId { get; set; }
        public DateTime ReplyDate { get; set; }

    }
}
