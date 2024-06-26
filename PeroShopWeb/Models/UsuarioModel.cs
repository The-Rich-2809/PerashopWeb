﻿namespace PeroShopWeb.Models
{
    public class UsuarioModel
    {
        public readonly PerashopDB _contextDB;

        public UsuarioModel(PerashopDB contextDB)
        {
            _contextDB = contextDB;
        }

        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Contrasena2 { get; set; }
        public static string Mensaje { get; set; }
        public static string Nombre { get; set; }
        public static string TipoUsuario { get; set; }
        public static string DireccionImagen { get; set; }
        public string Telefono { get; set; }

        public bool Login()
        {
            List<Usuario> ListaUsuarios = _contextDB.Usuario.ToList();

            foreach (var user in ListaUsuarios)
            {
                if (Correo == user.Correo)
                {
                    if (Contrasena == user.Contrasena)
                    {
                        TipoUsuario = user.TipoUsuario;

                        DireccionImagen = user.DireccionImagen;

                        Nombre = user.Nombre;

                        return true;
                    }
                }
            }

            return false;
        }

        public bool Register()
        {
            if (Contrasena == Contrasena2)
            {
                int i = 0;

                List<Usuario> ListaUsuarios = _contextDB.Usuario.ToList();

                foreach (var user in ListaUsuarios)
                {
                    if (user.Correo == Correo)
                        break;
                    else
                        i++;
                }
                if (i == ListaUsuarios.Count)
                {
                    var u = new Usuario[]
                        {
                            new Usuario() {Correo = Correo, Contrasena = Contrasena, TipoUsuario = "Cliente", Nombre = Nombre, DireccionImagen = "../Images/Usuarios/Usuario.png", NumeroTelefono = Telefono}
                        };

                    foreach (var us in u)
                    {
                        _contextDB.Usuario.Add(us); 
                    }
                    _contextDB.SaveChanges();

                    return true;
                }
                else
                {
                    Mensaje = "El correo ya esta registrado";
                    return false;
                }
            }
            else
            {
                Mensaje = "La contraseña no coincide";
                return false;
            }
        }

        //public bool EditarUsuario()
        //{
            
        //}
    }
}
