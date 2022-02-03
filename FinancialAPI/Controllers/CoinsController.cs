using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Dynamic;

namespace FinancialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        //this route hande getting the top 100 coins. No analysis yet.
        [HttpGet]
        public ActionResult<List<Coin>> Get()
        {
            try
            {
                HttpClient client = new HttpClient();
                dynamic? obj = new ExpandoObject();
                string? result;
                //top cryptos api endpoint  
                try
                {
                    HttpResponseMessage response = client.GetAsync("https://api.coincap.io/v2/assets").Result;
                    response.EnsureSuccessStatusCode();
                    result = response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }


                var expConverter = new Newtonsoft.Json.Converters.ExpandoObjectConverter();
                obj = JsonConvert.DeserializeObject<ExpandoObject>(result, expConverter);

                var json = JsonConvert.SerializeObject(obj.data);


                List<Coin> lCoins = JsonConvert.DeserializeObject<List<Coin>>(json);
                foreach (var l in lCoins)
                {
                    System.Diagnostics.Debug.WriteLine(l.symbol);
                }
                return Ok(lCoins);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
