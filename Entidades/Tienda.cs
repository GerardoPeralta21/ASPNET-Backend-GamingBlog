using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebApiGames.Entidades
{
    public class Tienda : IValidatableObject
    {
        public int Id { get; set; }
        public string Nombre { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            // Validación 1: Nombre debe comenzar con mayúscula
            if (!string.IsNullOrEmpty(Nombre) && !char.IsUpper(Nombre[0]))
            {
                errors.Add(new ValidationResult("El nombre debe comenzar con mayúscula.", new[] { "Nombre" }));
            }

            // Validación 2: Nombre debe contener solo letras
            if (!string.IsNullOrEmpty(Nombre) && !Regex.IsMatch(Nombre, "^[a-zA-Z ]+$"))
            {
                errors.Add(new ValidationResult("El nombre debe contener solo letras y espacios.", new[] { "Nombre" }));
            }

            return errors;
            //yield break;
        }
    }
}
