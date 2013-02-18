using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "-DoUpdate")
            {
                AutoUpdateStarter st = new AutoUpdateStarter();
                st.StartProcessAndWait();
            }
        }
    }
}
