using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Completist.Model
{
    //Note: the spelling variation of the term 'colour' is consistenly spelt as 'color' as to avoid confusion when writing XAML code and spelling sensitive situations.
    //class for available colors to be used with both tags and priorities. to add custom colours, hex codes can be added into the SQLite DB
    public class AllColors
    {
        public override string ToString()
        {
            return Name;
        }

        string name;
        string color;

        public string Name { get => name; set => name = value; }
        public string Color { get => color; set => color = value; }


    }
}
