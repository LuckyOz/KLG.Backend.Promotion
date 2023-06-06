
using KLG.Backend.Promotion.Models.Request;

namespace KLG.Backend.Promotion.Services.Business.Promotion
{
    public interface IPromoManager
    {
        Task<bool> InsertDataDefault();
        Task<ServiceResponse<List<FindPromoResponseDto>>> FindPromo(PromoRequestDto promoRequestDto);
        Task<ServiceResponse<ValidatePromoResponseDto>> ValidatePromo(PromoRequestDto promoRequestDto);
    }
}
