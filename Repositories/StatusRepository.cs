using ecomove_back.Data;
using ecomove_back.Data.Models;
using ecomove_back.DTOs.StatusDTOs;
using ecomove_back.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using ecomove_back.Helpers;


namespace ecomove_back.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private EcoMoveDbContext _ecoMoveDbContext;

        public StatusRepository(EcoMoveDbContext ecoMoveDbContext)
        {
            _ecoMoveDbContext = ecoMoveDbContext;
        }

        public async Task<Response<StatusDTO>> CreateStatusAsync(StatusDTO statusDTO)
        {

            Status status = new Status
            {
                StatusLabel = statusDTO.StatusLabel,
            };

            try
            {
                await _ecoMoveDbContext.Status.AddAsync(status);
                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<StatusDTO>
                {
                    Message = $"Le statut {status.StatusLabel} a bien �t� cr��",
                    Data = statusDTO,
                    IsSuccess = true,
                    CodeStatus = 201
                };

            }
            catch (Exception e)
            {
                return new Response<StatusDTO>
                {
                    Message = e.Message,
                    IsSuccess = false
                };
            }


        }

        public async Task<Response<string>> DeleteStatusAsync(int statusId)
        {
            Status? status = await _ecoMoveDbContext
            .Status.FirstOrDefaultAsync(status => status.StatusId == statusId);

            if (status is null)
            {
                return new Response<string>
                {
                    Message = "Le status que vous voulez supprimer n'existe pas.",
                    IsSuccess = false,
                    CodeStatus = 404,
                };
            }

            try
            {
                _ecoMoveDbContext.Status.Remove(status);
                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<string>
                {
                    Message = $"Le status {status.StatusLabel} a été supprimée avec succés.",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new Response<string>
                {
                    Message = e.Message,
                    IsSuccess = false
                };
            }

        }

        public async Task<Response<List<StatusDTO>>> GetAllStatusAsync()
        {
            try
            {
                List<Status> listStatus = await _ecoMoveDbContext.Status.ToListAsync();


                if (listStatus.Count > 0)
                {


                    List<StatusDTO> StatusDTO = new();

                    foreach (var status in listStatus)
                    {
                        StatusDTO.Add(new StatusDTO { StatusLabel = status.StatusLabel });
                    }

                    return new Response<List<StatusDTO>>
                    {
                        IsSuccess = true,
                        Data = StatusDTO,
                        Message = null,
                        CodeStatus = 200,
                    };
                }
                else
                {
                    return new Response<List<StatusDTO>>
                    {
                        IsSuccess = false,
                        Message = "La liste des statuts est vide",
                        CodeStatus = 404
                    };
                }

            }
            catch (Exception e)
            {
                return new Response<List<StatusDTO>>
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }

        }

        public async Task<Response<StatusDTO>> GetStatusByIdAsync(int id)
        {
            try
            {
                Status? status = await _ecoMoveDbContext.Status.FirstOrDefaultAsync(s => s.StatusId == id);

                if (status == null)
                {
                    return new Response<StatusDTO>
                    {
                        Message = "Le statut que vous chercher n'existe pas.",
                        IsSuccess = false,
                        CodeStatus = 404,
                    };
                }

                StatusDTO StatusDTO = new StatusDTO
                {
                    StatusLabel = status.StatusLabel,
                };

                return new Response<StatusDTO>
                {
                    IsSuccess = true,
                    Data = StatusDTO,
                    Message = null,
                    CodeStatus = 200,
                };

            }

            catch (Exception e)
            {
                return new Response<StatusDTO>
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }

        }

        public async Task<Response<StatusDTO>> UpdateStatusAsync(int StatusId, StatusDTO StatusDTO)
        {
            try
            {
                Status? status = await _ecoMoveDbContext.Status.FirstOrDefaultAsync(c => c.StatusId == StatusId);

                if (status is null)
                {
                    return new Response<StatusDTO>
                    {
                        CodeStatus = 404,
                        Message = "Le statut que vous recherchez n'existe pas",
                        IsSuccess = false,
                    };
                }

                status.StatusLabel = StatusDTO.StatusLabel;
                await _ecoMoveDbContext.SaveChangesAsync();

                return new Response<StatusDTO>
                {
                    Message = $"Le statut bien été modifiée",
                    IsSuccess = true,
                    CodeStatus = 201,
                };
            }
            catch (Exception e)
            {
                return new Response<StatusDTO>
                {
                    Message = e.Message,
                    IsSuccess = false,
                    CodeStatus = 500,
                };
            }
        }
    }
}
