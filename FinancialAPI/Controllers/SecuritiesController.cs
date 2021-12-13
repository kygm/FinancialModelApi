using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinancialAPI.Data;
using FinancialAPI.Models;

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
        public async Task<ActionResult<IEnumerable<Security>>> GetSecurities()
        {
            return await _context.Securities.ToListAsync();
        }

        // GET: api/Securities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Security>> GetSecurity(int id)
        {
            var security = await _context.Securities.FindAsync(id);

            if (security == null)
            {
                return NotFound();
            }

            return security;
        }

        // PUT: api/Securities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSecurity(int id, Security security)
        {
            if (id != security.ID)
            {
                return BadRequest();
            }

            _context.Entry(security).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecurityExists(id))
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

        // POST: api/Securities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Security>> PostSecurity(Security security)
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
