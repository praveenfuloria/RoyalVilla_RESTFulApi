using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla_DTO;
using RoyalVilla_Web.Models;
using RoyalVilla_Web.Services.IServices;
using System.Diagnostics;

namespace RoyalVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IVillaService villaService, IMapper mapper)
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
