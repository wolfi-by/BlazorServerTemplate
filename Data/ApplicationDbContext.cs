﻿using System.Reflection;
using Ardalis.Result;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorServerTemplate.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<QuantityDto> Units { get; set; }
    public DbSet<QuantityMappingDto> UnitMappings { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appname = Assembly.GetExecutingAssembly().GetName().Name ?? nameof(ApplicationDbContext);
        if (!Path.Exists(Path.Combine(appdata, appname)))
        {
            Directory.CreateDirectory(Path.Combine(appdata, appname));
        }
        string dbPath = Path.Combine(appdata, appname, "app.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath};Cache=Shared");

        base.OnConfiguring(optionsBuilder);
    }

    public async Task<Result> AddQuantityMappingAsync(QuantityMappingRecord dto)
    {

        var record = QuantityMappingDto.FromRecord(dto);

        await UnitMappings.AddAsync(record);
        await SaveChangesAsync();
        return Result.Success();
    }
    public async Task<Result<QuantityMappingRecord[]>> GetQuantityMappingsAsync()
    {
        return Result<QuantityMappingRecord[]>.Success(await UnitMappings.Select(x => x.ToRecord(x)).ToArrayAsync());
    }
    public async Task<Result> RemoveQuantityMappingsAsync(Guid id)
    {
        var item=await UnitMappings.FindAsync(id);
        if (item is null)
        {
            return Result.NotFound("Item not found");
        }
        UnitMappings.Remove(item);
        await SaveChangesAsync();
        return Result.Success();
    }
}
