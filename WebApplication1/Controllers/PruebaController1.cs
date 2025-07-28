using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using WebApplication1.Models;
using WebApplication1.Utilidades;

namespace WebApplication1.Controllers
{
    public class PruebaController : Controller
    {
        private readonly Db1Context _context;

        public PruebaController(Db1Context context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }


        public async Task<IActionResult> Index()
        {
            SHA256 mySHA256 = SHA256.Create();
            byte[] datos = new byte[] { 83, 65, 80, 83 }; // SAPS infoLogin.Password
            byte[] hashValue = mySHA256.ComputeHash(datos);
            ViewBag.Hash = hashValue;
            ViewBag.T = BitConverter.ToString(hashValue).Replace("-", "").ToLowerInvariant();

            int[] a = new int[] { 10, 20, 30, 40 };
            ViewBag.Comentario1 = "Comentario 1";
            ViewBag.a1 = a;

            List<string> listaNombres = new List<string> { "Juan Marcos Alas", "María Josefina Galdamez", "Pedro Angel Alas" };
            ViewBag.Nombres = listaNombres;

            List<string> listaSO = new List<string>
            {
                "Windows 11",
                "macOS Sonoma",
                "Ubuntu 24.04 LTS",
                "Fedora 40",
                "Linux Mint 21.3",
                "Debian 12",
                "Arch Linux",
                "Zorin OS 17",
                "Kali Linux",
                "Chrome OS"
            };
            ViewBag.SistemasOperativos = listaSO;
            return View(await _context.Productos.ToListAsync());
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        public async Task<IActionResult> Targeta(int? id)
        {
            var prod1 = await _context.Productos.FindAsync(id);
            if (prod1 == null)
            {
                return NotFound();
            }
            return View(prod1);

        }

        public IActionResult Colores(int? id)
        {
            string[] colores = { "Rojo", "Azul", "Verde", "Amarillo", "Morado" };
            string colorSeleccionado = null;

            if (id.HasValue && id >= 0 && id < colores.Length)
            {
                colorSeleccionado = colores[id.Value];
            }

            ViewBag.ColorSeleccionado = colorSeleccionado;
            ViewBag.Colores = colores;
            return View();
        }


        [Authorize(Roles ="Administradores, Estandar")]
        public async Task<IActionResult> ListaPrecioMedio(bool mayores)
        {
            if (_context.Productos == null)
            {
                return NotFound();
            }
            var precio_medio = await _context.Productos.AverageAsync(a => a.Precio);
            var productos = (mayores) ? await _context.Productos.Where(a => a.Precio > precio_medio).ToListAsync() : await _context.Productos.Where(a => a.Precio < precio_medio).ToListAsync();
            ViewBag.Promedio = precio_medio;
            ViewBag.Titulo = (mayores) ? "mayores" : "menores";
            return View(productos);
        }


        [HttpGet(Name = "GeneratePdf")]
        public IResult GeneratePdf()
        {

            var roles = _context.Roles.Where(a => a.Id > 0);
            var data = new List<Role>();
            foreach (var item in roles)
            {
                data.Add(new Role
                {
                    Id = item.Id,
                    Nombre = item.Nombre
                });
            }

            var documento = new TableUsuario(data);
            var pdfBytes = documento.GeneratePdf();

            return Results.File(pdfBytes, "application/pdf", "lista_roles.pdf");
        }



    }

}