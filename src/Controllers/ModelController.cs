using Ecomove.Api.Data;
using Ecomove.Api.DTOs.ModelDTOs;
using Ecomove.Api.Helpers;
using Ecomove.Api.Interfaces.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomove.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = $"{Roles.ADMIN}")]
    public class ModelController : ControllerBase
    {
        private readonly IModelRepository _modelRepository;

        public ModelController(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        /// <summary>
        /// Permet de cr�er un mod�le de v�hicule
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateModelAsync(ModelFKeyDTO modelFKeyDTO)
        {
            Response<ModelLabelDTO> response = await _modelRepository.CreateModelAsync(modelFKeyDTO);

            if (response.IsSuccess)
                return Ok(response);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de supprimer un mod�le de v�hicule
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModelAsync(int id)
        {
            Response<string> response = await _modelRepository.DeleteModelAsync(id);

            if (response.IsSuccess)
                return Ok(response.Message);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de r�cup�rer toutes les mod�les de v�hicule
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllModels()
        {
            Response<List<ModelLabelDTO>> response = await _modelRepository.GetAllModelsAsync();

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de r�cup�rer un mod�le de v�hicule en utilisant son Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetModelById(int id)
        {

            Response<ModelLabelDTO> response = await _modelRepository.GetModelByIdAsync(id);

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de modifier un mod�le
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModelById(int id, ModelLabelDTO modelLabelDTO)
        {
            Response<ModelLabelDTO> response = await _modelRepository.UpdateModelByIdAsync(id, modelLabelDTO);

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }
    }
}

