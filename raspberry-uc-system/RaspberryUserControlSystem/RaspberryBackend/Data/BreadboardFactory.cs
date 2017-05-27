﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;
using System.Text;

namespace RaspberryBackend
{
    class BreadboardFactory
    {
        private Dictionary<string, Dictionary<List<string>, Config>> hi_dictionary = new Dictionary<string, Dictionary<List<string>, Config>>();

        private const string _CONFIG_PATH = "Data/config/PinOutInfo.xml";

        private XDocument config;

        public BreadboardFactory()
        {
            config = XDocument.Load(_CONFIG_PATH);
            buildDictionary();
        }

        private void buildDictionary()
        {
            IEnumerable<XNode> familyNodes = config.Element("PinOutInfo").Nodes();

            foreach (XElement familyElement in familyNodes)
            {

                Dictionary<List<string>, Config> tmp = new Dictionary<List<string>, Config>();

                IEnumerable<XNode> modelNodes = familyElement.Nodes();
                foreach(XElement modelElement in modelNodes)
                {
                    string model_names_string = modelElement.Attribute("name").Value;

                    List<string> model_names_list = new List<string>();

                    Char delimiter = ',';
                    String[] model_names = model_names_string.Split(delimiter);

                    model_names_list.AddRange(model_names);

                    List<string> pin_value_list = new List<string>();

                    IEnumerable<XNode> pinNodes = modelElement.Nodes();
                    foreach (XElement pinElement in pinNodes)
                    {
                        string pin_value = pinElement.Attribute("value").Value;
                        pin_value_list.Add(pin_value);
                    }

                    Config pin_config = new Config(pin_value_list);

                    tmp.Add(model_names_list, pin_config);
                }
                string family_name = familyElement.Attribute("name").Value;

                hi_dictionary.Add(family_name, tmp);
            }
        }

        public Breadboard createBreadboard(string family, string model_name)
        {
            if (hi_dictionary.ContainsKey(family))
            {
                Dictionary<List<string>, Config> family_dic = hi_dictionary[family];

                Config bb_config = null;
                bool model_found = false;

                foreach(List<string> model_names in family_dic.Keys)
                {
                    if (model_names.Contains(model_name))
                    {
                        bb_config = family_dic[model_names];
                        model_found = true;
                    }
                }

                if (model_found)
                {
                    return new Breadboard(family, model_name, bb_config);
                }
            }

            return null;
        }

        public string toString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string familyName in hi_dictionary.Keys)
            {
                sb.Append("Family "+ familyName + ":");
                sb.Append("\n");

                Dictionary<List<string>, Config> model_dic = hi_dictionary[familyName];

                foreach (List<string> model_names in model_dic.Keys)
                {
                    sb.Append("\tModels: ");
                    foreach (string model_name in model_names)
                    {
                        sb.Append("[" + model_name + "]");
                    }
                   
                    sb.Append("\n");

                    Config config_obj = model_dic[model_names];
                    Dictionary<int, string> config_map = config_obj.Pin_value_map;

                    sb.Append("\tConfig: ");
                    sb.Append("\n");
                    foreach (int key in config_map.Keys)
                    {
                        string value = config_map[key];

                        sb.Append("\t\t PinID: " + key + ", Value: "+ value);
                        sb.Append("\n");
                    }
                }
                sb.Append("------------------------------------------------------------");
                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}
