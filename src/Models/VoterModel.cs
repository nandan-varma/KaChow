using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    public class VoterModel
    {

        public string Id { get; set; }
        public HashSet<string> products_voted_for { get; } = new HashSet<string>();

        public override string toString() => JsonSerializer.Serialize(this);
    }
}
