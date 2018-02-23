using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

public class importColumns
{
    [JsonProperty("column")]
    public int column { get; set; }

    [JsonProperty("attrid")]
    public int attrid { get; set; }

    [JsonProperty("name")]
    public string name { get; set; }
}
