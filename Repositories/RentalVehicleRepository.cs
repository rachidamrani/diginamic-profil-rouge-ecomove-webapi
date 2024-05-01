using ecomove_back.Data.Models;
using ecomove_back.Data;
using ecomove_back.DTOs.RentalVehicleDTO;
using ecomove_back.Helpers;
using ecomove_back.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ecomove_back.Repositories
{
    public class RentalVehicleRepository : IRentalVehicleRepository
    {
        private EcoMoveDbContext _ecoMoveDbContext;

        public RentalVehicleRepository(
            EcoMoveDbContext ecoMoveDbContext
        )
        {
            _ecoMoveDbContext = ecoMoveDbContext;
        }



        // Manque verification sur les reservations futures changer userID dans le controller par le user connecte
        public async Task<Response<string>> CreateRentalVehicleAsync(string userId, Guid vehicleId, RentalVehicleDTO rentalVehicleDTO)
        {
            // V�rification que le vehicule existe bien en BDD
            Vehicle? vehicle = await _ecoMoveDbContext.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == vehicleId);

            if (vehicle == null)
            {
                return new Response<string>
                {
                    Message = "Le v�hicle n'existe pas",
                    IsSuccess = false,
                    CodeStatus = 404
                };
            }

            // � refactorer
            if (rentalVehicleDTO.EndDate < rentalVehicleDTO.StartDate)
            {
                return new Response<string>
                {
                    Message = "La date de fin ne peut pas �tre inf�rieur � la date de d�but",
                    IsSuccess = false,
                    CodeStatus = 400
                };
            } 
            else if (rentalVehicleDTO.EndDate.ToString("d") == DateTime.Now.ToString("d"))
            {
                return new Response<string>
                {
                    Message = "La date minimale d'une r�servation est de 1 jour",
                    IsSuccess = false,
                    CodeStatus = 400
                };
            } 
            else if (rentalVehicleDTO.StartDate.Date < DateTime.Now.Date)
            {
                return new Response<string>
                {
                    Message = "La date de d�but ne peut pas �tre ant�rieur � la date du jour",
                    IsSuccess = false,
                    CodeStatus = 400
                };
            }

            RentalVehicle newRentalVehicle = new RentalVehicle
            {
                StartDate = rentalVehicleDTO.StartDate,
                EndDate = rentalVehicleDTO.EndDate,
                VehicleId = vehicleId,
                AppUserId = userId,
                Confirmed = true
            };

            try
            {
                await _ecoMoveDbContext.RentalVehicles.AddAsync(newRentalVehicle);
                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<string>
                {
                    Message = $"Votre r�servation pour le {rentalVehicleDTO.StartDate} au {rentalVehicleDTO.EndDate} a bien �t� cr�e",
                    IsSuccess = true,
                    CodeStatus = 201
                };
            }
            catch (Exception e)
            {
                return new Response<string>
                {
                    Message = e.Message,
                    IsSuccess = false,
                    CodeStatus = 500
                };
            }
        }

        // Manque verification sur les reservations presentes en BDD et verifier aussi que le user est bien celui qui a fait la reservation
        public async Task<Response<RentalVehicleDTO>> UpdateRentalVehicleAsync(int rentalId, RentalVehicleDTO rentalVehicleDTO)
        {
            try
            {
                RentalVehicle? rentalVehicle = await _ecoMoveDbContext.RentalVehicles.FirstOrDefaultAsync(r => r.RentalVehicleId == rentalId);   

                if (rentalVehicle == null)
                {
                    return new Response<RentalVehicleDTO>
                    {
                        Message = "Aucune location ne correspond � cette ID",
                        IsSuccess = false,
                        CodeStatus = 404
                    };
                }

                // � refactorer
                if (rentalVehicleDTO.EndDate < rentalVehicleDTO.StartDate)
                {
                    return new Response<RentalVehicleDTO>
                    {
                        Message = "La date de fin ne peut pas �tre inf�rieur � la date de d�but",
                        IsSuccess = false,
                        CodeStatus = 400
                    };
                }
                else if (rentalVehicleDTO.EndDate.ToString("d") == DateTime.Now.ToString("d"))
                {
                    return new Response<RentalVehicleDTO>
                    {
                        Message = "La date minimale d'une r�servation est de 1 jour",
                        IsSuccess = false,
                        CodeStatus = 400
                    };
                }
                else if (rentalVehicleDTO.StartDate.Date < DateTime.Now.Date)
                {
                    return new Response<RentalVehicleDTO>
                    {
                        Message = "La date de d�but ne peut pas �tre ant�rieur � la date du jour",
                        IsSuccess = false,
                        CodeStatus = 400
                    };
                }

                rentalVehicle.StartDate = rentalVehicleDTO.StartDate;
                rentalVehicle.EndDate = rentalVehicleDTO.EndDate;
                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<RentalVehicleDTO>
                {
                    Message = "Votre r�servatiopn a bien �t� modifi�e",
                    IsSuccess = true,
                    CodeStatus = 201
                };

            }
            catch (Exception e)
            {
                return new Response<RentalVehicleDTO>
                {
                    Message = e.Message,
                    IsSuccess = false,
                    CodeStatus = 500
                };
            }
        }


        // Verifier �galement que la r�servation n'a pas de r�servation de covoiturage
        public async Task<Response<string>> CancelRentalVehicleAsync(int rentalId)
        {
            try
            {
                RentalVehicle? rentalVehicle = await _ecoMoveDbContext.RentalVehicles.FirstOrDefaultAsync(r => r.RentalVehicleId == rentalId);

                if (rentalVehicle == null)
                {
                    return new Response<string>
                    {
                        Message = "Aucune location ne correspond � cette ID",
                        IsSuccess = false,
                        CodeStatus = 404
                    };
                }

                rentalVehicle.Confirmed = false;

                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<string>
                {
                    Message = "Votre r�servatiopn a bien �t� annul�e",
                    IsSuccess = true,
                    CodeStatus = 200
                };
            }
            catch (Exception e)
            {
                return new Response<string>
                {
                    Message = e.Message,
                    IsSuccess = false,
                    CodeStatus = 500
                };
            }
        }


        // Ajout plus de v�rification
        public async Task<Response<List<AllRentalVehicles>>> GetAllRentalVehiclesAysnc()
        {
            try
            {
                List<RentalVehicle> rentalVehicles = await _ecoMoveDbContext.RentalVehicles.ToListAsync();

                if (rentalVehicles.Count == 0)
                {
                    return new Response<List<AllRentalVehicles>>
                    {
                        Message = "Aucune r�servation trouv�e",
                        IsSuccess = false,
                        CodeStatus = 404
                    };
                }

                List<AllRentalVehicles> rentalvehiclesDTO = new List<AllRentalVehicles>();

                foreach (RentalVehicle rentalVehicle in rentalVehicles)
                {
                    rentalvehiclesDTO.Add(new AllRentalVehicles
                    {
                        RentalVehicleId = rentalVehicle.RentalVehicleId,
                        StartDate = rentalVehicle.StartDate,
                        EndDate = rentalVehicle.EndDate,
                    });
                }

                return new Response<List<AllRentalVehicles>>
                {
                    Data = rentalvehiclesDTO,
                    IsSuccess = true,
                    CodeStatus = 200
                };
            }
            catch (Exception e)
            {
                return new Response<List<AllRentalVehicles>>
                {
                    Message = e.Message,
                    IsSuccess = false,
                    CodeStatus = 500
                };
            }
        }


        // Ajouter plus d'�l�ments � renvoyer
        public async Task<Response<SingleRentalVehicleDTO>> GetRentalVehicleByIdAysnc(int rentalId)
        {
            try
            {
                RentalVehicle? rentalVehicle = await _ecoMoveDbContext.RentalVehicles.FirstOrDefaultAsync(r => r.RentalVehicleId == rentalId);

                if (rentalVehicle == null)
                {
                    return new Response<SingleRentalVehicleDTO>
                    {
                        Message = "La r�servation que vous voulez n'existe pas",
                        IsSuccess = false,
                        CodeStatus = 404
                    };
                }

                SingleRentalVehicleDTO rentalVehicleDTO = new SingleRentalVehicleDTO
                {
                    RentalVehicleId = rentalVehicle.RentalVehicleId,
                    StartDate = rentalVehicle.StartDate,
                    EndDate = rentalVehicle.EndDate,
                };

                return new Response<SingleRentalVehicleDTO>
                {
                    Data = rentalVehicleDTO,
                    IsSuccess = true,
                    CodeStatus = 200
                };
            }
            catch (Exception e)
            {
                return new Response<SingleRentalVehicleDTO>
                {
                    Message = e.Message,
                    IsSuccess = false,
                    CodeStatus = 500
                };
            }


        }




    }
}