using System.Collections.Generic;

namespace FinnishApp.Models
{
    public class WordPair
    {
        public string Polish  { get; set; } = "";
        public string Finnish { get; set; } = "";
        public Dictionary<string,string> InflectionsSingular { get; set; } = new();
        public Dictionary<string,string> InflectionsPlural   { get; set; } = new();
    }
}