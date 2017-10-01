using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Commands
{
    class Storage
    {
        internal Command command;
        public Storage(Command c)
        {
            command = c;
        }
        public void SetI(string path)
        {
            command.IPath = path;
        }
        public void SetO(string path)
        {
            command.OPath = path;
        }
        public void Save(string name ="untitled")
        {
            try
            {
              
                command.Edit.Bitmap.Save(command.OPath+name);
            }
            catch 
            {
                Console.WriteLine("Wrong outputpath use SetO for outpath"+ command.OPath);
            }
        }
        public void Aload()
        {
            command.Edit.Init();
        }


    }

}
