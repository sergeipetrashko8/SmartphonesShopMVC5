using System.Collections.Generic;
using System.Linq;

namespace SmartphoneStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> _lineCollection = new List<CartLine>();

        public void AddItem(Smartphone smartphone, int quantity)
        {
            CartLine line = _lineCollection
                .FirstOrDefault(g => g.Smartphone.SmartphoneId == smartphone.SmartphoneId);

            if (line == null)
            {
                _lineCollection.Add(new CartLine
                {
                    Smartphone = smartphone,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Smartphone smartphone)
        {
            _lineCollection.RemoveAll(l => l.Smartphone.SmartphoneId == smartphone.SmartphoneId);
        }

        public decimal ComputeTotalValue()
        {
            return _lineCollection.Sum(e => e.Smartphone.Price * e.Quantity);

        }
        public void Clear()
        {
            _lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines => _lineCollection;
    }

    public class CartLine
    {
        public Smartphone Smartphone { get; set; }
        public int Quantity { get; set; }
    }
}