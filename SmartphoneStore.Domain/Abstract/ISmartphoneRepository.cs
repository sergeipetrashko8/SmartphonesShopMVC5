using System.Collections.Generic;
using SmartphoneStore.Domain.Entities;

namespace SmartphoneStore.Domain.Abstract
{
    public interface ISmartphoneRepository
    {
        IEnumerable<Smartphone> Smartphones { get; }
    }
}