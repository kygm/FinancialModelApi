using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinancialAPI.Data;
using FinancialAPI.Models;
using Newtonsoft.Json;
using System.Dynamic;

namespace FinancialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricalSecuritiesController : ControllerBase
    {
        private readonly FinancialModelContext _context;

        public HistoricalSecuritiesController(FinancialModelContext context)
        {
            _context = context;
        }

        // GET: api/HistoricalSecurities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoricalSecurity>>> GetHistoricalSecurities()
        {
            return await _context.HistoricalSecurities.ToListAsync();
        }
        
        // GET: api/HistoricalSecurities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HistoricalSecurity>> GetHistoricalSecurity(int id)
        {

            HttpClient client = new HttpClient();
            dynamic obj = new ExpandoObject();
            //top cryptos api endpoint  
            HttpResponseMessage response = client.GetAsync("https://api.coincap.io/v2/assets").Result;
            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;


            var expConverter = new Newtonsoft.Json.Converters.ExpandoObjectConverter();
            obj = JsonConvert.DeserializeObject<ExpandoObject>(result, expConverter);

            var json = JsonConvert.SerializeObject(obj.data);


            List<Coin> lCoins = JsonConvert.DeserializeObject<List<Coin>>(json);
            foreach(var l in lCoins)
            {
                System.Diagnostics.Debug.WriteLine(l.symbol);
            }
            var historicalSecurity = await _context.HistoricalSecurities.FindAsync(id);

            if (historicalSecurity == null)
            {
                return NotFound();
            }

            return historicalSecurity;
        }

        // PUT: api/HistoricalSecurities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistoricalSecurity(int id, HistoricalSecurity historicalSecurity)
        {
            if (id != historicalSecurity.ID)
            {
                return BadRequest();
            }

            _context.Entry(historicalSecurity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoricalSecurityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/HistoricalSecurities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HistoricalSecurity>> PostHistoricalSecurity(HistoricalSecurity historicalSecurity)
        {
            _context.HistoricalSecurities.Add(historicalSecurity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistoricalSecurity", new { id = historicalSecurity.ID }, historicalSecurity);
        }

        // DELETE: api/HistoricalSecurities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistoricalSecurity(int id)
        {
            var historicalSecurity = await _context.HistoricalSecurities.FindAsync(id);
            if (historicalSecurity == null)
            {
                return NotFound();
            }

            _context.HistoricalSecurities.Remove(historicalSecurity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistoricalSecurityExists(int id)
        {
            return _context.HistoricalSecurities.Any(e => e.ID == id);
        }
    }
}
