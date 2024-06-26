﻿using ecomove_back.Data;
using ecomove_back.Data.Models;
using ecomove_back.DTOs.AppUserDTOs;
using ecomove_back.Helpers;
using ecomove_back.Interfaces.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ecomove_back.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = $"{Roles.ADMIN}")]
    public class AppUserController : ControllerBase
    {
        private IAppUserRepository _appUserRepository;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public AppUserController(
            IAppUserRepository appUserRepository,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _appUserRepository = appUserRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        /// <summary>
        /// Permet de créer un utilisateur avec le rôle USER
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUser(CreateAppUserDTO userDTO)
        {
            Response<string> response = await _appUserRepository.CreateUserAsync(userDTO);

            if (response.IsSuccess)
                return Ok(response);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de récupérer la liste de tous les utilisateurs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsers()
        {
            Response<List<AllUsersDTO>> response = await _appUserRepository.GetAllUsersAysnc();

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de récuperer un utilisateur avec son id
        /// </summary>
        /// <param name="id">string : identifiant de l'utilisateur (Guid)</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById(string id)
        {
            Response<UserDTO> response = await _appUserRepository.GetUserByIdAysnc(id);

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de modifier les infos d'un utilisateur
        /// </summary>
        /// <param name="id">string : identifiant de l'utilisateur (Guid)</param>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUserById(string id, UpdateUserDTO userDTO)
        {
            Response<UpdateUserDTO> response = await _appUserRepository.UpdateUserAysnc(id, userDTO);

            if (response.IsSuccess)
                return Ok(response);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }

        /// <summary>
        /// Permet de supprimer un utilisateur
        /// </summary>
        /// <param name="id">string : identifiant de l'utilisateur (Guid)</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            Response<string> response = await _appUserRepository.DeleteUserAsync(id);

            if (response.IsSuccess)
                return Ok(response.Message);
            else if (response.CodeStatus == 404)
                return NotFound(response.Message);
            else
                return Problem(response.Message);
        }
    }
}