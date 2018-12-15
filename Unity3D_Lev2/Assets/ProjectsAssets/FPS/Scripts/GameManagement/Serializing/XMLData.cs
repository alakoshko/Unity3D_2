using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace FPS
{
    public class XMLData : IDataProvider
    {
        string _path;

        public FPSData Load()
        {
            if (!File.Exists(_path)) return default(FPSData);

            var fpsData = new FPSData
            {

            };
            string key;
            using(var reader = new XmlTextReader(_path))
            {
                while (reader.Read())
                {
                    key = "PlayerName";
                    if (reader.IsStartElement(key))
                        fpsData.PlayerName = reader.GetAttribute("value");

                    key = "Health";
                    if (reader.IsStartElement(key))
                        float.TryParse(reader.GetAttribute("value"), out fpsData.Health);

                    key = "PlayerPosition";
                    if (reader.IsStartElement(key))
                    {
                        var s = reader.GetAttribute("value");
                        fpsData.PlayerPosition = ConvertFunctions.GetVector3(s);
                    }
                }
            }
            Debug.Log("XML Data loaded");

            return fpsData;
        }

        
        public void Save(FPSData fPSData)
        {
            var xmlDoc = new XmlDocument();
            var rootNode = xmlDoc.CreateElement("FPSData");
            xmlDoc.AppendChild(rootNode);

            var element = xmlDoc.CreateElement("PlayerName");
            element.SetAttribute("value", fPSData.PlayerName);
            rootNode.AppendChild(element);

            element = xmlDoc.CreateElement("Health");
            element.SetAttribute("value", fPSData.Health.ToString());
            rootNode.AppendChild(element);

            element = xmlDoc.CreateElement("PlayerPosition");
            element.SetAttribute("value", fPSData.PlayerPosition.ToString());
            rootNode.AppendChild(element);

            xmlDoc.Save(_path);
            Debug.Log("XML Data saved");
        }

        public void SetOption(string path)
        {
            _path = Path.Combine(path, "XMLData.xml");
        }
    }
}