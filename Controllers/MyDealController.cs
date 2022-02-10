using MyWebAPIDeal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;

namespace MyWebAPIDeal.Controllers
{
    public class MyDealController : ApiController
    {
        public IHttpActionResult GetRecordedClientDeals()
        {
            string sPath = Path.Combine(@"C:\", "MyDeal.txt");
            bool fileExist = File.Exists(sPath);
            if (fileExist)
            {
                using (Stream stream = File.Open(sPath, FileMode.Open, FileAccess.Read))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    List<ClientDealsModel> clientDeal = (List<ClientDealsModel>)bformatter.Deserialize(stream);
                    return Ok(clientDeal);
                }
            }
            else
                return BadRequest();
        }
        
        public IHttpActionResult RecordClientDeals(ClientDealsModel clientDealsDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");
            string sPath = Path.Combine(@"C:\", "MyDeal.txt");
            bool fileExist = File.Exists(sPath);
            if (fileExist)
            {
                using (Stream stream = File.Open(sPath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    List<ClientDealsModel> clientDeal = (List<ClientDealsModel>)bformatter.Deserialize(stream);
                    clientDeal.Add(clientDealsDetails);
                    stream.Close();
                    using (Stream str = new FileStream(sPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        bformatter.Serialize(str, clientDeal);
                        str.Close();
                    }
                }
            }
            else
            {
                using (var fs = File.Create(sPath))
                {
                    fs.Close();
                    using (Stream stream = new FileStream(sPath,FileMode.Create, FileAccess.ReadWrite))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        List<ClientDealsModel> clientDeal = new List<ClientDealsModel>() { clientDealsDetails };
                        bformatter.Serialize(stream, clientDeal);
                        stream.Close();
                    }
                }
            }
            return Ok();
        }
    }
}
