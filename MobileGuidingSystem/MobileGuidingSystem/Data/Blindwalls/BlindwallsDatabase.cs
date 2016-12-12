using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace MobileGuidingSystem.Data.Blindwalls
{
    class BlindwallsDatabase
    {
        public static BlindwallsDatabase Instance => _instance ?? (_instance = new BlindwallsDatabase());

        private static BlindwallsDatabase _instance;

        public string status { get; set; }
        public int count { get; set; }
        public int count_total { get; set; }
        public int pages { get; set; }
        public List<Post> posts { get; set; }
        public Query query { get; set; }

        
    }

    
}
