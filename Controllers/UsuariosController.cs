using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class UsuariosController : Controller
    {
        private dbAventurasEntities1 dbEntities = new dbAventurasEntities1();

        // GET: Usuarios
        public ActionResult Index()
        {
            // Sino ha Iniciado Session y No es Administrador No tiene Permiso de Acceder
            if (Request.Cookies["MiCookie"] == null || Request.Cookies["MiCookie"]["idRol"] != "1")
            {
                ViewBag.ErrorMessage = "No tiene Permitido el Acceso";

                return View("~/Views/Shared/Error.cshtml");
            }
                         
            var usuarios = dbEntities.tbUsuarios.Include(r => r.tbRoles);
            return View(usuarios.ToList());
        }

        
        // GET: Usuarios/Create
        public ActionResult Create()
        {
            ViewBag.listaRoles = new SelectList(dbEntities.tbRoles, "id", "descripcion");
            return View("NuevoUsuario");
        }

        public ActionResult Save([Bind(Include = "id, username,password,id_rol, estado")] tbUsuarios usuario)
        {
            tbUsuarios auxUser;
            try
            {
                if (ModelState.IsValid)
                {
                    /*-- Si el nuevo usuario se encripta contraseña, sino se Omite ya ques Update --*/
                    /*-- Es usuario Nuevo --*/
                    if (usuario.id == 0)
                    {
                         auxUser = dbEntities.tbUsuarios.Where(u => u.username == usuario.username).FirstOrDefault();

                        if ( auxUser != null)
                        {
                            /*-- Usuario ya Existe --*/

                            ViewBag.error = "Usuario: " + auxUser.username + " Ya Existe";
                            ViewBag.listaRoles = new SelectList(dbEntities.tbRoles, "id", "descripcion");
                            return View("NuevoUsuario");
                        }

                        /*--- Encriptar Password ---*/
                        usuario.password = GetSHA256(usuario.password);

                        if (usuario.id_rol != 1)
                        {
                            usuario.estado = true;
                            usuario.id_rol = 2;
                        }

                        // Para Agregar Nuevo Usuario
                        dbEntities.tbUsuarios.Add(usuario);
                    }/*-- Es Update --*/
                    else
                    {
                        auxUser = dbEntities.tbUsuarios.Find(usuario.id);

                        auxUser.id_rol = usuario.id_rol;
                        auxUser.estado = usuario.estado;
                        auxUser.username = usuario.username;
                    }

                    dbEntities.SaveChanges();
                }
            } catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;

                return View("~/Views/Shared/Error.cshtml");
            }

            if (Request.Cookies["MiCookie"] == null)  // No Registrado, esta Creando Usuario
                return RedirectToAction("Login");
            else
                return RedirectToAction("Index");
        }

        public ActionResult Login([Bind(Include = "username, password")] tbUsuarios usuarios)
        {

            if (usuarios.username == null)
            {
                return View("Login");
            }
            else
            {
                string passTemp = GetSHA256(usuarios.password);
                var usuarioTemp = dbEntities.tbUsuarios.Where(u => u.username == usuarios.username && u.password == passTemp
                                                               && u.estado == true).FirstOrDefault();

                if (usuarioTemp != null)
                {
                    CrearCookie(usuarioTemp.username, usuarioTemp.id, usuarioTemp.id_rol);

                    // Si es Administrador
                    if ( usuarioTemp.id_rol == 1 )
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Datos Incorrectos, Oh Usuario Deshabilidato";

                    return View("Login");
                }

            }

        }

        // Crear Cookie
        public void CrearCookie(string usuario, int idUser, int idRol)
        {
            /*HttpCookie usuarioCookie = new HttpCookie("usuario", usuario);
            usuarioCookie.Expires = new DateTime(2024, 5, 15);
            Response.Cookies.Add(usuarioCookie);

            HttpCookie idUserCookie = new HttpCookie("idUSer", idUser.ToString());
            idUserCookie.Expires = new DateTime(2024, 5, 15);
            Response.Cookies.Add(idUserCookie);

            HttpCookie idRolCookie = new HttpCookie("idRol", idRol.ToString());
            idRolCookie.Expires = new DateTime(2024, 5, 15);
            Response.Cookies.Add(idRolCookie);*/

            HttpCookie cookie = new HttpCookie("MiCookie");
            cookie["usuario"] = usuario;
            cookie["idRol"] = idRol.ToString();
            cookie["idUser"] = idUser.ToString();

            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie);
        }

        //Eliminar Cookies
        public ActionResult Logout()
        {
            /*if (Request.Cookies["usuario"] != null)
            {
                var usuario = Request.Cookies["usuario"];
                usuario.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(usuario);

                var idUser = Request.Cookies["idUser"];
                idUser.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(idUser);

                var idRol = Request.Cookies["idRol"];
                idRol.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(idRol);
            }*/

            HttpCookie cookie = Request.Cookies["MiCookie"];

            if ( cookie != null)
            {
                // Elimina la cookie estableciendo su fecha de expiración en el pasado
                cookie.Expires = DateTime.Now.AddDays(-1);
                // Agrega la cookie modificada a la colección de cookies de la respuesta para que se envíe al cliente
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("index", "Home");
        }

        public static string EncodePassword(string originalPassword)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] inputBytes = (new UnicodeEncoding()).GetBytes(originalPassword);
            byte[] hash = sha1.ComputeHash(inputBytes);

            return Convert.ToBase64String(hash);
        }

        // Encriptar Password
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;

            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));

            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}", stream[i]);
            }

            return sb.ToString();
        }

        
        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            if (int.Parse(Request.Cookies["MiCookie"]["idRol"].ToString()) == 1)
            {
                try
                {
                    tbUsuarios user = dbEntities.tbUsuarios.Include(r => r.tbRoles).Where(u => u.id == id).FirstOrDefault();
                    ViewBag.Roles = new SelectList(dbEntities.tbRoles, "id", "descripcion");

                    if (user == null)
                    {
                        ViewBag.ErrorMessage = "Usuario no Existe, Haga el Proceso Correcto";

                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                        return View("EditarUsuario", user);
                }
                catch (NullReferenceException ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View("~/Views/Shared/Error.cshtml");
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMessage = e.Message;
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "No tiene Permitido el Acceso";

                return View("~/Views/Shared/Error.cshtml");
            }

        }
    }
}
