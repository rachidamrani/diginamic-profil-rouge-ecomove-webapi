using ecomove_back.Data;
using ecomove_back.Data.Models;
using ecomove_back.DTOs.AdressDTOs;
using ecomove_back.DTOs.CarpoolAddressDTOs;
using ecomove_back.Helpers;
using ecomove_back.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ecomove_back.Repositories
{
    public class CarpoolAddressRepository : ICarpoolAddressRepository
    {
        private readonly EcoMoveDbContext _ecoMoveDbContext;
        private readonly OpenStreetMapHttpRequest _openStreetMapRequest;

        public CarpoolAddressRepository(
            EcoMoveDbContext ecoMoveDbContext,
            OpenStreetMapHttpRequest openStreetMapRequest)
        {
            _ecoMoveDbContext = ecoMoveDbContext;
            _openStreetMapRequest = openStreetMapRequest;
        }

        public async Task<Response<CarpoolAddressOutGoingDTO>> CreateCarpoolAddressAsync(CarpoolAddressDTO carpoolAddressDTO)
        {
            try
            {
                HttpResponseMessage? response = await _openStreetMapRequest.GetAdress(carpoolAddressDTO);

                List<AddressJsonModel>? content = await response!.Content.ReadFromJsonAsync<List<AddressJsonModel>>();

                AddressJsonModel? address = content!.Count > 0 ? content[0] : null;

                if (address is null)
                    return new Response<CarpoolAddressOutGoingDTO>
                    {
                        Message = "Aucune addresse n'a été trouvée",
                        IsSuccess = false,
                        Data = null,
                        CodeStatus = 500,
                    };

                CarpoolAddress carpoolAddress = new()
                {
                    CarpoolAddressId = Guid.NewGuid(),
                    Address = address!.display_name,
                    Latitude = address.lat,
                    Longitude = address.lon
                };

                _ecoMoveDbContext.CarpoolAddresses.Add(carpoolAddress);
                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<CarpoolAddressOutGoingDTO>
                {
                    Message = "L'adresse a bien été créée",
                    Data = new CarpoolAddressOutGoingDTO { Address = carpoolAddress.Address, Latitude = carpoolAddress.Latitude, Longitude = carpoolAddress.Longitude },
                    IsSuccess = true,
                    CodeStatus = 201,
                };
            }
            catch (Exception ex)
            {
                return new Response<CarpoolAddressOutGoingDTO>
                {
                    Message = "Aucune addresse n'a été trouvée",
                    Data = null,
                    IsSuccess = false,
                    CodeStatus = 500,
                };
            }
        }

        public async Task<Response<CarpoolAddressDTO>> DeleteCarpoolAddressAsync(Guid id)
        {
            try
            {
                CarpoolAddress? carpoolAddress = await _ecoMoveDbContext.CarpoolAddresses.FirstOrDefaultAsync(c => c.CarpoolAddressId == id);

                // check if the carpoolAddress is not in a carpooling before deleting / updating it !!!!

                if (carpoolAddress is null)
                    return new Response<CarpoolAddressDTO>
                    {
                        IsSuccess = false,
                        CodeStatus = 404,
                        Message = "Aucune adresse n'a été trouvée"

                    };

                _ecoMoveDbContext.CarpoolAddresses.Remove(carpoolAddress);

                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<CarpoolAddressDTO>
                {
                    IsSuccess = true,
                    CodeStatus = 200,
                    Message = "L'addresse est supprimée avec succès"
                };
            }
            catch (Exception ex)
            {
                return new Response<CarpoolAddressDTO>
                {
                    IsSuccess = false,
                    CodeStatus = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<List<CarpoolAddressOutGoingDTO>>> GetAllCarpoolAddressesAsync()
        {
            try
            {
                List<CarpoolAddress> carpoolAddresses = await _ecoMoveDbContext.CarpoolAddresses.ToListAsync();

                if (carpoolAddresses.Count > 0)
                {
                    List<CarpoolAddressOutGoingDTO> carpoolAddressOutGoingDTOs = new();

                    foreach (CarpoolAddress carpoolAddress in carpoolAddresses)
                    {
                        carpoolAddressOutGoingDTOs.Add(new CarpoolAddressOutGoingDTO
                        {
                            Address = carpoolAddress.Address,
                            Longitude = carpoolAddress.Longitude,
                            Latitude = carpoolAddress.Latitude,
                        });
                    }

                    return new Response<List<CarpoolAddressOutGoingDTO>>
                    {
                        Data = carpoolAddressOutGoingDTOs,
                        IsSuccess = true,
                        CodeStatus = 200,
                        Message = null,
                    };
                }

                return new Response<List<CarpoolAddressOutGoingDTO>>
                {
                    Data = null,
                    IsSuccess = false,
                    CodeStatus = 404,
                    Message = "Aucune addresse n'a été trouvée",
                };

            }
            catch (Exception ex)
            {
                return new Response<List<CarpoolAddressOutGoingDTO>>
                {
                    Data = null,
                    IsSuccess = false,
                    CodeStatus = 404,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<CarpoolAddressOutGoingDTO>> GetCarpoolAddressByIdAsync(Guid id)
        {
            try
            {
                CarpoolAddress? carpoolAddress = await _ecoMoveDbContext.CarpoolAddresses.FindAsync(id);

                if (carpoolAddress is null)
                    return new Response<CarpoolAddressOutGoingDTO>
                    {
                        Data = null,
                        IsSuccess = false,
                        CodeStatus = 404,
                        Message = "Aucune addresse n'a été trouvée"
                    };

                return new Response<CarpoolAddressOutGoingDTO>
                {
                    Data = new CarpoolAddressOutGoingDTO { Address = carpoolAddress.Address, Latitude = carpoolAddress.Latitude, Longitude = carpoolAddress.Longitude },
                    IsSuccess = true,
                    CodeStatus = 200,
                    Message = null
                };
            }
            catch (Exception ex)
            {
                return new Response<CarpoolAddressOutGoingDTO>
                {
                    Data = null,
                    IsSuccess = false,
                    CodeStatus = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<CarpoolAddressOutGoingDTO>> UpdateCarpoolAddressAsync(Guid id, CarpoolAddressDTO carpoolAddressDTO)
        {
            try
            {
                CarpoolAddress? carpoolAddress = await _ecoMoveDbContext.CarpoolAddresses.FindAsync(id);

                if (carpoolAddress is null)
                    return new Response<CarpoolAddressOutGoingDTO>
                    {
                        Data = null,
                        IsSuccess = false,
                        CodeStatus = 404,
                        Message = "Aucune addresse n'a été trouvée"
                    };

                HttpResponseMessage? response = await _openStreetMapRequest.GetAdress(carpoolAddressDTO);

                List<AddressJsonModel>? content = await response!.Content.ReadFromJsonAsync<List<AddressJsonModel>>();

                AddressJsonModel? address = content!.Count > 0 ? content[0] : null;

                if (address is null)
                    return new Response<CarpoolAddressOutGoingDTO>
                    {
                        Message = "Aucune addresse n'a été trouvée",
                        IsSuccess = false,
                        Data = null,
                        CodeStatus = 500,
                    };

                // Better to verify is the address is not used by a carpool / rental vehicle before allowing update

                carpoolAddress.Address = address!.display_name;
                carpoolAddress.Latitude = address.lat;
                carpoolAddress.Longitude = address.lon;

                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<CarpoolAddressOutGoingDTO>
                {
                    Message = "L'adresse a bien été mise à jour",
                    Data = new CarpoolAddressOutGoingDTO
                    {
                        Address = carpoolAddress.Address,
                        Latitude = carpoolAddress.Latitude,
                        Longitude = carpoolAddress.Longitude
                    },
                    IsSuccess = true,
                    CodeStatus = 201,
                };
            }
            catch (Exception ex)
            {
                return new Response<CarpoolAddressOutGoingDTO>
                {
                    Data = null,
                    Message = ex.Message,
                    IsSuccess = false,
                    CodeStatus = 500,
                };
            }
        }
    }
}