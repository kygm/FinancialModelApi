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
            //interval enums set in ApiStore class
            var stocks = stockOperations.GetCurrentStocks(query, interval);
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
            JsonObject json = new JsonObject();
            json.Add("Total", stocks.Count.ToString());
            json.Add("Ticker", query);
            double stdDev = stat.CalcuateStandardDeviation();
            json.Add("Standard deviation over the past " + days + " days: ", stdDev.ToString());
            string direction = (stdDev > 0) ? "Positive" : "Negative";
            json.Add("Movement", direction);
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
