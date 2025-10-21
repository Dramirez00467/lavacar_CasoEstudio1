using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lavacarDAL.Entidades;

namespace lavacarDAL.Repositorios
{
    public class ClientesRepositorio : IClientesRepositorio
    {
        private List<Cliente> clientes = new List<Cliente>()
        {
            new Cliente { Id = 1, Nombre = "Juan", Apellido = "Pérez", Identificacion="123", Edad = 30, Telefono="8888-8888", Correo="juan@correo.com", FechaCreacion = DateTime.Now },
            new Cliente { Id = 2, Nombre = "María", Apellido = "Gómez", Identificacion="456", Edad = 25, Telefono="8999-9999", Correo="maria@correo.com", FechaCreacion = DateTime.Now },
            new Cliente { Id = 3, Nombre = "Carlos", Apellido = "López", Identificacion="789", Edad = 28, Telefono="8777-7777", Correo="carlos@correo.com", FechaCreacion = DateTime.Now }
        };

        public async Task<List<Cliente>> ObtenerClientesAsync()
        {
            return clientes;
        }

        public async Task<Cliente> ObtenerClientePorIdAsync(int id)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            return cliente;
        }

        public async Task<bool> AgregarClienteAsync(Cliente cliente)
        {
            cliente.Id = clientes.Any() ? clientes.Max(c => c.Id) + 1 : 1;
            cliente.FechaCreacion = DateTime.Now;
            clientes.Add(cliente);
            return true;
        }

        public async Task<bool> ActualizarClienteAsync(Cliente cliente)
        {
            var existente = clientes.FirstOrDefault(c => c.Id == cliente.Id);
            if (existente == null) return false;

            existente.Nombre = cliente.Nombre;
            existente.Apellido = cliente.Apellido;
            existente.Identificacion = cliente.Identificacion;
            existente.Edad = cliente.Edad;
            existente.Telefono = cliente.Telefono;
            existente.Correo = cliente.Correo;
            return true;
        }

        public async Task<bool> EliminarClienteAsync(int id)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                clientes.Remove(cliente);
                return true;
            }
            return false;
        }
    }
}