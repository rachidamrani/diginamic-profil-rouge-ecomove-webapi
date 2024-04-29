namespace ecomove_back.DTOs.VehicleDTOs
{
    public class VehicleDTO
    {
        public int CarSeatNumber { get; set; }
        public string Registration { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public int CO2emission { get; set; }
        public double Consumption { get; set; }
        
    }
}
