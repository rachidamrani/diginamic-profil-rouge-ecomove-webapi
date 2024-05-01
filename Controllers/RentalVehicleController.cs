using ecomove_back.Data.Models;
using ecomove_back.DTOs.RentalVehicleDTO;
using ecomove_back.Helpers;
using ecomove_back.Interfaces.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ecomove_back.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RentalVehicleController : ControllerBase
    {
        private IRentalVehicleRepository _rentalVehicleRepository;
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public RentalVehicleController(
            IRentalVehicleRepository rentalVehicleRepository,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _rentalVehicleRepository = rentalVehicleRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Permet de cr�er une r�servation de v�hicule
        /// </summary>
        /// <param name="vehicleId">Guid : identifiant du vehicule</param>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPost("{vehicleId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRentalVehicle(Guid vehicleId, RentalVehicleDTO userDTO)
        {
            string userId = "203d7613-ab34-4dee-86f0-56eb1ee205bd";

            Response<string> response = await _rentalVehicleRepository.CreateRentalVehicleAsync(userId, vehicleId, userDTO);

            if (response.IsSuccess)
                return Ok(response);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de modifier les dates d'une r�servation
        /// </summary>
        /// <param name="rentalId">int : identifiant de la r�servation</param>
        /// <param name="rentalVehicleDTO"></param>
        /// <returns></returns>
        [HttpPut("{rentalId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRentalVehicle(Guid rentalId, RentalVehicleDTO rentalVehicleDTO)
        {
            Response<RentalVehicleDTO> response = await _rentalVehicleRepository.UpdateRentalVehicleAsync(rentalId, rentalVehicleDTO);

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet d'annuler une r�servation de v�hicule
        /// </summary>
        /// <param name="rentalId">int : identifiant de la r�servation</param>
        /// <returns></returns>
        [HttpPut("{rentalId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CancelRentalVehicle(int rentalId)
        {
            Response<string> response = await _rentalVehicleRepository.CancelRentalVehicleAsync(rentalId);

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de r�cup�rer toutes les r�servations de v�hicule
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRentalVehicles()
        {
            Response<List<AllRentalVehicles>> response = await _rentalVehicleRepository.GetAllRentalVehiclesAysnc();

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de r�cup�rer une r�servation de v�hicule avec son id
        /// </summary>
        /// <param name="rentalId">int : identifiant de la r�servation</param>
        /// <returns></returns>
        [HttpGet("{rentalId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRentalVehicleById(int rentalId)
        {
            Response<SingleRentalVehicleDTO> response = await _rentalVehicleRepository.GetRentalVehicleByIdAysnc(rentalId);

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }


    }
}