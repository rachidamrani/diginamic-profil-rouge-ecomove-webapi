using ecomove_back.DTOs.StatusDTOs;
using ecomove_back.Helpers;


namespace ecomove_back.Interfaces.IRepositories
{
    public interface IStatusRepository
    {
        public Task<Response<StatusDTO>> CreateStatusAsync(StatusDTO StatusDTO);
        public Task<Response<string>> DeleteStatusAsync(int StatusId);
        public Task<Response<List<StatusDTO>>> GetAllStatusAsync();
        public Task<Response<StatusDTO>> GetStatusByIdAsync(int id);
        public Task<Response<StatusDTO>> UpdateStatusAsync(int StatusId, StatusDTO StatusDTO);


    }
}