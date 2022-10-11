
namespace IBAS.MenuApp.Model
{
    public class MenuItemDTO
    {
        public string? DOW {get;set;}
        public string? WarmDish {get;set;}
        public string? ColdDish {get;set;}

        public MenuItemDTO(string? dow, string? warmDish, string? coldDish)
        {
            DOW = dow;
            WarmDish = warmDish;
            ColdDish = coldDish;
        }
    }
}