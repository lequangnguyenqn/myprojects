using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeddingInvitation.Infrastructure.Constant
{
    public class Common
    {
        public const string XMLFormat = "<![CDATA[{0}]]>";

        public static List<Topic> GetTopics()
        {
            var list = new List<Topic>();
            list.Add(new Topic { TopicId = 1, TopicName = "I am unable to sign in" });
            list.Add(new Topic { TopicId = 2, TopicName = "I have a question before I sign up" });
            list.Add(new Topic { TopicId = 3, TopicName = "I want to request a feature" });
            list.Add(new Topic { TopicId = 4, TopicName = "I have a billing question" });
            list.Add(new Topic { TopicId = 5, TopicName = "I am confused about how something works" });
            list.Add(new Topic { TopicId = 5, TopicName = "I am experiencing technical difficulties" });
            list.Add(new Topic { TopicId = 5, TopicName = "Other" });
            return list;
        }

        public static string GetInvoiceCode(int invoiceId)
        {
            return (500000 + invoiceId).ToString();
        }
    }

    public class Topic
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }
    }
}
