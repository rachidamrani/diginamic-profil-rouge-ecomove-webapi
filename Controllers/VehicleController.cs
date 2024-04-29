using ecomove_back.DTOs.VehicleDTOs;
using ecomove_back.Helpers;
using ecomove_back.Interfaces.IRepositories;
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
        /// Permet de créer un véhicule
        /// </summary>
        [HttpPost]
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
        /// Permet de récupérer tous les véhicules
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllVehiclesAsync()
        {
            var response = await _vehicleRepository.GetAllVehiclesAsync();
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
        /// Permet de trouver un véhicule avec un ID
        /// </summary>
        [HttpGet("{id}")]
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
        /// Permet de trouver un véhicule avec un ID pour un admin
        /// </summary>
        [HttpGet("{id}/admin")]
        public async Task<IActionResult> GetVehicleByIdForAdmin(Guid id)
        {
            Response<VehicleForGetByIdForAdminDTO> response = await _vehicleRepository.GetVehicleByIdForAdminAsync(id);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            else
            {
                return StatusCode(response.CodeStatus, response.Message);
            }
        }

        /// <summary>
        /// Permet de supprimer un véhicule
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            Response<bool> response = await _vehicleRepository.DeleteVehicleAsync(id);
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
        /// Permet de mettre à jour un véhicule
        /// </summary>
        [HttpPut("{id}")]
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
        /// Permet de changer le statut d'un véhicule
        /// </summary>
        [HttpPut("{id}")]
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
