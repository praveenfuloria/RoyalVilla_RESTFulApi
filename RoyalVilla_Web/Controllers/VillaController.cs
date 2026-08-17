using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla_DTO;
using RoyalVilla_Web.Models;
using RoyalVilla_Web.Services.IServices;
using System.Diagnostics;

namespace RoyalVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(ILogger<HomeController> logger, IVillaService villaService, IMapper mapper)
        {
            _logger = logger;
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<VillaDTO> villaList = new();
            try
            {
                var response = await _villaService.GetAllAsync<ApiResponse<List<VillaDTO>>>("");
                if (response != null && response.Success && response.Data != null)
                {
                    villaList = response.Data;
                }
                else
                {
                    _logger.LogWarning("Failed to fetch villa data. Response: {Response}", response);
                    // Optionally, you can set a TempData message or ViewBag to show an error message in the view.
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred while fetching villa data. Please try again later. {ex.Message}";
                _logger.LogError(ex, "An error occurred while fetching villa data.");
                // Optionally, you can set a TempData message or ViewBag to show an error message in the view.
            }
            return View(villaList);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(VillaCreateDTO createDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _villaService.CreateAsync<ApiResponse<VillaDTO>>(createDTO, "");
                    TempData["success"] = $"Villa Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Invalid data. Please check the input fields.";
                    return View(createDTO);
                    
                }
            }
            catch (Exception ex)
            {
                
                TempData["error"] = $"An error occurred while fetching villa data. Please try again later. {ex.Message}";
                _logger.LogError(ex, "An error occurred while fetching villa data.");
                // Optionally, you can set a TempData message or ViewBag to show an error message in the view.
                return View(createDTO);
            }
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["error"] = "Invalid villa ID.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                try
                {
                    var response = _villaService.GetAsync<ApiResponse<VillaDTO>>(id, "").Result;
                    if (response == null || !response.Success || response.Data == null)
                    {
                        TempData["error"] = "Villa not found.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var villaToDelete = response.Data;
                        return View(villaToDelete);
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"An error occurred while fetching villa data. Please try again later. {ex.Message}";
                    _logger.LogError(ex, "An error occurred while fetching villa data.");
                    // Optionally, you can set a TempData message or ViewBag to show an error message in the view.
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(VillaDTO villaDTO)
        {
            try
            {
                    var response = await _villaService.DeleteAsync<ApiResponse<object>>(villaDTO.Id, "");
                    TempData["success"] = $"Villa Deleted Successfully";                
            }
            catch (Exception ex)
            {

                TempData["error"] = $"An error occurred while fetching villa data. Please try again later. {ex.Message}";
                _logger.LogError(ex, "An error occurred while fetching villa data.");
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["error"] = "Invalid villa ID.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                try
                {
                    var response = _villaService.GetAsync<ApiResponse<VillaUpdateDTO>>(id, "").Result;
                    if (response == null || !response.Success || response.Data == null)
                    {
                        TempData["error"] = "Villa not found.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var villaToUpdate = response.Data;
                        return View(villaToUpdate);
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"An error occurred while fetching villa data. Please try again later. {ex.Message}";
                    _logger.LogError(ex, "An error occurred while fetching villa data.");
                    // Optionally, you can set a TempData message or ViewBag to show an error message in the view.
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(VillaUpdateDTO villaUpdateDTO)
        {
            try
            {
                var response = await _villaService.UpdateAsync<ApiResponse<VillaDTO>>(villaUpdateDTO, "");
                TempData["success"] = $"Villa Updated Successfully";
            }
            catch (Exception ex)
            {

                TempData["error"] = $"An error occurred while fetching villa data. Please try again later. {ex.Message}";
                _logger.LogError(ex, "An error occurred while fetching villa data.");
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
