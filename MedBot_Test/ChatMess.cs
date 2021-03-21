using System;
using System.Collections.Generic;
using System.Text;

namespace MedBot_Test
{
    class ChatMess
    {
        private String id;
        private String message;
        private String chatID;
        private String isUsers;

        public ChatMess(string id, string text, string parentID, string type)
        {
            this.id = id;
            this.message = text;
            this.chatID = parentID;
            this.isUsers = type;
        }

        public string Mesage { get => message; set => message = value; }
        public string ChatID { get => chatID; set => chatID = value; }
        public string Id { get => id; set => id = value; }
        public string IsUsers { get => isUsers; set => isUsers = value; }
    }
}
