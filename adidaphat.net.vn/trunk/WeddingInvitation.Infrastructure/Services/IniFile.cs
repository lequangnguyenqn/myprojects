using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace WeddingInvitation.Infrastructure.Services
{
    public class IniFile
    {
        private string iniFile;

        public IniFile(string iniFileName)
        {
            iniFile = iniFileName;
        }

        //
        public string GetStringValue(string keyName)
        {
            string valReturn = "";
            if (File.Exists(iniFile))
            {
                StreamReader sr = new StreamReader(iniFile);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string search = Regex.Match(line, keyName + ".*").Value;
                    if (!(search == ""))
                    {
                        valReturn = search.Replace(keyName + "=", "");
                        break;
                    }
                }
                sr.Close();
            }
            return valReturn;
        }
    }
}
