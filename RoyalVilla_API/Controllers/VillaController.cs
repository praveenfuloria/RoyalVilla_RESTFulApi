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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaDTO>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaDTO>>>> GetVillas()
        {
            var Villas = await _db.Villa.ToListAsync();
            var DTOResponseVilla = _mapper.Map<List<VillaDTO>>(Villas);
            var apiResponse = ApiResponse<IEnumerable<VillaDTO>>.Ok("Retrive All the Villas Successfully", DTOResponseVilla);
            return Ok(apiResponse);

        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> GetVillaById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object?>.NotFound("Villa Id must be greater than 0"));
                }
                var Villa = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if (Villa is null)
                {
                    return NotFound(ApiResponse<object?>.NotFound($"Villa with id {id} was not found"));
                }
                var villaDto = _mapper.Map<VillaDTO>(Villa);
                var apiResponse = ApiResponse<VillaDTO>.Ok($"Retrive Villa with id {id} Successfully", villaDto);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<VillaDTO>.Error($"An error occured while retriving the villa with id :{id}", ex.Message);
            }

        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<VillaDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> CreateVilla(VillaCreateDTO villaDTO)
        {
            try
            {
                if (villaDTO is null)
                {
                    return BadRequest(ApiResponse<object?>.BadRequest("Villa Data Is Required",null));
                }
                if (ModelState.IsValid)
                {
                    var duplicateVilla = await _db.Villa
                   .FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower());
                    if (duplicateVilla is not null)
                    {
                        return Conflict(ApiResponse<object?>.Conflict($"A villa with name '{villaDTO.Name}' is already exists"));
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
                    villaDto.Id = villa.Id;
                    return CreatedAtAction(nameof(CreateVilla),new { id = villa.Id},ApiResponse<VillaDTO>.CreatedAt("Villa Created Successfully", villaDto));
                }
                return BadRequest(ApiResponse<object?>.BadRequest("Villa Data Is Required",null));

            }
            catch (Exception ex)
            {
                return ApiResponse<VillaDTO>.Error($"An error occured while adding the villa", ex.Message);
            }

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> UpdateVilla(int id, VillaUpdateDTO villaDTO)
        {
            try
            {
                if (villaDTO is null)
                {
                    return BadRequest(ApiResponse<object?>.BadRequest("Villa Data Is Required",null));
                }
                if (id != villaDTO.Id)
                {
                    return BadRequest(ApiResponse<object?>.BadRequest("Villa Id in URL does not match with Villa Id in request body",null));
                }

                var existingVilla = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla is null)
                {
                    return NotFound(ApiResponse<object?>.NotFound($"Villa with the id {id}Not Found"));
                }

                var duplicateVilla = await _db.Villa
                    .FirstOrDefaultAsync(u=>u.Name.ToLower()==villaDTO.Name.ToLower() && u.Id!=id);
                if (duplicateVilla is not null)
                {
                    return Conflict(ApiResponse<object?>.Conflict($"A villa with name '{villaDTO.Name}' is already exists"));
                }
                _mapper.Map(villaDTO, existingVilla);
                existingVilla.UpdatedDate = DateTime.Now;
                _db.Villa.Update(existingVilla);
                await _db.SaveChangesAsync();
                var apiResponse = ApiResponse<VillaDTO>.Ok($"Updated Villa with id {id} Successfully", _mapper.Map<VillaDTO>(villaDTO));
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                return ApiResponse<VillaDTO>.Error($"An error occured while Updating the villa with id:{id} ", ex.Message);
            }

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVilla(int id)
        {
            try
            {
                var existingVilla = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla is null)
                {
                    return NotFound(ApiResponse<object?>.NotFound($"Villa with the id {id}Not Found"));
                }

                _db.Villa.Remove(existingVilla);
                await _db.SaveChangesAsync();
                var apiResponse = ApiResponse<object>.NoContent($"Deleted Villa with id {id} Successfully");
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Error($"An error occured while deleting the villa with id:{id} ", ex.Message);
            }

        }
    }
}
