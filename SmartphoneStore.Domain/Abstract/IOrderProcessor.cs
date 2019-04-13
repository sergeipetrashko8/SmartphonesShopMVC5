using SmartphoneStore.Domain.Entities;

namespace SmartphoneStore.Domain.Abstract
{
    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}