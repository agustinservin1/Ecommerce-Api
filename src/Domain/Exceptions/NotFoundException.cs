namespace Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        // Constructor por defecto
        public NotFoundException()
            : base()
        {
        }

        // Constructor con mensaje personalizado
        public NotFoundException(string message)
            : base(message)
        {
        }

        // Constructor con mensaje y excepción interna
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Constructor con el nombre de la entidad y la clave
        public NotFoundException(string name, object key)
            : base($"Entity {name} ({key}) was not found.")
        {
        }
    }
}
