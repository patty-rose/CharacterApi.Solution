using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharacterApi.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CharacterApi.AddControllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CharactersController : ControllerBase
  {
    private readonly CharacterApiContext _db;

    public CharactersController(CharacterApiContext db)
    {
      _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Character>>> Get(string firstName, string lastName, int minimumAge)
    {
      var query = _db.Characters.AsQueryable();//=> a queryable LINQ object

      if (firstName != null)
      {
        query = query.Where(entry => entry.FirstName == firstName);
      }
      if (lastName != null)
      {
        query = query.Where(entry => entry.LastName == lastName);
      }
      if (minimumAge > 0)//Because integers in C# are non-nullable data types the default for an integer value parameter will be 0
      {
        query = query.Where(entry => entry.Age >= minimumAge);
      }
      return await query.ToListAsync();//turn our new results into a list.
    }

    // POST api/Characters -- Our POST route utilizes the function CreatedAtAction. This is so that it can end up returning the Character object to the user, as well as update the status code to 201, for "Created", rather than the default 200 OK.
    [HttpPost]
    public async Task<ActionResult<Character>> Post(Character character)
    {
      _db.Characters.Add(character);
      await _db.SaveChangesAsync();

      return CreatedAtAction(nameof(GetCharacter), new { id = character.CharacterId }, character);//This first argument affects the value of the Location in the response header -- we'll change it to the result of our GetCharacter route. Upon creation, the result contains a link to where that newly-created object can be found with a GET request:
    }

    // GET: api/Characters/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Character>> GetCharacter(int id)
    {
        var character = await _db.Characters.FindAsync(id);

        if (character == null)
        {
            return NotFound();
        }

        return character;
    }

    [HttpPut("{id}")] //PUT is like POST in that it makes a change to the server. However, PUT changes existing information while POST creates new information. a PUT request requires the client to send the entire updated entity, not just the changes. To support partial updates, use PATCH.
    public async Task<IActionResult> Put(int id, Character character)
    {
      if (id != character.CharacterId)
      {
        return BadRequest();
      }

      _db.Entry(character).State = EntityState.Modified;

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!CharacterExists(id))
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

    private bool CharacterExists(int id)
    {
      return _db.Characters.Any(e => e.CharacterId == id);
    }

    [HttpDelete("{id}")]//Ultimately, the main difference between update and delete methods in a web application versus an API is the annotation. Remember that forms in HTML5 don't allow for PUT, PATCH or DELETE verbs.
    public async Task<IActionResult> DeleteCharacter(int id)
    {
      var character = await _db.Characters.FindAsync(id);
      if (character == null)
      {
        return NotFound();
      }

      _db.Characters.Remove(character);
      await _db.SaveChangesAsync();

      return NoContent();
    }
  }
}