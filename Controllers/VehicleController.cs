using ecomove_back.Data;
using ecomove_back.DTOs.VehicleDTOs;
using ecomove_back.Helpers;
using ecomove_back.Interfaces.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecomove_back.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// Permet de cr�er un v�hicule
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{Roles.ADMIN}")]
        public async Task<IActionResult> CreateVehicleAsync(VehicleForCreateDTO vehicleForCreationDTO)
        {
            Response<VehicleForCreateDTO> response = await _vehicleRepository.CreateVehicleAsync(vehicleForCreationDTO);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return Problem(response.Message);
            }
        }

        /// <summary>
        /// Permet de r�cup�rer tous les v�hicules
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllVehiclesAsync()
        {
            Response<List<VehicleForGetDTO>> response = await _vehicleRepository.GetAllVehiclesAsync();

            if (response.IsSuccess)
                return Ok(response.Data);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de trouver un v�hicule avec un ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetVehicleById(Guid id)
        {
            Response<VehicleForGetDTO> response = await _vehicleRepository.GetVehicleByIdAsync(id);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            else
            {
                return Problem(response.Message);
            }
        }

        /// <summary>
        /// Permet de trouver un v�hicule avec un ID pour un admin
        /// </summary>
        [HttpGet("{id}/admin")]
        [Authorize(Roles = $"{Roles.ADMIN}")]
        public async Task<IActionResult> GetVehicleByIdForAdmin(Guid id)
        {
            Response<VehicleForGetByIdForAdminDTO> response = await _vehicleRepository.GetVehicleByIdForAdminAsync(id);
            if (response.IsSuccess)
                return Ok(response.Data);
            else
                return StatusCode(response.CodeStatus, response.Message);
        }

        /// <summary>
        /// Permet de supprimer un v�hicule
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{Roles.ADMIN}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            Response<bool> response = await _vehicleRepository.DeleteVehicleAsync(id);
            if (response.IsSuccess)
                return Ok(response);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de mettre � jour un v�hicule
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = $"{Roles.ADMIN}")]
        public async Task<IActionResult> UpdateVehicle(Guid id, VehicleForUpdateDTO vehicleDto)
        {
            Response<VehicleForUpdateDTO> response = await _vehicleRepository.UpdateVehicleAsync(id, vehicleDto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return Problem(response.Message);
            }
        }

        /// <summary>
        /// Permet de changer le statut d'un v�hicule
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = $"{Roles.ADMIN}")]
        public async Task<IActionResult> ChangeVehicleStatus(Guid id, VehicleForChangeStatusDTO statusDto)
        {
            Response<VehicleForChangeStatusDTO> response = await _vehicleRepository.ChangeVehicleStatusAsync(id, statusDto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return Problem(response.Message);
            }
        }
    }
}
