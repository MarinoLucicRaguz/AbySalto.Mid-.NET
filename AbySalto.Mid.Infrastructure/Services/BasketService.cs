using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.Application.Mapper;
using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Polly.Caching;

namespace AbySalto.Mid.Infrastructure.Services
{
    public class BasketService : IBasketService
    {
        private readonly AbysaltoDbContext _context;
        private readonly IProductService _productService;

        public BasketService(AbysaltoDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task<ServiceResponse<BasketDto>> AddAsync(int userId, int productId, int incrementBy = 1, CancellationToken ct = default)
        {
            if (incrementBy <= 0)
            {
                return ServiceResponse<BasketDto>.Fail("Increment must be greater than zero.");
            }

            var item = await _context.Basket.FirstOrDefaultAsync(b => b.UserId == userId && b.ProductId == productId, ct);
            if (item is null)
            {
                item = new Basket { UserId = userId, ProductId = productId, Quantity = incrementBy };
                _context.Basket.Add(item);
            }
            else
            {
                item.Quantity += incrementBy;
                _context.Basket.Update(item);
            }

            await _context.SaveChangesAsync();
            return await GetBasketAsync(userId, ct);
        }

        public async Task<ServiceResponse<bool>> ClearAsync(int userId, CancellationToken ct = default)
        {
            var items = await _context.Basket.Where(b => b.UserId == userId).ToListAsync(ct);
            if (!items.Any())
            {
                return ServiceResponse<bool>.Fail("Basket already empty", 404);
            }

            _context.Basket.RemoveRange(items);
            await _context.SaveChangesAsync(ct);
            return ServiceResponse<bool>.Ok(true, "Product removed from basket");
        }

        public async Task<ServiceResponse<BasketDto>> GetBasketAsync(int userId, CancellationToken ct = default)
        {
            var items = await _context.Basket.Where(b => b.UserId == userId).ToListAsync(ct);
            if (!items.Any())
            {
                return ServiceResponse<BasketDto>.Ok(null, "Basket is empty", 200);
            }

            var dtoItems = new List<BasketItemDto>();
            foreach (var item in items)
            {
                var productResp = await _productService.GetByIdAsync(item.ProductId, ct);
                dtoItems.Add(BasketMapper.ToItemDto(item, productResp.Data));
            }

            var basketDto = BasketMapper.ToDto(dtoItems);
            return ServiceResponse<BasketDto>.Ok(basketDto);
        }

        public async Task<ServiceResponse<BasketDto>> ReduceAsync(int userId, int productId, int decrementBy = 1, CancellationToken ct = default)
        {
            if (decrementBy <= 0)
            {
                return ServiceResponse<BasketDto>.Fail("Increment must be greater than zero.");
            }

            var item = await _context.Basket.FirstOrDefaultAsync(b => b.UserId == userId && b.ProductId == productId, ct);
            if (item == null)
            {
                return ServiceResponse<BasketDto>.Fail("Product not found in basket", 404);
            }

            item.Quantity -= Math.Abs(decrementBy);

            if (item.Quantity <= 0)
            {
                _context.Basket.Remove(item);
                await _context.SaveChangesAsync();
                return ServiceResponse<BasketDto>.Ok(null, "Product removed from basket", 200);
            }

            _context.Basket.Update(item);
            await _context.SaveChangesAsync(ct);

            return await GetBasketAsync(userId, ct);
        }

        public async Task<ServiceResponse<bool>> RemoveAsync(int userId, int productId, CancellationToken ct = default)
        {
            var item = await _context.Basket.FirstOrDefaultAsync(b => b.UserId == userId && b.ProductId == productId, ct);
            if (item == null)
            {
                return ServiceResponse<bool>.Fail("Product not found in basket", 404);
            }

            _context.Basket.Remove(item);
            await _context.SaveChangesAsync(ct);
            return ServiceResponse<bool>.Ok(true, "Product removed from basket");
        }
    }
}
