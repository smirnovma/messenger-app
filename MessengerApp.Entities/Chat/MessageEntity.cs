using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp.Entities.Chat
{
    public class MessageEntity : AbstractEntity
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string UserLoginFrom { get; set; }
        public string UserLoginTo { get; set; }
    }
}

