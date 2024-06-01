using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Adventure_MVC.Models;

namespace Adventure_MVC.Controllers
{
    public class ComentariosController : Controller
    {
        private dbAventurasEntities1 dbEntitis = new dbAventurasEntities1();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind(Include = "id,id_foto, id_user, comentario, fechaCreado, titulo")] tbComentarios comentarios)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    comentarios.fechaCreado = DateTime.Now;
                    comentarios.id_user = int.Parse(Request.Cookies["MiCookie"]["idUser"].ToString());

                    dbEntitis.tbComentarios.Add(comentarios);
                    dbEntitis.SaveChanges();

                    return RedirectToAction("Detalles", "Fotos", new { id = comentarios.id_foto });
                }
            }
            catch( Exception e)
            {
                ViewBag.ErrorMessage = e.Message;

                return View("~/Views/Shared/Error.cshtml");
            }
            
            return RedirectToAction("Index", "Fotos");
        }

        public ActionResult Eliminar(int id_comentario)
        {
            try
            {
                tbComentarios comentario = dbEntitis.tbComentarios.FirstOrDefault(c => c.id == id_comentario);
                tbComentarios auxcomentario = dbEntitis.tbComentarios.FirstOrDefault(c => c.id == id_comentario);

                //int id_foto = comentario.id;
                dbEntitis.tbComentarios.Remove(comentario);
                dbEntitis.SaveChanges();

                return RedirectToAction("Detalles", "Fotos", new { id = auxcomentario.id_foto });
            }
            catch( Exception e)
            {
                ViewBag.ErrorMessage = e.Message;

                return View("~/Views/Shared/Error.cshtml");
            }

            return RedirectToAction("Index", "Fotos");
        }
    }
}
