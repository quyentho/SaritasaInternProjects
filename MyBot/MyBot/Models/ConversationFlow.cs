using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot.Models
{
    public class ConversationFlow
    {
        public Question LastQuestionAsked { get; set; }

        public enum Question
        {
            None,
            Username,
            Password
        }
    }
}
