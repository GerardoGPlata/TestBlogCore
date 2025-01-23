using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBlogCore.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Slider> slidersList { get; set; }
        public IEnumerable<Article> articlesList { get; set; }
    }
}
