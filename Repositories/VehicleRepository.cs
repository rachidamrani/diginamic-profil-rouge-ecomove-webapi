using ecomove_back.Data;
using ecomove_back.Data.Models;
using ecomove_back.DTOs.VehicleDTOs;
using ecomove_back.Helpers;
using ecomove_back.Interfaces.IRepositories;
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
                    CategoryId = vehicleCreate.CategoryId
                };

                _ecoMoveDbContext.Vehicles.Add(vehicle);
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
                    Message = $"Error creating vehicle: {e.Message}",
                    CodeStatus = 500  
                };
            }
        }

        public async Task<List<VehicleForGetDTO>> GetAllVehiclesAsync()
        {
            var vehicles = await _ecoMoveDbContext.Vehicles
                .Include(v => v.Model)
                    //.ThenInclude(m => m.Brand)
                .Include(v => v.Category)
                .ToListAsync();

            return vehicles.Select(v => new VehicleForGetDTO
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
            }).ToList();
        }
        public async Task<Response<VehicleForGetDTO>> GetVehicleByIdAsync(Guid id)  
        {
            try
            {
                Vehicle? vehicle = await _ecoMoveDbContext.Vehicles
                    .Include(v => v.Model) 
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
                        //.ThenInclude(m => m.Brand)
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
                var vehicle = await _ecoMoveDbContext.Vehicles.FindAsync(vehicleId);
                if (vehicle == null)
                {
                    return new Response<bool>
                    {
                        IsSuccess = false,
                        Message = "V�hicule non trouv�.",
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
                var vehicle = await _ecoMoveDbContext.Vehicles.FindAsync(vehicleId);
                if (vehicle == null)
                {
                    return new Response<VehicleForUpdateDTO>
                    {
                        IsSuccess = false,
                        Message = "V�hicule non trouv�.",
                        CodeStatus = 404
                    };
                }

                vehicle.Model.ModelId = vehicleUpdate.ModelId;  
                //vehicle.Model.Brand.BrandId = vehicleUpdate.BrandId;  
                vehicle.Category.CategoryId = vehicleUpdate.CategoryId;
                vehicle.CarSeatNumber = vehicleUpdate.CarSeatNumber;
                vehicle.Registration = vehicleUpdate.Registration;
                vehicle.Photo = vehicleUpdate.Photo;
                vehicle.CO2emission = vehicleUpdate.CO2emission;
                vehicle.Consumption = vehicleUpdate.Consumption;
                vehicle.Status.StatusLabel = vehicleUpdate.StatusLabel;

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
        public async Task<Response<bool>> ChangeVehicleStatusAsync(Guid vehicleId, VehicleForChangeStatusDTO statusDto)
        {
            try
            {
                Vehicle? vehicle = await _ecoMoveDbContext.Vehicles.Include(v => v.Status).FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
                if (vehicle == null)
                {
                    return new Response<bool> { IsSuccess = false, Message = "V�hicule non trouv�." };
                }

                // Supposition que vous souhaitez mettre � jour le statut du v�hicule.
                // Cette approche suppose que Status est un objet complexe li� au v�hicule.
                var status = await _ecoMoveDbContext.Status.FirstOrDefaultAsync(s => s.StatusLabel == statusDto.StatusLabel);
                if (status == null)
                {
                    return new Response<bool> { IsSuccess = false, Message = "�tiquette de statut non trouv�e." };
                }

                vehicle.Status = status; // Assigner le statut r�cup�r� au v�hicule
                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<bool> { IsSuccess = true, Message = "Statut du v�hicule mis � jour avec succ�s." };
            }
            catch (Exception e)
            {
                return new Response<bool> { IsSuccess = false, Message = $"Erreur lors de la mise � jour du statut du v�hicule : {e.Message}" };
            }
        }
    }
}
