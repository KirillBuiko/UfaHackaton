using System;
using System.Collections.Generic;
using System.Text;

namespace MedBot_Test
{
    class Comand
    {
        private String id;
        private String text;
        private String childID;
        private String parentID;
        private String type;

        public Comand(string id, string text, string childID, string parentID, string type)
        {
            this.id = id;
            this.text = text;
            this.ChildID = childID;
            this.parentID = parentID;
            this.type = type;
        }

        public string Text { get => text; set => text = value; }
        public string ParentID { get => parentID; set => parentID = value; }
        public string Id { get => id; set => id = value; }
        public string Type { get => type; set => type = value; }
        public string ChildID { get => childID; set => childID = value; }
    }
}
