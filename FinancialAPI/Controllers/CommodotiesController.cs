using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Dynamic;

namespace FinancialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodotiesController : ControllerBase
    {
        
        [HttpGet]
        public ActionResult<Commodity> Get()
        {
            //https://data.nasdaq.com/api/v3/datatables/WASDE/DATA?code=MILK_US_33&report_month=2022-01&api_key=ayrizEM55fccZxKeAWae
            //api addr above

            try
            {
                HttpClient client = new HttpClient();
                dynamic? obj = new ExpandoObject();
                string? result;
                //top cryptos api endpoint  
                try
                {
                                                                       //USE API KEY HERE and remove guest login creds after debug/dev
                    HttpResponseMessage response = client.GetAsync("https://api.tradingeconomics.com/markets/commodities?c=guest:guest&f=json").Result;
                    response.EnsureSuccessStatusCode();
                    result = response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }


                List<Commodity> lCom = JsonConvert.DeserializeObject<List<Commodity>>(result);

                foreach (var l in lCom)
                {
                    System.Diagnostics.Debug.WriteLine(l.Name);
                }
                return Ok(lCom);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
