using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class CommadnTreeComponent
    {
        private Comand comand;
        private int level;
        private int position;

        public CommadnTreeComponent(Comand comand, int level, int position)
        {
            this.Comand = comand;
            this.Level = level;
            this.Position = position;
        }

        public int Level { get => level; set => level = value; }
        public int Position { get => position; set => position = value; }
        internal Comand Comand { get => comand; set => comand = value; }
    }
}
