using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTO;

namespace RoyalVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            var Villas = await _db.Villa.ToListAsync();
            var VillaDTO = _mapper.Map<List<VillaDTO>>(Villas);
            return Ok(VillaDTO);

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VillaDTO>> GetVillaById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Villa Id must be greater than 0");
                }
                var Villa = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if (Villa is null)
                {
                    return NotFound($"Villa with id {id} was not found");
                }
                var villaDto = _mapper.Map<VillaDTO>(Villa);
                return Ok(villaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occured while retriving the villa with id :{id} : {ex.Message}");
            }

        }

        [HttpPost]
        public async Task<ActionResult<VillaCreateDTO>> CreateVilla(VillaCreateDTO villaDTO)
        {
            try
            {
                if (villaDTO is null)
                {
                    return BadRequest("Villa Data Is Required");
                }
                if (ModelState.IsValid)
                {
                    var duplicateVilla = await _db.Villa
                   .FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower());
                    if (duplicateVilla is not null)
                    {
                        return Conflict($"A villa with name '{villaDTO.Name}' is already exists");
                    }
                    Villa villa = _mapper.Map<Villa>(villaDTO);
                    //Villa villa = new Villa()
                    //{
                    //    Name = villaDTO.Name,
                    //    Details = villaDTO.Details,
                    //    ImageUrl = villaDTO.ImageUrl,
                    //    Sqft = villaDTO.Sqft,
                    //    Occupancy = villaDTO.Occupancy,
                    //    CreatedDate = DateTime.Now,
                    //    Rate = villaDTO.Rate

                    //};
                    await _db.Villa.AddAsync(villa);
                    await _db.SaveChangesAsync();
                    var villaDto = _mapper.Map<VillaDTO>(villa);
                    return CreatedAtAction(nameof(GetVillaById), new { id = villa.Id }, villaDto);
                }
                return BadRequest("Villa Data Is Required");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occured while adding the villa : {ex.Message}");
            }

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<VillaUpdateDTO>> UpdateVilla(int id, VillaUpdateDTO villaDTO)
        {
            try
            {
                if (villaDTO is null)
                {
                    return BadRequest("Villa Data Is Required");
                }
                if (id != villaDTO.Id)
                {
                    return BadRequest("Villa Id in URL does not match with Villa Id in request body");
                }

                var existingVilla = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla is null)
                {
                    return NotFound($"Villa with the id {id}Not Found");
                }

                var duplicateVilla = await _db.Villa
                    .FirstOrDefaultAsync(u=>u.Name.ToLower()==villaDTO.Name.ToLower() && u.Id!=id);
                if (duplicateVilla is not null)
                {
                    return Conflict($"A villa with name '{villaDTO.Name}' is already exists");
                }
                _mapper.Map(villaDTO, existingVilla);
                existingVilla.UpdatedDate = DateTime.Now;
                _db.Villa.Update(existingVilla);
                await _db.SaveChangesAsync();
                return Ok(villaDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occured while Updating the villa with id:{id} : {ex.Message}");
            }

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            try
            {
                var existingVilla = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla is null)
                {
                    return NotFound($"Villa with the id {id}Not Found");
                }

                _db.Villa.Remove(existingVilla);
                await _db.SaveChangesAsync();
                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occured while deleting the villa with id:{id} : {ex.Message}");
            }

        }
    }
}
