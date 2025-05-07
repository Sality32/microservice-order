using ProductApp.Core.Application.DTOs;

namespace ProductApp.Core.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto> CreateAsync(CreateProductDto createProductDto);
    Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto updateProductDto);
    Task DeleteAsync(Guid id);
}