using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBlogCore.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage ="El nombre es obligatorio")]
        [Display(Name = "Nombre del Articulo")]
        public string Name { get; set; }


        [Required(ErrorMessage = "La descripcion es obligatoria")]
        [Display(Name = "Descripcion del Articulo")]
        public string Description { get; set; }


        [Display(Name = "Fecha de Creación")]
        public string? CreatedAt { get; set; }


        [Display(Name = "Image")]
        [DataType(DataType.ImageUrl)]
        public string? URLImage { get; set; }


        [Required(ErrorMessage = "Seleccione una categoria")]
        public int CategoryId { get; set; }


        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}
