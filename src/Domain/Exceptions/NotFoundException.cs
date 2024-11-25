using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    // La clase NotFoundException hereda de la clase base Exception.
    // Se utiliza para representar una situación en la que una entidad específica no se encuentra en el sistema.
    public class NotFoundException : Exception
    {
        // Constructor predeterminado que llama al constructor base de la clase Exception sin ningún argumento.
        public NotFoundException()
            : base()
        {
        }

        // Constructor que acepta un mensaje de error y llama al constructor base con ese mensaje.
        public NotFoundException(string message)
            : base(message)
        {
        }

        // Constructor que acepta un mensaje de error y una excepción interna.
        // Llama al constructor base con el mensaje y la excepción interna.
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Constructor que acepta el nombre de la entidad y su clave.
        // Genera un mensaje de error que incluye el nombre de la entidad y su clave.
        public NotFoundException(string name, object key)
            : base($"Entity {name} ({key}) was not found.")
        {
        }
    }
}
