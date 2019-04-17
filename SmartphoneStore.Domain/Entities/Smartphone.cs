using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SmartphoneStore.Domain.Entities
{
    public class Smartphone
    {
        [HiddenInput(DisplayValue = false)]
        public int SmartphoneId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название смартфона")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание для смартфона")]
        public string Description { get; set; }

        [Display(Name = "Производитель")]
        [Required(ErrorMessage = "Пожалуйста, укажите производителя смартфона")]
        public string Manufacturer { get; set; }

        [Display(Name = "Цена (руб)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}