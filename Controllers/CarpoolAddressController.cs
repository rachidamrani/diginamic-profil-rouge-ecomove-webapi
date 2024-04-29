using ecomove_back.DTOs.AdressDTOs;
using ecomove_back.DTOs.CarpoolAddressDTOs;
using ecomove_back.Helpers;
using ecomove_back.Interfaces.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace ecomove_back.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CarpoolAddressController : ControllerBase
    {
        private readonly ICarpoolAddressRepository _carpoolAddressRepository;

        public CarpoolAddressController(ICarpoolAddressRepository carpoolAddressRepository)
        {
            _carpoolAddressRepository = carpoolAddressRepository;
        }


        /// <summary>
        /// Permet de créer une adresse de covoirutage
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCarpoolAddress(CarpoolAddressDTO carpoolAddressDTO)
        {
            Response<CarpoolAddressOutGoingDTO> response = await _carpoolAddressRepository.CreateCarpoolAddressAsync(carpoolAddressDTO);

            if (response.IsSuccess)
                return Ok(response);
            else
                return Problem(response.Message);
        }



        /// <summary>
        /// Permet de supprimer une adresse de covoirutage
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCarpoolAddress(Guid id)
        {
            Response<CarpoolAddressDTO> response = await _carpoolAddressRepository.DeleteCarpoolAddressAsync(id);

            if (response.IsSuccess)
                return Ok(response);
            else
                return Problem(response.Message);
        }


        /// <summary>
        /// Permet de récupérer toutes les adresses de covoirutage
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCarpoolAddresses()
        {
            Response<List<CarpoolAddressOutGoingDTO>> response = await _carpoolAddressRepository.GetAllCarpoolAddressesAsync();

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }


        /// <summary>
        /// Permet de récupérer une adresse de covoirutage en utilisant son Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarpoolAddressById(Guid id)
        {
            Response<CarpoolAddressOutGoingDTO> response = await _carpoolAddressRepository.GetCarpoolAddressByIdAsync(id);

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }


        /// <summary>
        /// Permet de mettre à jour une adresse de covoirutage en utilisant son Id
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCarpoolAddressAsync(Guid id, CarpoolAddressDTO carpoolAddressDTO)
        {
            Response<CarpoolAddressOutGoingDTO> response = await _carpoolAddressRepository.UpdateCarpoolAddressAsync(id, carpoolAddressDTO);

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }
    }
}