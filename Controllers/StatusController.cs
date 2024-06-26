using ecomove_back.Data;
using ecomove_back.DTOs.StatusDTOs;
using ecomove_back.Helpers;
using ecomove_back.Interfaces.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecomove_back.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = $"{Roles.ADMIN}")]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepository _StatusRepository;

        public StatusController(IStatusRepository StatusRepository)
        {
            _StatusRepository = StatusRepository;
        }

        /// <summary>
        /// Permet de cr�er un statut de v�hicule
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateStatus(StatusDTO StatusForCreationDTO)
        {
            Response<StatusDTO> response = await _StatusRepository.CreateStatusAsync(StatusForCreationDTO);

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
        /// Permet de supprimer un statut de v�hicule
        /// </summary>
        /// <param name="id">int: identifiant du statuts</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            Response<string> response = await _StatusRepository.DeleteStatusAsync(id);

            if (response.IsSuccess)
                return Ok(response.Message);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de r�cup�rer tout les statuts de v�hicule
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllStatusAsync()
        {

            Response<List<StatusDTO>> response = await _StatusRepository.GetAllStatusAsync();

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);

        }

        /// <summary>
        /// Permet de r�cup�rer un de v�hicule en utilisant son Id
        /// </summary>
        /// <param name="id">int: identifiant du statuts</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStatusByIdAsync(int id)
        {

            Response<StatusDTO> response = await _StatusRepository.GetStatusByIdAsync(id);

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);

        }

        /// <summary>
        /// Permet de mettre � jour un statut de v�hicule
        /// </summary>
        /// <param name="id">int: identifiant du statuts</param>
        /// <param name="statusDTO"></param>
        /// <returns></returns>    
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatusAsync(int id, StatusDTO statusDTO)
        {
            Response<StatusDTO>? response = await _StatusRepository.UpdateStatusAsync(id, statusDTO);

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound();
            else
                return Problem(response.Message);
        }
    }

}
