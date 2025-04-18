using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizRuLet.DataAccess;
using QuizRuLet.DataAccess.Entities;

namespace QuizRuLet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly QuizRuLetDbContext _context;

        public ModulesController(QuizRuLetDbContext context)
        {
            _context = context;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleEntity>>> GetModules()
        {
            return await _context.Modules.ToListAsync();
        }

        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleEntity>> GetModuleEntity(Guid id)
        {
            var moduleEntity = await _context.Modules.FindAsync(id);

            if (moduleEntity == null)
            {
                return NotFound();
            }

            return moduleEntity;
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModuleEntity(Guid id, ModuleEntity moduleEntity)
        {
            if (id != moduleEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(moduleEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleEntityExists(id))
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

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ModuleEntity>> PostModuleEntity(ModuleEntity moduleEntity)
        {
            _context.Modules.Add(moduleEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModuleEntity", new { id = moduleEntity.Id }, moduleEntity);
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModuleEntity(Guid id)
        {
            var moduleEntity = await _context.Modules.FindAsync(id);
            if (moduleEntity == null)
            {
                return NotFound();
            }

            _context.Modules.Remove(moduleEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModuleEntityExists(Guid id)
        {
            return _context.Modules.Any(e => e.Id == id);
        }
    }
}
