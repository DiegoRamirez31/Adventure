using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Adventure_MVC.Models;

namespace Adventure_MVC.Controllers
{
    public class FotosController : Controller
    {
        private dbAventurasEntities dbEntities = new dbAventurasEntities();
        
        // GET: Fotos
        public ActionResult Index()
        {
            var fotos = dbEntities.tbFotos;
            return View(fotos.ToList());
        }

        
        // GET: Fotos/Create
        public ActionResult Create()
        {
            //return RedirectToAction("Create", "Fotos");
            return View("NuevaFoto");
        }


        // GET: Fotos/Details/5
        public ActionResult Detalles(int id)
        {
            var foto = dbEntities.tbFotos.FirstOrDefault(f => f.id == id);
            //ViewBag.listaComentarios = dbEntities.tbFotos.Include(r => r.tbComentarios).Where(f => f.id == id);
            ViewBag.listaComentarios = dbEntities.tbComentarios.Where(f => f.id_foto == id)
                                       .Include(u => u.tbUsuarios);

            if (Request.Cookies["MiCookie"] != null)
            {
                int idUser = int.Parse(Request.Cookies["MiCookie"]["idUser"].ToString());
                ViewBag.favorito = dbEntities.tbFavoritos.FirstOrDefault(f => f.id_foto == id
                                                                && f.id_user == idUser);
            }
            return View(foto);
        }

        public ActionResult AddFavorito(int idFoto)
        {
            try
            {
                tbFavoritos favorito = new tbFavoritos();
                favorito.id_foto = idFoto;
                favorito.id_user = int.Parse(Request.Cookies["MiCookie"]["idUser"]);

                dbEntities.tbFavoritos.Add(favorito);
                dbEntities.SaveChanges();

                return RedirectToAction("Detalles", new { id = idFoto });
            }
            catch( Exception e)
            {
                ViewBag.ErrorMessage = e.Message;

                return View("~/Views/Shared/Error.cshtml");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(tbFotos foto, HttpPostedFileBase pathFoto)
        {
            try
            {
                if (ModelState.IsValid && pathFoto != null && pathFoto.ContentLength > 0)
                {
                    // Generar un nombre único para la imagen
                    string nombreUnico = $"{DateTime.Now:yyyyMMddHHmmssfff}_{Path.GetFileName(pathFoto.FileName)}";

                    // Guardar la imagen en la carpeta "imagenes"
                    string rutaImagen = Path.Combine(Server.MapPath("~/Images"), nombreUnico);
                    pathFoto.SaveAs(rutaImagen);

                    // Guardar la información de la foto en la base de datos
                    foto.pathFoto = nombreUnico; // Guardar el nombre único en la base de datos
                    foto.fechaCreacion = DateTime.Now;
                    foto.fechaModificacion = DateTime.Now;
                    foto.id_user = int.Parse(Request.Cookies["MiCookie"]["idUser"].ToString());

                    // Puedes asignar otros campos de la foto aquí

                    dbEntities.tbFotos.Add(foto);
                    dbEntities.SaveChanges();

                    return RedirectToAction("Index"); // Redireccionar a la página principal de fotos
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                return View("~/Views/Shared/Error.cshtml");
            }

            return View(foto); // Si hay algún error, volver al formulario de carga de fotos
        }


        public ActionResult Eliminar(int idFoto)
        {
            try
            {
                // Buscar la Foto a Eliminar
                tbFotos foto = dbEntities.tbFotos.Find(idFoto);

                // Buscar Todos los Comentarios Correspondientes a la Foto
                var comentarios = dbEntities.tbComentarios.Where(c => c.id_foto == idFoto);

                // Buscar Todos los Favoritos Correspondientes a la Foto
                var favoritos = dbEntities.tbFavoritos.Where(f => f.id_foto == idFoto);

                // Eliminar la foto físicamente de la carpeta
                string rutaImagen = Path.Combine(Server.MapPath("~/Images"), foto.pathFoto);
                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen);
                }

                // Ejecutando La Eliminacion en la Base de Datos
                dbEntities.tbComentarios.RemoveRange(comentarios);  // Elimando Comentarios
                dbEntities.tbFavoritos.RemoveRange(favoritos);  // Elimando Favoritos
                dbEntities.tbFotos.Remove(foto);  // Eliminando la Foto

                dbEntities.SaveChanges(); // Guardanndo Cambios
            }catch ( Exception e)
            {
                ViewBag.ErrorMessage = e.Message;

                return View("~/Views/Shared/Error.cshtml");
            }

            return RedirectToAction("Index", "Fotos");
        }

        public ActionResult Carrusel()
        {
            var fotos = dbEntities.tbFotos;

            return View(fotos.ToList());
        }

        public ActionResult Favoritos()
        {
            //var listaFavoritos = dbEntities.tbFotos.Include()

            int idUser = int.Parse(Request.Cookies["MiCookie"]["idUser"].ToString());
            var listaFavoritos = from f in dbEntities.tbFotos
                                 join fav in dbEntities.tbFavoritos
                                 on f.id equals fav.id_foto
                                 where fav.id_user == idUser
                                 select f;

            return View(listaFavoritos.ToList());
        }
    }
}
