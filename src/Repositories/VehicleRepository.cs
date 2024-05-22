using ecomove_back.Data;
using ecomove_back.Data.Models;
using ecomove_back.DTOs.VehicleDTOs;
using ecomove_back.Helpers;
using ecomove_back.Interfaces.IRepositories;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;


namespace ecomove_back.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private EcoMoveDbContext _ecoMoveDbContext;

        public VehicleRepository(EcoMoveDbContext ecoMoveDbContext)
        {
            _ecoMoveDbContext = ecoMoveDbContext;
        }

        public async Task<Response<VehicleForCreateDTO>> CreateVehicleAsync(VehicleForCreateDTO vehicleCreate)
        {
            try
            {
                Vehicle vehicle = new Vehicle
                {
                    CarSeatNumber = vehicleCreate.CarSeatNumber,
                    Registration = vehicleCreate.Registration,
                    Photo = vehicleCreate.Photo,
                    CO2emission = vehicleCreate.CO2emission,
                    Consumption = vehicleCreate.Consumption,
                    ModelId = vehicleCreate.ModelId,
                    MotorizationId = vehicleCreate.MotorizationId,
                    CategoryId = vehicleCreate.CategoryId,
                    StatusId = 1
                };

                await _ecoMoveDbContext.Vehicles.AddAsync(vehicle);
                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<VehicleForCreateDTO>
                {
                    Data = vehicleCreate,
                    IsSuccess = true,
                    Message = "Vehicle cr�e avec succ�s",
                    CodeStatus = 201
                };
            }
            catch (Exception e)
            {
                return new Response<VehicleForCreateDTO>
                {
                    IsSuccess = false,
                    Message = "Une erreur s'est produite lors de la cr�ation du v�hicule: " + e.Message,
                };
            }
        }

        public async Task<Response<List<VehicleForGetDTO>>> GetAllVehiclesAsync()
        {
            List<Vehicle> vehicles = await _ecoMoveDbContext.Vehicles
                .Include(v => v.Model)
                .Include(b => b.Model.Brand)
                .Include(v => v.Category)
                .ToListAsync();

            if (vehicles.Count == 0)
            {
                return new Response<List<VehicleForGetDTO>>
                {
                    Message = "Aucun v�hicule trouv�.",
                    IsSuccess = false,
                    CodeStatus = 404,
                };
            }

            List<VehicleForGetDTO> vehiclesDTO = new List<VehicleForGetDTO>();

            foreach (var v in vehicles)
            {
                vehiclesDTO.Add(new VehicleForGetDTO
                {
                    VehicleId = v.VehicleId,
                    BrandLabel = v.Model.Brand.BrandLabel,
                    ModelLabel = v.Model.ModelLabel,
                    CategoryLabel = v.Category.CategoryLabel,
                    CarSeatNumber = v.CarSeatNumber,
                    Registration = v.Registration,
                    Photo = v.Photo,
                    CO2emission = v.CO2emission,
                    Consumption = v.Consumption
                });
            }

            return new Response<List<VehicleForGetDTO>>
            {
                Data = vehiclesDTO,
                IsSuccess = true,
                CodeStatus = 200,
            };
        }


        public async Task<Response<VehicleForGetDTO>> GetVehicleByIdAsync(Guid id)
        {
            try
            {
                Vehicle? vehicle = await _ecoMoveDbContext.Vehicles
                    .Include(v => v.Model)
                    .Include(b => b.Model.Brand)
                    .Include(v => v.Status)
                    .Include(v => v.Category)
                    .Include(v => v.Motorization)
                    .FirstOrDefaultAsync(v => v.VehicleId == id);

                if (vehicle == null)
                {
                    return new Response<VehicleForGetDTO>
                    {
                        Message = "Le v�hicule que vous cherchez n'existe pas.",
                        IsSuccess = false,
                        CodeStatus = 404,
                    };
                }

                VehicleForGetDTO vehicleDTO = new VehicleForGetDTO
                {
                    VehicleId = vehicle.VehicleId,
                    BrandLabel = vehicle.Model.Brand.BrandLabel,
                    ModelLabel = vehicle.Model.ModelLabel,
                    CategoryLabel = vehicle.Category.CategoryLabel,
                    CarSeatNumber = vehicle.CarSeatNumber,
                    Registration = vehicle.Registration,
                    Photo = vehicle.Photo,
                    CO2emission = vehicle.CO2emission,
                    Consumption = vehicle.Consumption,

                };

                return new Response<VehicleForGetDTO>
                {
                    Data = vehicleDTO,
                    IsSuccess = true,
                    CodeStatus = 200,
                };
            }
            catch (Exception e)
            {
                return new Response<VehicleForGetDTO>
                {
                    IsSuccess = false,
                    Message = "Une erreur s'est produite lors de la r�cup�ration du v�hicule: " + e.Message,
                    CodeStatus = 500
                };
            }
        }

        public async Task<Response<VehicleForGetByIdForAdminDTO>> GetVehicleByIdForAdminAsync(Guid vehicleId)
        {
            try
            {
                var vehicle = await _ecoMoveDbContext.Vehicles
                    .Include(v => v.Status)
                    .Include(v => v.Model)
                    .ThenInclude(b => b.Brand)
                    .Include(v => v.Category)
                    .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);

                if (vehicle == null)
                {
                    return new Response<VehicleForGetByIdForAdminDTO>
                    {
                        IsSuccess = false,
                        Message = "V�hicule non trouv�.",
                        CodeStatus = 404
                    };
                }

                var vehicleDto = new VehicleForGetByIdForAdminDTO
                {
                    VehicleId = vehicle.VehicleId,
                    BrandLabel = vehicle.Model.Brand.BrandLabel,
                    ModelLabel = vehicle.Model.ModelLabel,
                    CategoryLabel = vehicle.Category.CategoryLabel,
                    CarSeatNumber = vehicle.CarSeatNumber,
                    Registration = vehicle.Registration,
                    Photo = vehicle.Photo,
                    CO2emission = vehicle.CO2emission,
                    Consumption = vehicle.Consumption,
                    StatusLabel = vehicle.Status.StatusLabel
                };

                return new Response<VehicleForGetByIdForAdminDTO>
                {
                    Data = vehicleDto,
                    IsSuccess = true,
                    Message = "V�hicule r�cup�r� avec succ�s.",
                    CodeStatus = 200
                };
            }
            catch (Exception e)
            {
                return new Response<VehicleForGetByIdForAdminDTO>
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la r�cup�ration du v�hicule: {e.Message}",
                    CodeStatus = 500
                };
            }
        }

        //verifier les reservation, covoit avec un m�thode
        public async Task<Response<bool>> DeleteVehicleAsync(Guid vehicleId)
        {
            try
            {
                Vehicle? vehicle = await _ecoMoveDbContext.Vehicles
                    .Include(v  => v.RentalVehicles)
                    .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);

                if (vehicle == null)
                {
                    return new Response<bool>
                    {
                        IsSuccess = false,
                        Message = "V�hicule non trouv�.",
                        CodeStatus = 404
                    };
                }

                // V�rifier que le v�hicule n'a pas de r�servation
                if (vehicle.RentalVehicles.Count != 0)
                {
                    return new Response<bool>
                    {
                        Message = "Vous ne pouvez pas supprimer ce v�hicule car des r�servations y sont associ�s",
                        IsSuccess = false,
                        CodeStatus = 404
                    };
                }

                _ecoMoveDbContext.Vehicles.Remove(vehicle);
                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = "V�hicule supprim� avec succ�s.",
                    CodeStatus = 200
                };
            }
            catch (Exception e)
            {
                return new Response<bool>
                {
                    IsSuccess = false,
                    Message = $"Une erreur s'est produite lors de la suppression du v�hicule : {e.Message}",
                    CodeStatus = 500
                };
            }

        }



        public async Task<Response<VehicleForUpdateDTO>> UpdateVehicleAsync(Guid vehicleId, VehicleForUpdateDTO vehicleUpdate)
        {
            try
            {
                var vehicle = await _ecoMoveDbContext.Vehicles
                    .Include(v => v.RentalVehicles)
                    .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);

                if (vehicle == null)
                {
                    return new Response<VehicleForUpdateDTO>
                    {
                        IsSuccess = false,
                        Message = "V�hicule non trouv�.",
                        CodeStatus = 404
                    };
                }

                // V�rifier que le v�hicule n'a pas de r�servation
                if (vehicle.RentalVehicles.Count != 0)
                {
                    return new Response<VehicleForUpdateDTO>
                    {
                        Message = "Vous ne pouvez pas modifier ce v�hicule car des r�servations y sont associ�s",
                        IsSuccess = false,
                        CodeStatus = 404
                    };
                }

                vehicle.ModelId = vehicleUpdate.ModelId;
                vehicle.CategoryId = vehicleUpdate.CategoryId;
                vehicle.CarSeatNumber = vehicleUpdate.CarSeatNumber;
                vehicle.Registration = vehicleUpdate.Registration;
                vehicle.Photo = vehicleUpdate.Photo;
                vehicle.CO2emission = vehicleUpdate.CO2emission;
                vehicle.Consumption = vehicleUpdate.Consumption;
                vehicle.StatusId = vehicleUpdate.StatusId;

                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<VehicleForUpdateDTO>
                {
                    Data = vehicleUpdate,
                    IsSuccess = true,
                    Message = "V�hicule mis � jour avec succ�s.",
                    CodeStatus = 200
                };
            }
            catch (Exception e)
            {
                return new Response<VehicleForUpdateDTO>
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la mise � jour du v�hicule: {e.Message}",
                    CodeStatus = 500
                };
            }
        }

        //verification avec une m�thode
        public async Task<Response<VehicleForChangeStatusDTO>> ChangeVehicleStatusAsync(Guid vehicleId, VehicleForChangeStatusDTO statusDto)
        {
            try
            {
                var vehicle = await _ecoMoveDbContext.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
                if (vehicle == null)
                {
                    return new Response<VehicleForChangeStatusDTO>
                    {
                        IsSuccess = false,
                        Message = "V�hicule non trouv�.",
                        CodeStatus = 404
                    };
                }

                vehicle.StatusId = statusDto.StatusId;
                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<VehicleForChangeStatusDTO>
                {
                    IsSuccess = true,
                    Message = "Statut du v�hicule mis � jour avec succ�s.",
                    CodeStatus = 200
                };
            }
            catch (Exception e)
            {
                return new Response<VehicleForChangeStatusDTO>
                {
                    IsSuccess = false,
                    Message = $"Erreur lors de la mise � jour du statut du v�hicule: {e.Message}",
                    CodeStatus = 500
                };
            }
        }
    }
}
