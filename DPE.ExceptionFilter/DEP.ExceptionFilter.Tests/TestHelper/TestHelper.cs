using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DEP.ExceptionFilter.Tests.TestHelper
{
   public static class TestHelper
    {
        public static string ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var sb = new StringBuilder();
            using (var sr = new StreamReader(path))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }

        public static void RemoveFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

       public static string GenerateExpectedResult(string message, Dictionary<string, object> dict)
       {
           const string header = "----------------------------------";
           const string footer = "----------------------------------";

           var expectedResult = new StringBuilder();
           expectedResult.AppendLine(header);
           foreach (var property in dict)
           {
               expectedResult.AppendLine(string.Format("{0} : {1}", property.Key, property.Value));
           }
           expectedResult.AppendFormat("Message : {0}\r\n", message);
           expectedResult.AppendLine(footer);
           return expectedResult.ToString();
       }
    }
}
