using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

public class importAttributes
{
    [JsonProperty("id")]
    public int id { get; set; }

    [JsonProperty("name")]
    public string name { get; set; }
}
