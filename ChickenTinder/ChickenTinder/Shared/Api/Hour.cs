using System.Collections.Generic; 
namespace ChickenTinder.Shared.Api{ 

    public class Hour
    {
        public List<Open> Open { get; set; }
        public string HoursType { get; set; }
        public bool IsOpenNow { get; set; }
    }

}