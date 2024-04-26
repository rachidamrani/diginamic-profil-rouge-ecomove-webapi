using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecomove_back.Data;
using ecomove_back.Data.Models;
using ecomove_back.DTOs.VehicleMotorizationDTOs;
using ecomove_back.Helpers;
using ecomove_back.Interfaces.IRepositories;
using ecomove_back.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ecomove_back.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VehicleMotorizationController : ControllerBase
    {
        private readonly IMotorizationRepository _vehicleMotorizationRepository;

        public VehicleMotorizationController(IMotorizationRepository vehiculeMotorizationRepository)
        {
            _vehicleMotorizationRepository = vehiculeMotorizationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicleMotorization(VehicleMotorizationDTO vehicleMotorizationDTO)
        {
            Response<VehicleMotorizationDTO> response = await _vehicleMotorizationRepository.CreateVehicleMotorizationAsync(vehicleMotorizationDTO);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            else
            {
                return Problem(response.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleMotorization(int id)
        {
            Response<string> response = await _vehicleMotorizationRepository.DeleteVehicleMotorizationAsync(id);

            if (response.IsSuccess)
                return Ok(response.Message);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicleMotorizations()
        {
            Response<List<Motorization>> response = await _vehicleMotorizationRepository.GetAllVehicleMotorizationsAsync();

            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            else if (response.CodeStatus == 404)
            {
                return NotFound(response.Message);
            }
            else
            {
                return Problem(response.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleMotorizationById(int id)
        {
            Response<int> response = await _vehicleMotorizationRepository.GetVehicleMotorizationByIdAsync(id);
            
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else if (response.CodeStatus == 404)
            {
                return NotFound(response.Message);
            }
            else
            {
                return Problem(response.Message);
            }
        }
    }      
}