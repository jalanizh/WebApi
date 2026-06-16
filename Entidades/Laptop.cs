using System.ComponentModel.DataAnnotations;

namespace WebAPI.Entidades
{
    public class Laptop
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="el campo {0} es requerido")]
        public required string Nombre { get; set; }
    }
}
