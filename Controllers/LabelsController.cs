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
    public class LabelsController : ControllerBase
    {
        private readonly ILogger<LabelsController> _logger;
        private readonly Models.DatabaseContext _dbContext;

        public LabelsController(ILogger<LabelsController> logger, Models.DatabaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Models.DTO.LabelDTO> Get()
        {
            return _dbContext.Labels
                .OrderBy(l => l.Name)
                .Select(l => new Models.DTO.LabelDTO(l))
                .AsEnumerable();
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.DTO.LabelDTO> Get(int id)
        {
            Models.Label label =
                _dbContext.Labels
                .AsTracking()
                .FirstOrDefault(l => l.LabelId == id);

            if (label == null)
            {
                return NotFound();
            }

            return new Models.DTO.LabelDTO(label);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Models.DTO.LabelDTO>> Post(Models.DTO.LabelDTO label)
        {
            if (label.Name == null)
            {
                return BadRequest($"Label name not provided");
            }

            if (_dbContext.Labels
                .AsEnumerable()
                .Any(l => string.Equals(l.Name, label.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                return Conflict($"Label '{label.Name}' already exists.");
            }

            Models.Label newLabel = new Models.Label(label);
            newLabel.LabelId = 0;

            _dbContext.Labels.Add(newLabel);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new { id = newLabel.LabelId},
                new Models.DTO.LabelDTO(newLabel)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, Models.DTO.LabelDTO label)
        {
            if (id != label.Id)
            {
                return BadRequest($"Query param id {id} must match body data id {label.Id}");
            }

            if (!_dbContext.Labels
                .Any(l => l.LabelId == id))
            {
                return NotFound();
            }

            _dbContext.Entry(new Models.Label(label)).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
