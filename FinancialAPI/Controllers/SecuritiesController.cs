using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinancialAPI.Data;
using FinancialAPI.Models;
using System.Net;
using System.Text.Json;
using ServiceStack;
using ServiceStack.Text;
using System.Dynamic;

namespace FinancialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecuritiesController : ControllerBase
    {
        private readonly FinancialModelContext _context;

        public SecuritiesController(FinancialModelContext context)
        {
            _context = context;
        }

        // GET: api/Securities
        [HttpGet]
        public ActionResult<JsonObject> GetAllSecurities(string query, int interval)
        {
            ApiStore stockOperations = new ApiStore();
            List<Stock> stocks = new List<Stock>();
            //interval enums set in ApiStore class
            stocks = stockOperations.GetCurrentStocks(query, interval);
            if(stocks.IsNullOrEmpty())
            {
                JsonObject ret = new JsonObject();
                ret.Add("Message", "No stock found");
                return BadRequest(ret);
            }
            //calculate days of movement:
            int days;
            if(interval == 5)
            {
                days = 2;
            }
            else if(interval == 15)
            {
                days = 4;
            }
            else
            {
                days = 6;
            }

            //gen list of doubles:
            var ldouble = new List<double>();
            foreach(var s in stocks)
            {
                double avg = ((s.High + s.Low) / 2);
                ldouble.Add(avg);
            }
            StatOperations stat = new StatOperations(ldouble, query);
            
            foreach (var i in stocks)
            {
                DateTime dt = new DateTime();
                dt = i.Timestamp.ToLocalTime();
                System.Diagnostics.Debug.WriteLine("Date: " + dt.ToString());
                System.Diagnostics.Debug.WriteLine("High: " + i.High + " Low: " + i.Low);
            }
            var arrDouble = ldouble.ToArray();
            JsonObject json = new JsonObject();
            json.Add("Total", stocks.Count.ToString());
            json.Add("Ticker", query);      
            StatOperations sop = new StatOperations(ldouble, query);
            Stat st = sop.CalcuateStandardDeviation(); 
            json.Add("Mean price over the past "   + days + " days: ", st.Mean.ToString());
            json.Add("High", ldouble.Max().ToString());
            json.Add("Low", ldouble.Min().ToString());
            json.Add("Standard Deviation", st.StandardDeviation.ToString());
            string direction = ((arrDouble[0] - arrDouble[arrDouble.Length -1]) < 0) ? "Positive" : "Negative";
            json.Add("Movement", direction);

            //Probabilities section
            double probability = (((st.StandardDeviation / st.Mean) * 100) * 2);
            json.Add("Probability of price moving +- 10%", String.Format("{0:0.##}", probability)+"%");
            return Ok(json);
        }



        [HttpPost]
        public async Task<ActionResult<Security>> AddSecurity(Security security)
        {
            /*
             For list of type obj
            List<Security> list = new List<Security>();

            foreach(var item in list)
            {
                _context.Add(item);
            }
            */
            _context.Securities.Add(security);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSecurity", new { id = security.ID }, security);
        }

        // DELETE: api/Securities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSecurity(int id)
        {
            var security = await _context.Securities.FindAsync(id);
            if (security == null)
            {
                return NotFound();
            }

            _context.Securities.Remove(security);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SecurityExists(int id)
        {
            return _context.Securities.Any(e => e.ID == id);
        }
    }
}
