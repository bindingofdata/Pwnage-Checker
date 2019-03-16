using System;
using System.Collections.Generic;

namespace PwndAPILib.Models
{
    public class Breach
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Domain { get; set; }
        public DateTime BreachDate { get; set; }
        public DateTime AddedDate { get; set; }
        public string Description { get; set; }
        public List<string> DataClasses { get; set; }
        public bool IsVerified { get; set; }
    }
}
