namespace APBD8.Services;

using Microsoft.EntityFrameworkCore;
using APBD8.Data;
using APBD8.DTOs;
using APBD8.Models;


public class PCService : IPCService
{
    private readonly AppDbContext _context;

    public PCService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PCResponseDto>> GetAllAsync()
    {
        return await _context.PCs
            .Select(pc => new PCResponseDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            })
            .ToListAsync();
    }

    public async Task<List<PCComponentResponseDto>?> GetComponentsByPCIdAsync(int id)
    {
        var pcExists = await _context.PCs.AnyAsync(pc => pc.Id == id);

        if (!pcExists)
            return null;

        return await _context.PCComponents
            .Where(pcComponent => pcComponent.PCId == id)
            .Select(pcComponent => new PCComponentResponseDto
            {
                Amount = pcComponent.Amount,
                Component = new ComponentResponseDto
                {
                    Code = pcComponent.Component.Code,
                    Name = pcComponent.Component.Name,
                    Description = pcComponent.Component.Description,
                    Type = pcComponent.Component.ComponentType.Name,
                    Manufacturer = pcComponent.Component.ComponentManufacturer.FullName
                }
            })
            .ToListAsync();
    }

    public async Task<PCResponseDto> CreateAsync(PCRequestDto dto)
    {
        var pc = new PC
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.PCs.Add(pc);
        await _context.SaveChangesAsync();

        return new PCResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<PCResponseDto?> UpdateAsync(int id, PCRequestDto dto)
    {
        var pc = await _context.PCs.FindAsync(id);

        if (pc is null)
            return null;

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();

        return new PCResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pc = await _context.PCs.FindAsync(id);

        if (pc is null)
            return false;

        _context.PCs.Remove(pc);
        await _context.SaveChangesAsync();

        return true;
    }
}