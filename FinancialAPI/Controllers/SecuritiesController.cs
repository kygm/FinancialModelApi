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
        public async Task<ActionResult<IEnumerable<Security>>> GetAllSecurities()
        {
            return await _context.Securities.ToListAsync();
        }

        // GET: api/Securities/5
        [HttpGet("{search}")]
        public async Task<ActionResult<string>> GetSecurityAsync(string search)
        {
            //Query based on passed in string
            string QUERY_URL = "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=" + search + "&interval=5min&apikey=3G6Z7P7YYUSBPI4N";
            Uri queryUri = new Uri(QUERY_URL);

            using (WebClient client = new WebClient())
            {
                dynamic json_data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(client.DownloadString(queryUri));
                string ret = "";
                foreach (var item in json_data)
                {
                    Console.WriteLine(item.Key);
                    Console.WriteLine(item.Value);
                    ret += item.Key;                            
                }
                return ret;
            }
            /*
             * To search DB based on ID, pass in INT to search on PK.
            var security = await _context.Securities.FindAsync(id);

            if (security == null)
            {
                return NotFound();
            }

            return security;
            */
        }


        // POST: api/Securities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Security>> AddSecurity(Security security)
        {

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
