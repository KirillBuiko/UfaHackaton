using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApp1
{
    class Comand
    {
        private String id;
        private String text;
        private String parentID;
        private String childID;
        private String type;

        public Comand(String id, string text,  string childID, string parentID, string type)
        {
            this.id = id;
            this.text = text;
            this.parentID = parentID;
            this.Type = type;
            if (childID != "")
            {
                this.childID = childID;
            }
            else this.childID = null;
        }

        public string Text { get => text; set => text = value; }
        public string ParentID { get => parentID; set => parentID = value; }
        public string ChildrID { get => childID; set => childID = value; }
        public string Id { get => id; set => id = value; }
        public string Type { get => type; set => type = value; }
    }
}
