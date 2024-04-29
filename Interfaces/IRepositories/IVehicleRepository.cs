using ecomove_back.DTOs.VehicleDTOs;
using ecomove_back.Helpers;

namespace ecomove_back.Interfaces.IRepositories
{
    public interface IVehicleRepository
    {
        public Task<Response<VehicleForCreateDTO>> CreateVehicleAsync(VehicleForCreateDTO VehicleForCreationDTO);
        public Task<Response<List<VehicleForGetDTO>>> GetAllVehiclesAsync();
        public Task<Response<VehicleForGetDTO>> GetVehicleByIdAsync(Guid vehicleId);
        public Task<Response<VehicleForGetByIdForAdminDTO>> GetVehicleByIdForAdminAsync(Guid vehicleId);
        public Task<Response<VehicleForUpdateDTO>> UpdateVehicleAsync(Guid vehicleId, VehicleForUpdateDTO vehicleDto);
        public Task<Response<VehicleForChangeStatusDTO>> ChangeVehicleStatusAsync(Guid vehicleId, VehicleForChangeStatusDTO statusDto);
        public Task<Response<bool>> DeleteVehicleAsync(Guid vehicleId);
    }
}