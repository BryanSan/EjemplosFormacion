using System.ComponentModel.DataAnnotations;

namespace EjemplosFormacion.WebApi.Models
{
    /// <summary>
    /// Entidad usada como modelo para los Action que reciban objetos
    /// </summary>
    public class TestModel
    {
        [Required(ErrorMessage = "Nombre requerido!.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Edad requerida!.")]
        public int Edad { get; set; }
    }
}