using Lab2_2.Data;
using Lab2_2.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2_2.Controllers
{
    [ApiController]
    [Route("person/[action]")]
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        // GET: Person
        public async Task<IActionResult> Index()
        {
              return _context.Persons != null ? 
                          Ok(await _context.Persons.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Persons'  is null.");
        }

        [HttpGet]
        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Patronymic,BirthDate,Gender,IssueDate,IssuePlace")] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(person);
            }

            _context.Add(person);
            await _context.SaveChangesAsync();
            return Ok();        
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit([Bind("Id,Name,Surname,Patronymic,BirthDate,Gender,IssueDate,IssuePlace")] Person person)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return BadRequest(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Persons == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Persons'  is null.");
            }
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
            }
            
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool PersonExists(int id)
        {
          return (_context.Persons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
