﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testes
{
    class Program
    {
        static void Main(string[] args)
        {
            var resu = DemoPluginNet.FormatarTabela.FormatarTextoTabela(Testes.Properties.Resources.res1);
            Console.WriteLine(resu);

             resu = DemoPluginNet.FormatarTabela.FormatarTextoTabela(Testes.Properties.Resources.res2);
            Console.WriteLine(resu);
        }
    }
}
