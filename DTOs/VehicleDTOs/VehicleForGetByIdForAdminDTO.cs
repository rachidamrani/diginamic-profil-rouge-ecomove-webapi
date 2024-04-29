using ecomove_back.Data;

namespace ecomove_back.DTOs.VehicleDTOs
{
    public class VehicleForGetByIdForAdminDTO: VehicleForGetDTO
    {
        public StatusEnum StatusLabel { get; set; }
    }
}
