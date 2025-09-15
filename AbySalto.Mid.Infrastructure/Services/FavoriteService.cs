using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Mid.Infrastructure.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly AbysaltoDbContext _context;
        private readonly IProductService _productService;

        public FavoriteService(AbysaltoDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task<ServiceResponse<FavoriteDto>> AddAsync(int userId, int productId, CancellationToken ct = default)
        {
            if (await _context.Favorites.AnyAsync(f => f.UserId == userId && f.ProductId == productId, ct))
            {
                return ServiceResponse<FavoriteDto>.Fail("Product is already in favorites.");
            }

            var favorite = new Favorite { UserId = userId, ProductId = productId };
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync(ct);

            ServiceResponse<ProductDto> product = await _productService.GetByIdAsync(productId, ct);
            var dto = new FavoriteDto(favorite.Id, productId, product.Data);

            return ServiceResponse<FavoriteDto>.Ok(dto, "Added to favorites", 201);
        }

        public async Task<ServiceResponse<List<FavoriteDto>>> GetAllByUserAsync(int userId, CancellationToken ct = default)
        {
            var favorites = await _context.Favorites.Where(f => f.UserId == userId).ToListAsync();

            var result = new List<FavoriteDto>();
            foreach (var fav in favorites)
            {
                var productResp = await _productService.GetByIdAsync(fav.ProductId, ct);
                result.Add(new FavoriteDto(fav.Id, fav.ProductId, productResp.Data));
            }

            return ServiceResponse<List<FavoriteDto>>.Ok(result, "Favorites retrieved.", 200);
        }

        public async Task<ServiceResponse<bool>> RemoveAsync(int userId, int productId, CancellationToken ct = default)
        {
            var fav = await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId, ct);
            if (fav == null)
            {
                return ServiceResponse<bool>.Fail("Favorite doesnt exist.", 404);
            }

            _context.Favorites.Remove(fav);
            await _context.SaveChangesAsync(ct);

            return ServiceResponse<bool>.Ok(true, "Succesffuly removed from favorites.");
        }
    }
}
