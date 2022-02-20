using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;



namespace Unillanos.ArquitecturaMS.Usuarios.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        
        [HttpPost]
        [Route(("InsertarUser"))]
        public string InsertarUser(UsuariosDto usuario)
        {
            CRUD crud = new CRUD();
            return crud.Ingresar(usuario);
        }

        [HttpPut]
        [Route(("ActualizarUser"))]
        public string ActualizarUser(UsuariosDto usuario)
        {
            CRUD crud = new CRUD();
            return crud.Actualizar(usuario);
        }

        [HttpGet]
        [Route(("BuscarUser/{usuario}"))]
        public UsuariosDto BuscarUser(string usuario)
        {
            CRUD crud = new CRUD();
            return crud.Leer(usuario);
        }

        [HttpDelete]
        [Route(("EliminarUser/{usuario}"))]
        public string EliminarUser(string usuario)
        {
            CRUD crud = new CRUD();
            return crud.Eliminar(usuario);
        }


    }

    public class UsuariosDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Sexo { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Edad { get; set; }
    }

    public class CRUD
    {
        public string Ingresar(UsuariosDto usuario)
        {
            List<UsuariosDto> users = new List<UsuariosDto>();
            JSONReadAndWrite jsonData = new JSONReadAndWrite();
            users = JsonConvert.DeserializeObject<List<UsuariosDto>>(jsonData.Read());

            if (users == null)
            {
                users = new List<UsuariosDto>();
            }

            Boolean exist = false;
            foreach (UsuariosDto datUser in users)
            {
                if (datUser.Nombre.Equals(usuario.Nombre))
                {
                    exist = true;
                    break;
                }
            }
            if (!exist)
            {
                users.Add(usuario);
                string jsonString = JsonConvert.SerializeObject(users);
                jsonData.Write(jsonString);
                return usuario.Nombre;
            }
            else
            {
                return "El usuario ya existe";
            }           
            
        }

        public string Actualizar(UsuariosDto usuario)
        {
            List<UsuariosDto> users = new List<UsuariosDto>();
            JSONReadAndWrite jsonData = new JSONReadAndWrite();
            users = JsonConvert.DeserializeObject<List<UsuariosDto>>(jsonData.Read());
            Boolean exist = false;
            int cont = 0;
            foreach(UsuariosDto datUser in users)
            {
                if (datUser.Nombre.Equals(usuario.Nombre))
                {
                    exist=true;
                    break;
                }
                cont++;
            }

            if(exist)
            {
                users[cont]=usuario;
                string jsonString = JsonConvert.SerializeObject(users);
                jsonData.Write(jsonString);
                return usuario.Nombre;
            }
            else
            {
                return "El usuario no existe";
            }
           
        }

        public UsuariosDto Leer(string NombreUsuario)
        {
            List<UsuariosDto> users = new List<UsuariosDto>();
            JSONReadAndWrite jsonData = new JSONReadAndWrite();
            users = JsonConvert.DeserializeObject<List<UsuariosDto>>(jsonData.Read());
            foreach (UsuariosDto datUser in users)
            {
                if (datUser.Nombre.Equals(NombreUsuario))
                {
                    return datUser;
                }
            }

            return null;
        }

        public string Eliminar(String NombreUsuario)
        {
            List<UsuariosDto> users = new List<UsuariosDto>();
            JSONReadAndWrite jsonData = new JSONReadAndWrite();
            users = JsonConvert.DeserializeObject<List<UsuariosDto>>(jsonData.Read());
            
            int cont = 0;
            Boolean borrado = false;
            foreach (UsuariosDto datUser in users)
            {
                if (datUser.Nombre.Equals(NombreUsuario))
                {
                    users.RemoveAt(cont);
                    borrado= true;
                    break;
                }
                cont++;
            }

            if (borrado)
            {
                string jsonString = JsonConvert.SerializeObject(users);
                jsonData.Write(jsonString);
                return "Usuario eliminado correctamente";
            }

            return "No se ha encontrado el usuario";
        }
    }

    public class JSONReadAndWrite{
        public JSONReadAndWrite()
        {

        }

        public string Read()
        {
            String jsonResult;
            string path = @"BD\TextFile.json";
            using (StreamReader sr = new StreamReader(path))
            {
                jsonResult = sr.ReadToEnd();
            }
            return jsonResult;
        }

        public void Write(string jsonString)
        {
            string path = @"BD\TextFile.json";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(jsonString);
            }    
        }

    }

}
