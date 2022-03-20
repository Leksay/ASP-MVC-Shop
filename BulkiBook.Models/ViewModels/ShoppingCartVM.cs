namespace BulkiBook.Models.ViewModels;

public class ShoppingCartVM
{
    public IEnumerable<ShoppingCardModel> ListCart { get; set; }
    public OrderHeader OrderHeader { get; set; }
}
