using Microsoft.AspNetCore.Mvc;
using DeliciaSoft.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DeliciaSoft.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DeliciaSoftContext _context;

        public DashboardController(DeliciaSoftContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Obtener datos de ventas totales
            var ventasTotales = _context.DetalleVenta
                .Sum(dv => dv.Cantidad * dv.PrecioUnitario) ?? 0;

            // Obtener datos de gastos totales (usando tabla de compras)
            var gastosTotales = _context.Compras
                .Sum(c => c.Total) ?? 0;

            // Obtener promedio mensual
            var fechaActual = DateTime.Now;
            var primerDiaMes = new DateOnly(fechaActual.Year, fechaActual.Month, 1);

            // Primero obtener todas las ventas del mes actual y luego filtrar en memoria
            var ventasDelMes = _context.Venta
                .Include(v => v.DetalleVenta)
                .Where(v => v.FechaVenta.HasValue && v.FechaVenta >= primerDiaMes)
                .ToList();

            var ventasMes = ventasDelMes.SelectMany(v => v.DetalleVenta)
                .Sum(dv => dv.Cantidad * dv.PrecioUnitario) ?? 0;

            // Calcular días transcurridos del mes
            var diasTranscurridos = fechaActual.Day;
            var promedioDiario = diasTranscurridos > 0 ? ventasMes / diasTranscurridos : 0;

            // Datos para gráfico de ventas diarias (últimos 7 días)
            var ultimosSieteDias = Enumerable.Range(0, 7)
                .Select(i => DateOnly.FromDateTime(DateTime.Now.AddDays(-i)))
                .Reverse()
                .ToList();

            // Obtener todas las ventas de los últimos 7 días en una sola consulta
            var fechaInicio = ultimosSieteDias.FirstOrDefault();
            var fechaFin = ultimosSieteDias.LastOrDefault();

            var ventasUltimaSemana = _context.Venta
                .Include(v => v.DetalleVenta)
                .Where(v => v.FechaVenta.HasValue && v.FechaVenta >= fechaInicio && v.FechaVenta <= fechaFin)
                .ToList();

            var ventasDiarias = ultimosSieteDias.Select(fecha => {
                var ventasDelDia = ventasUltimaSemana
                    .Where(v => v.FechaVenta == fecha)
                    .ToList();

                var totalDia = ventasDelDia.Sum(v =>
                    v.DetalleVenta.Sum(dv => dv.Cantidad * dv.PrecioUnitario ?? 0));

                return new
                {
                    Fecha = fecha.ToString("dd/MM"),
                    VentasDia = totalDia
                };
            }).ToList();

            // Datos para gráfico de ventas mensuales (últimos 6 meses)
            var ultimosSeisMeses = Enumerable.Range(0, 6)
                .Select(i => {
                    var fecha = DateTime.Now.AddMonths(-i);
                    return new DateOnly(fecha.Year, fecha.Month, 1);
                })
                .Reverse()
                .ToList();

            // Obtener primer y último día del rango de 6 meses
            var primerDiaRango = ultimosSeisMeses.FirstOrDefault();
            var ultimoDiaRango = DateOnly.FromDateTime(DateTime.Now);

            // Obtener todas las ventas de los últimos 6 meses en una sola consulta
            var ventasUltimosMeses = _context.Venta
                .Include(v => v.DetalleVenta)
                .Where(v => v.FechaVenta.HasValue && v.FechaVenta >= primerDiaRango && v.FechaVenta <= ultimoDiaRango)
                .ToList();

            var ventasMensuales = ultimosSeisMeses.Select(primerDia => {
                var ultimoDia = new DateOnly(
                    primerDia.Month == 12 ? primerDia.Year + 1 : primerDia.Year,
                    primerDia.Month == 12 ? 1 : primerDia.Month + 1,
                    1).AddDays(-1);

                var ventasEnRango = ventasUltimosMeses
                    .Where(v => v.FechaVenta >= primerDia && v.FechaVenta <= ultimoDia)
                    .ToList();

                var totalMes = ventasEnRango.Sum(v =>
                    v.DetalleVenta.Sum(dv => dv.Cantidad * dv.PrecioUnitario ?? 0));

                return new
                {
                    Mes = primerDia.ToString("MM/yyyy"),
                    VentasMes = totalMes
                };
            }).ToList();

            // Ventas en tiempo real (últimas 5)
            var ventasRecientes = _context.Venta
                .Include(v => v.DetalleVenta)
                .Include(v => v.IdClienteNavigation)
                .OrderByDescending(v => v.FechaVenta)
                .Take(5)
                .AsEnumerable()  // Mover procesamiento a memoria
                .Select(v => new {
                    v.IdVenta,
                    v.FechaVenta,
                    ClienteNombre = v.IdClienteNavigation != null ?
                        $"{v.IdClienteNavigation.Nombre} {v.IdClienteNavigation.Apellido}" : "Cliente no registrado",
                    Total = v.DetalleVenta.Sum(dv => dv.Cantidad * dv.PrecioUnitario ?? 0)
                })
                .ToList();

            // Pasar todos los datos a la vista
            ViewBag.VentasTotales = ventasTotales;
            ViewBag.GastosTotales = gastosTotales;
            ViewBag.PromedioDiario = promedioDiario;
            ViewBag.VentasDiarias = ventasDiarias;
            ViewBag.VentasMensuales = ventasMensuales;
            ViewBag.VentasRecientes = ventasRecientes;

            return View();
        }
    }
}