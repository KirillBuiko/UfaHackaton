using System;
using System.Collections.Generic;
using System.Text;

namespace SpecialistGUI
{
    class Comand
    {
        private String id;
        private String text;
        private String parentID;
        private String childID;

        public Comand(String id, string text,  string childID, string parentID)
        {
            this.id = id;
            this.text = text;
            this.parentID = parentID;
            this.childID = childID;
        }

        public string Text { get => text; set => text = value; }
        public string ParentID { get => parentID; set => parentID = value; }
        public string ChildrID { get => childID; set => childID = value; }
        public string Id { get => id; set => id = value; }
    }
}
