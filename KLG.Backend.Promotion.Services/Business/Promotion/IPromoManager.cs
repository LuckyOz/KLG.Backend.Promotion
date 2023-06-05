
using KLG.Backend.Promotion.Models.Request;

namespace KLG.Backend.Promotion.Services.Business.Promotion
{
    public interface IPromoManager
    {
        Task<bool> InsertDataDefault();
        Task<List<FindPromoResponseDto>> FindPromo(PromoRequestDto promoRequestDto);
    }
}
