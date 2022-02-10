using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebAPIDeal.Models
{
    [Serializable]
    public class ClientDealsModel
    {
        public long ClientId { get; set; }
        public string ClientName { get; set; }
        public long DealValueInDollars { get; set; }
    }
}