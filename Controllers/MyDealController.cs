using MyWebAPIDeal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace MyWebAPIDeal.Controllers
{
    [RoutePrefix("api/MyDeal")]
    public class MyDealController : ApiController
    {
        [Route("GetDeal")]
        public IHttpActionResult GetRecordedClientDeals(int id = 0)
        {
            IHttpActionResult result = null;
            string sPath = Path.Combine(@"C:\", "MyDeal.txt");
            bool fileExist = File.Exists(sPath);
            if (fileExist)
            {
                    string jsonString = File.ReadAllText(sPath);
                    List<ClientDealsModel> clientDeal = JsonConvert.DeserializeObject<List<ClientDealsModel>>(jsonString);
                    if (id > 0)
                    {
                        var deal = clientDeal.Where(e => e.ClientId == id).ToList();
                        return Ok(deal);
                    }
                    if(clientDeal != null)
                        result = Ok(clientDeal);
                    else
                        result = BadRequest();
            }
            else
                result=  BadRequest();

            return result;
        }
        [Route("SaveDeal")]
        public IHttpActionResult RecordClientDeals(ClientDealsModel clientDealsDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");
            string sPath = Path.Combine(@"C:\", "MyDeal.txt");
            bool fileExist = File.Exists(sPath);
            if (fileExist)
            {
                string jsonString = File.ReadAllText(sPath);
                List<ClientDealsModel> clientDeal = JsonConvert.DeserializeObject<List<ClientDealsModel>>(jsonString);
                if (clientDeal.Any(deal => deal.ClientId == clientDealsDetails.ClientId))
                {
                    return BadRequest();
                }
                clientDeal.Add(clientDealsDetails);
                using (StreamWriter myFile = new StreamWriter(sPath))
                {
                    myFile.WriteLine(JsonConvert.SerializeObject(clientDeal));
                    DisposeFile(myFile);
                }
            }
            else
            {
                using (StreamWriter myFile = new StreamWriter(sPath))
                {
                    List<ClientDealsModel> clientDeal = new List<ClientDealsModel>() { clientDealsDetails };
                    myFile.WriteLine(JsonConvert.SerializeObject(clientDeal));
                    DisposeFile(myFile);
                    
                }
            }
            return Ok();
        }

       

        [Route("UpdateDeal")]
        public IHttpActionResult EditClientDeal(ClientDealsModel clientDealsDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");
            if (clientDealsDetails != null && clientDealsDetails.ClientId > 0)
            {
                string sPath = Path.Combine(@"C:\", "MyDeal.txt");
                bool fileExist = File.Exists(sPath);
                if (fileExist)
                {
                    string jsonString = File.ReadAllText(sPath);
                    List<ClientDealsModel> clientDeal = JsonConvert.DeserializeObject<List<ClientDealsModel>>(jsonString);
                    var deal = clientDeal.Where(uniqueId => uniqueId.ClientId == clientDealsDetails.ClientId).FirstOrDefault();
                    if (deal != null)
                    {
                        clientDeal.Remove(deal);
                        clientDeal.Add(clientDealsDetails);
                        using (StreamWriter myFile = new StreamWriter(sPath))
                        {
                            myFile.WriteLine(JsonConvert.SerializeObject(clientDeal));
                            DisposeFile(myFile);
                        }
                    }
                    else
                        return BadRequest();
                   
                }
            }
            return Ok();
        }
        [Route("DeleteDeal")]
     
        public IHttpActionResult DeleteClientDeals(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");
            string sPath = Path.Combine(@"C:\", "MyDeal.txt");
            bool fileExist = File.Exists(sPath);
            if (fileExist)
            {
                string jsonString = File.ReadAllText(sPath);
                List<ClientDealsModel> clientDeal = JsonConvert.DeserializeObject<List<ClientDealsModel>>(jsonString);
                if (clientDeal.Count > 0 && clientDeal.Any(deal => deal.ClientId == id))
                {
                    clientDeal.Remove(clientDeal.Where(deal => deal.ClientId == id).First());
                    using (StreamWriter myFile = new StreamWriter(sPath))
                    {
                        myFile.WriteLine(JsonConvert.SerializeObject(clientDeal));
                        DisposeFile(myFile);
                    }
                }
                return Ok(clientDeal);
            }
            return BadRequest("Check with Administrator");
        }

        private void DisposeFile(StreamWriter myFile)
        {
            myFile.Flush();
            myFile.Close();
            myFile.Dispose();
        }
    }
}
