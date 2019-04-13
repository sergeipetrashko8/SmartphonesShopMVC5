using System.Collections.Generic;
using SmartphoneStore.Domain.Entities;

namespace SmartphoneStore.WebUI.Models
{
    public class SmartphonesListViewModel
    {
        public IEnumerable<Smartphone> Smartphones { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentManufacturer { get; set; }
    }
}
