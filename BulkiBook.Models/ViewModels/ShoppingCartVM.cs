using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkiBook.Models.ViewModels;

public class ShoppingCartVM
{
    public IEnumerable<ShoppingCardModel> ListCart { get; set; }
    public double Total { get; set; }
}
