using ecomove_back.Data;

namespace ecomove_back.DTOs.VehicleDTOs
{
    public class VehicleForChangeStatusDTO: VehicleDTO
    {
        public StatusEnum StatusLabel { get; set; }
    }
}
