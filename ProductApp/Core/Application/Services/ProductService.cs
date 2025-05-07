using ProductApp.Core.Application.DTOs;
using ProductApp.Core.Application.Interfaces;
using ProductApp.Core.Domain.Entities;
using ProductApp.Core.Domain.Interfaces;

namespace ProductApp.Core.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product != null ? MapToDto(product) : throw new KeyNotFoundException("Product not found");
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            Stock = createProductDto.Stock,
            CreatedAt = DateTime.UtcNow
        };

        await _productRepository.CreateAsync(product);
        return MapToDto(product);
    }

    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto updateProductDto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new KeyNotFoundException("Product not found");

        product.Name = updateProductDto.Name ?? product.Name;
        product.Description = updateProductDto.Description ?? product.Description;
        product.Price = updateProductDto.Price ?? product.Price;
        product.Stock = updateProductDto.Stock ?? product.Stock;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product);
        return MapToDto(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productRepository.DeleteAsync(id);
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.CreatedAt,
            product.UpdatedAt
        );
    }
}