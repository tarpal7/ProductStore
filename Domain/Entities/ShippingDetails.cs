using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Зазначте Ваше Ім'я")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Вкажіть квартиру")]
        [Display(Name = "Квартира")]
        public string Line1 { get; set; }
        [Required(ErrorMessage = "Вкажіть будинок")]
        [Display(Name = "Будинок")]
        public string Line2 { get; set; }
        [Required(ErrorMessage = "Вкажіть вулицю")]
        [Display(Name = "Вулиця")]
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Вкажіть місто")]
        [Display(Name = "Місто")]
        public string City { get; set; }

        [Required(ErrorMessage = "Вкажіть телефон")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        public bool GiftWrap { get; set; }
    }

}
