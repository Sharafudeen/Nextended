﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;

namespace Nextended.Core.Helper;

public interface IJObjectParser
{
    JObject Parse(string content);
}


public class JsonJObjectParser : IJObjectParser
{
    public JObject Parse(string content)
    {
        return JObject.Parse(content);
    }
}

public class XmlJObjectParser : IJObjectParser
{
    public JObject Parse(string content)
    {
        // Use Newtonsoft.Json's capability to convert XML to JSON
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(content);
        string jsonString = JsonConvert.SerializeXmlNode(xmlDoc);
        return JObject.Parse(jsonString);
    }
}

public class YamlJObjectParser : IJObjectParser
{
    public JObject Parse(string content)
    {        
        var deserializer = new YamlDotNet.Serialization.DeserializerBuilder().Build();
        var yamlObject = deserializer.Deserialize<object>(content);
        string jsonString = JsonConvert.SerializeObject(yamlObject);
        return JObject.Parse(jsonString);
    }
}

public enum ModelInputType
{
    Json,
    Xml,
    Yaml
}