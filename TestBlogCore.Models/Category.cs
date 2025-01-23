using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBlogCore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingrese un nombre para la categoria")]
        [Display(Name = "Nombre de Categoria")]
        public string Name { get; set; }

        [Display(Name = "Orden de visualización")]
        [Range(1,100,ErrorMessage ="El valor debe de estar entre 1 y 100")]
        public int? Order { get; set; }
    }
}
