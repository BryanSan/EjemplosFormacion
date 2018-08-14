using System.ComponentModel.DataAnnotations;

namespace EjemplosFormacion.WebApi.Stubs.Models
{
    /// <summary>
    /// Entidad usada como modelo para los Action que reciban objetos
    /// </summary>
    public class TestModel
    {
        [Required(ErrorMessage = "Nombre requerido!.")]
        public string Nombre { get; set; }

        // Si no es nulo el Data Annotation de Required no reportara los errores de ModelState, esto pasa con las entidades que por default no son nullable, pasar de no nullable a nullable int -> int?
        [Required(ErrorMessage = "Edad requerida!.")]
        public int? Edad { get; set; } 
    }
}