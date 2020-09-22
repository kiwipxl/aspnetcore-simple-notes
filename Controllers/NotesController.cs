using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace notes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly ILogger<NotesController> _logger;
        private readonly Models.DatabaseContext _dbContext;

        public NotesController(ILogger<NotesController> logger, Models.DatabaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Models.DTO.NoteDTO> Get(
            [FromQuery] int start,
            [FromQuery] int count,
            [FromQuery] string[] labels)
        {
            if (labels != null && labels.Length > 0)
            {
                return _dbContext.Notes
                    .Include(n => n.NoteLabels)
                        .ThenInclude(nl => nl.Label)
                    .Where(n => n.NoteLabels.Any(nl => labels.Contains(nl.Label.Name)))
                    .OrderBy(n => n.Created)
                    .Skip(start)
                    .Take(count)
                    .Select(n => new Models.DTO.NoteDTO(n))
                    .AsEnumerable();
            }
            else
            {
                return _dbContext.Notes
                    .Include(n => n.NoteLabels)
                        .ThenInclude(nl => nl.Label)
                    .OrderBy(n => n.Created)
                    .Skip(start)
                    .Take(count)
                    .Select(n => new Models.DTO.NoteDTO(n))
                    .AsEnumerable();
            }
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.DTO.NoteDTO> Get(int id)
        {
            Models.Note note =
                _dbContext.Notes
                .Include(n => n.NoteLabels)
                    .ThenInclude(nl => nl.Label)
                .FirstOrDefault(n => n.NoteId == id);

            if (note == null)
            {
                return NotFound();
            }

            return new Models.DTO.NoteDTO(note);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Models.DTO.NoteDTO>> Post(Models.DTO.NoteDTO note)
        {
            Models.Note newNote = null;

            try
            {
                newNote = new Models.Note(note, _dbContext);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // Update timestamps
            newNote.Created = DateTime.Now;
            newNote.Edited = newNote.Created;

            _dbContext.Notes.Add(newNote);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get), 
                new { id = newNote.NoteId },
                new Models.DTO.NoteDTO(newNote)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, Models.DTO.NoteDTO note)
        {
            if (id != note.Id)
            {
                return BadRequest($"Query param id {id} must match body data id {note.Id}");
            }

            Models.Note dbNote = _dbContext.Notes
                .Include(n => n.NoteLabels)
                .AsTracking()
                .FirstOrDefault(n => n.NoteId == id);
            
            if (dbNote == null)
            {
                return NotFound();
            }

            try
            {
                dbNote.SetFrom(note, _dbContext);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // TODO: support JsonPatch for `PATCH /api/notes/{id}`
    }
}
