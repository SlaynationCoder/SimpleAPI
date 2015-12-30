using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAPI
{
    /*
        SimpleINI 0.0.1 by Slaynation_Coder
    */
    public class SimpleINI
    {
        private string path;
        private Dictionary<string, string> config = new Dictionary<string, string>();

        private static string InitText = "; SimpleINI ini File by Slaynation_Coder";

        /// <summary>
        /// クラスを初期化し、INIファイルをロードします。
        /// ファイルがない場合は新規作成します。
        /// </summary>
        /// <param name="path">.iniのファイルパス</param>
        public SimpleINI(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            this.path = path;
            if (!loadINI())
                throw new Exception("INI load failed!");


        }

        private bool loadINI()
        {
            try
            {
                string line;
                using (StreamReader file = new StreamReader(path))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (!line.Contains("=") || !line.Contains("\""))
                            continue;

                        //escape-string format
                        //\" and \=
                        line = line.Replace("\\\"", "%DOUBLEQUOTE%").Replace("\\=", "%EQUAL%");

                        string sKey = line.Split('=')[0];
                        string sValue = line.Split('=')[1];

                        string Key = sKey.Substring(1, sKey.Length - 2);
                        string Value = sValue.Substring(1, sValue.Length - 2);


                        //un-escape string
                        Key = Key.Replace("%DOUBLEQUOTE%", "\"").Replace("%EQUAL%", "=");
                        Value = Value.Replace("%DOUBLEQUOTE%", "\"").Replace("%EQUAL%", "=");

                        config.Add(Key, Value);
                    }
                }

                return true;
            }
            catch (Exception)
            { return false; }
        }

        /// <summary>
        /// INIファイルをセーブします。
        /// </summary>
        public bool saveINI()
        {
            try
            {
                using (StreamWriter file = new StreamWriter(path))
                {
                    file.WriteLine(InitText);

                    foreach (string item in config.Keys)
                    {
                        string key = item;
                        string value = config[item];

                        //escape
                        key = key.Replace("\"", "\\\"").Replace("=", "\\=");
                        value = value.Replace("\"", "\\\"").Replace("=", "\\=");

                        file.WriteLine("\"" + key + "\"=\"" + value + "\"");
                    }
                }

                return true;
            }
            catch (Exception)
            { return false; }
        }

        public string get(string key)
        {
            if (!config.ContainsKey(key))
                return string.Empty;

            return config[key];
        }

        public void set(string key, string value)
        {
            if (config.ContainsKey(key))
                config.Remove(key);

            config.Add(key, value);

        }
    }
}
