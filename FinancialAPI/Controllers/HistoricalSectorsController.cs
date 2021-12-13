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
    public class HistoricalSectorsController : ControllerBase
    {
        private readonly FinancialModelContext _context;

        public HistoricalSectorsController(FinancialModelContext context)
        {
            _context = context;
        }

        // GET: api/HistoricalSectors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoricalSector>>> GetHistoricalSectors()
        {
            return await _context.HistoricalSectors.ToListAsync();
        }

        // GET: api/HistoricalSectors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HistoricalSector>> GetHistoricalSector(int id)
        {
            var historicalSector = await _context.HistoricalSectors.FindAsync(id);

            if (historicalSector == null)
            {
                return NotFound();
            }

            return historicalSector;
        }

        // PUT: api/HistoricalSectors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistoricalSector(int id, HistoricalSector historicalSector)
        {
            if (id != historicalSector.ID)
            {
                return BadRequest();
            }

            _context.Entry(historicalSector).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoricalSectorExists(id))
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

        // POST: api/HistoricalSectors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HistoricalSector>> PostHistoricalSector(HistoricalSector historicalSector)
        {
            _context.HistoricalSectors.Add(historicalSector);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistoricalSector", new { id = historicalSector.ID }, historicalSector);
        }

        // DELETE: api/HistoricalSectors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistoricalSector(int id)
        {
            var historicalSector = await _context.HistoricalSectors.FindAsync(id);
            if (historicalSector == null)
            {
                return NotFound();
            }

            _context.HistoricalSectors.Remove(historicalSector);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistoricalSectorExists(int id)
        {
            return _context.HistoricalSectors.Any(e => e.ID == id);
        }
    }
}
