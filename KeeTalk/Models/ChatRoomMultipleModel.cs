using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeeTalk.Models
{
    public class ChatRoomMultipleModel
    {
        public IEnumerable<Message> Messages { get; set; }
        public Message Message { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}
