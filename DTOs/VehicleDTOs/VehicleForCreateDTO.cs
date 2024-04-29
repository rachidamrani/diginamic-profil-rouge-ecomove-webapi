namespace ecomove_back.DTOs.VehicleDTOs
{
    public class VehicleForCreateDTO: VehicleDTO
    {
        public int CategoryId { get; set; }
        public int MotorizationId { get; set; }
        public int ModelId { get; set; }
    }
}
