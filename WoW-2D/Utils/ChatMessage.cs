using Framework.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.Utils
{
    /// <summary>
    /// Stores chat-message information.
    /// </summary>
    public class ChatMessage
    {
        public ChatFlag Flag { get; set; }
        public string Sender { get; set; } = string.Empty;
        public string Message { get; set; }
    }
}
