using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla_DTO;

namespace RoyalVilla_API.Controllers
{
    [Route("api/villa-Amenties")]
    [ApiController]
    public class VillaAmentiesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaAmentiesController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmentiesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaAmentiesDTO>>>> GetVillaAmenties()
        {
            try
            {
                var villaAmenties = await _db.VillaAmenties.ToListAsync();
                var villaAmentiesDTO = _mapper.Map<List<VillaAmentiesDTO>>(villaAmenties);
                var apiResponse = ApiResponse<IEnumerable<VillaAmentiesDTO>>.Ok("Retrieved all villa amenities successfully", villaAmentiesDTO);
                return Ok(apiResponse);
            }
            catch(Exception ex)
            {
                var apiResponse = ApiResponse<IEnumerable<VillaAmentiesDTO>>.Error("An error occurred while retrieving villa amenities", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, apiResponse);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmentiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<VillaAmentiesDTO>>> GetVillaAmentiesById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object?>.NotFound("Villa Amenties Id must be greater than 0"));
                }
                var Villa = await _db.VillaAmenties.FirstOrDefaultAsync(u => u.Id == id);
                if (Villa is null)
                {
                    return NotFound(ApiResponse<object?>.NotFound($"Villa Amenties with id {id} was not found"));
                }
                var VillaAmentiesDTO = _mapper.Map<VillaAmentiesDTO>(Villa);
                var apiResponse = ApiResponse<VillaAmentiesDTO>.Ok($"Retrive Villa Amenties with id {id} Successfully", VillaAmentiesDTO);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<VillaAmentiesDTO>.Error($"An error occured while retriving the villa Amenties with id :{id}", ex.Message);
            }

        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<VillaAmentiesDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<VillaAmentiesDTO>>> CreateVillaAmenties(VillaAmentiesCreateDTO villaAmentiesDTO)
        {
            try
            {
                if (villaAmentiesDTO is null)
                {
                    return BadRequest(ApiResponse<object?>.BadRequest("Villa Amenties Data Is Required", null));
                }
                if (ModelState.IsValid)
                {
                    var villaExist = await _db.Villa
                   .FirstOrDefaultAsync(u => u.Id== villaAmentiesDTO.VillaId);
                    if (villaExist is null)
                    {
                        return Conflict(ApiResponse<object?>.Conflict($"A villa amenities with Id '{villaAmentiesDTO.VillaId}' does not exists"));
                    }
                    VillaAmenties villaAmenties = _mapper.Map<VillaAmenties>(villaAmentiesDTO);
                   
                    villaAmenties.CreatedDate = DateTime.Now;
                    await _db.VillaAmenties.AddAsync(villaAmenties);
                    await _db.SaveChangesAsync();
                    var VillaAmentiesDTO = _mapper.Map<VillaAmentiesDTO>(villaAmenties);
                    VillaAmentiesDTO.Id = villaAmenties.Id;
                    return CreatedAtAction(nameof(CreateVillaAmenties), new { id = villaAmenties.Id }, ApiResponse<VillaAmentiesDTO>.CreatedAt("Villa Created Successfully", VillaAmentiesDTO));
                }
                return BadRequest(ApiResponse<object?>.BadRequest("Villa Amenties  Data Is Required", null));

            }
            catch (Exception ex)
            {
                return ApiResponse<VillaAmentiesDTO>.Error($"An error occured while adding the villa Amenties", ex.Message);
            }

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmentiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<VillaAmentiesDTO>>> UpdateVillaAmenties(int id, VillaAmentiesUpdateDTO villaAmentiesDTO)
        {
            try
            {
                if (villaAmentiesDTO is null)
                {
                    return BadRequest(ApiResponse<object?>.BadRequest("Villa Amenties Data Is Required", null));
                }
                if (id != villaAmentiesDTO.Id)
                {
                    return BadRequest(ApiResponse<object?>.BadRequest("Villa Amenties Id in URL does not match with Villa Amenties Id in request body", null));
                }

                var existingVillaAmenties = await _db.VillaAmenties.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVillaAmenties is null)
                {
                    return NotFound(ApiResponse<object?>.NotFound($"Villa Amenties with the id {id }Not Found"));
                }

                var villaExist = await _db.Villa
                   .FirstOrDefaultAsync(u => u.Id == villaAmentiesDTO.VillaId);
                if (villaExist is null)
                {
                    return Conflict(ApiResponse<object?>.Conflict($"A villa amenities with Id '{villaAmentiesDTO.VillaId}' does not exists"));
                }
                _mapper.Map(villaAmentiesDTO, existingVillaAmenties);
                existingVillaAmenties.UpdatedDate = DateTime.Now;
               _db.VillaAmenties.Update(existingVillaAmenties);
                await _db.SaveChangesAsync();
                var apiResponse = ApiResponse<VillaAmentiesDTO>.Ok($"Updated Villa Amenties with id {id} Successfully", _mapper.Map<VillaAmentiesDTO>(existingVillaAmenties));
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                return ApiResponse<VillaAmentiesDTO>.Error($"An error occured while Updating the villa with id:{id} ", ex.Message);
            }

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVillaAmenties(int id)
        {
            try
            {
                var existingVilla = await _db.VillaAmenties.FirstOrDefaultAsync(u => u.Id == id);
                if (existingVilla is null)
                {
                    return NotFound(ApiResponse<object?>.NotFound($"Villa Amenties with the id {id}Not Found"));
                }

               _db.VillaAmenties.Remove(existingVilla);
                await _db.SaveChangesAsync();
                var apiResponse = ApiResponse<object>.NoContent($"Deleted Villa Amenties with id {id} Successfully");
                return Ok(apiResponse);

            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Error($"An error occured while deleting the villa Amenties with id:{id} ", ex.Message);
            }

        }

    }
}
