using System.Collections.Generic; 
namespace ChickenTinder.Shared.Core{ 

    public class Location
    {
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string Address3 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public List<string> DisplayAddress { get; set; } = new();
    }

}