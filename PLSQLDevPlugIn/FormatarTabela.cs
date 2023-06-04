using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DemoPluginNet
{
    public class FormatarTabela
    {
        public static string FormatarTextoTabela(string textToFormat)
        {
            if (string.IsNullOrWhiteSpace(textToFormat))
            {
                return "--ERRO !!";
            }

            var lines = textToFormat.Split('\n', '\r');

            if (!lines.Any())
            {
                return "-- ??? : " + textToFormat;
            }

            var cols = lines[0].Split('\t');

            if (cols.Count() <= 0)
            {
                return "-- ??? : " + textToFormat;
            }

            var maxLen = new int[cols.Count()];

            for (int i = 0; i < lines.Count(); i++)
            {
                var lineCols = lines[i].Split('\t');

                if (lineCols.Count() < cols.Count())
                {
                    continue;
                }
                for (int j = 0; j < cols.Count(); j++)
                {
                    var len = lineCols[j].Length;
                    if (maxLen[j] < len)
                    {
                        maxLen[j] = len;
                    }
                }
            }

            var cc = new StringBuilder();
            //cc.AppendLine("--resultado:");

            for (int i2 = 0; i2 < lines.Count(); i2++)
            {
                if (string.IsNullOrEmpty(lines[i2]))
                {
                    continue;
                }
                var lineCols = lines[i2].Split('\t');

                if (lineCols.Count() < cols.Count())
                {
                    continue;
                }

                StringBuilder cc2 = new StringBuilder();
                cc2.Append("-- ");

                for (int k = 0; k < cols.Count(); k++)
                {
                    //Debug.WriteLine(lineCols[k]);
                    cc2.Append(lineCols[k].PadRight(maxLen[k]));
                    cc2.Append(" ");
                }

                cc.AppendLine(cc2.ToString());
            }

            return cc.ToString();
        }

    }
}
