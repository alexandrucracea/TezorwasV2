

namespace TezorwasV2.Model
{
    public class ReceiptModel
    {
        public string Id { get; set; }
        public string Name { get; set;}
        public DateTime CreationDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public List<ReceiptItemModel> ReceiptItems { get; set; } = new List<ReceiptItemModel>();
        public Color BackgroundColor { get; set; } = new Color(20,70,67);// Adaugă această proprietate
    }
}
