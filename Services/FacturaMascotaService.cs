using Microsoft.EntityFrameworkCore;
using ProyectoFinalAp1.Data;
using ProyectoFinalAp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalAp1.Services
{
    public class FacturaMascotaService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public FacturaMascotaService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<int> CrearFacturaMascota(List<CarritoMascotas> itemsCarrito)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();

            try
            {
                if (itemsCarrito == null || !itemsCarrito.Any())
                {
                    Console.WriteLine("Error: No hay items en el carrito de mascotas");
                    return -1;
                }

                var facturas = new List<FacturaMascota>();

                foreach (var item in itemsCarrito)
                {
                    if (item.Mascota == null)
                    {
                        Console.WriteLine($"Error: Mascota no encontrada para item {item.CarritoMascotaId}");
                        return -1;
                    }

                    var factura = new FacturaMascota
                    {
                        Fecha = DateTime.Now,
                        Total = (decimal)item.Mascota.Precio, // Solo 1 unidad
                        MascotaId = item.MascotaId,
                        CarritoMascotaId = item.CarritoMascotaId,
                        Cantidad = 1, // Siempre 1
                        Precio = item.Mascota.Precio,
                        Subtotal = item.Mascota.Precio,
                        Mascotas = item.Mascota
                    };

                    contexto.FacturaMascotas.Add(factura);
                    facturas.Add(factura);
                }

                var cambios = await contexto.SaveChangesAsync();
                if (cambios < facturas.Count)
                {
                    Console.WriteLine($"Error: Solo se guardaron {cambios} de {facturas.Count} facturas");
                    return -1;
                }

                return facturas.First().FacturaMascotaId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear factura mascota: {ex.Message}");
                return -1;
            }
        }

        public async Task<FacturaMascota> ObtenerFacturaMascota(int id)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.FacturaMascotas
                .Include(f => f.Mascotas)
                .ThenInclude(m => m.Donador)
                .FirstOrDefaultAsync(f => f.FacturaMascotaId == id);
        }

        public async Task<List<FacturaMascota>> ObtenerFacturasMascotasPorCarrito(int carritoMascotaId)
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.FacturaMascotas
                .Include(f => f.Mascotas)
                .ThenInclude(m => m.Donador)
                .Where(f => f.CarritoMascotaId == carritoMascotaId)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }

        public async Task<List<FacturaMascota>> ObtenerTodasFacturasMascotas()
        {
            await using var contexto = await _dbFactory.CreateDbContextAsync();
            return await contexto.FacturaMascotas
                .Include(f => f.Mascotas)
                .ThenInclude(m => m.Donador)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();
        }
    }
}