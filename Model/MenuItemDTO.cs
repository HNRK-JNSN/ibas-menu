
using Azure;
using Azure.Data.Tables;

namespace IBAS.MenuApp.Model
{
    public class MenuItemDTO : ITableEntity
    {
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        ETag ITableEntity.ETag { get; set; }

        public string? DOW {get;set;}
        public string? WarmDish {get;set;}
        public string? ColdDish {get;set;}

        public MenuItemDTO()
        {
        }
    }

}