namespace ecomove_back.DTOs.VehicleDTOs
{
    public class VehicleForGetDTO: VehicleDTO
    {
        public Guid VehicleId { get; set; }
        public string BrandLabel { get; set; } = string.Empty;
        public string ModelLabel { get; set; } = string.Empty;
        public string CategoryLabel { get; set; } = string.Empty;
        public string MotorizationLabel { get; set; } = string.Empty;
    }
}
