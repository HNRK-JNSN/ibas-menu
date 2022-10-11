using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure;
using Azure.Data.Tables;
using IBAS.MenuApp.Model;

namespace IBAS_menu.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public List<MenuItemDTO> MenuItems {get; set;}

    private TableClient _tableClient;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
    {
        _logger = logger;
        MenuItems = new List<MenuItemDTO>();

        var serviceUri = "https://csb10032000f00e7492.table.core.windows.net/";
        var tableName = "IBASKantinen";
        var accountName = "csb10032000f00e7492";
        
        try {
            var storageAccountKey = config["StorageAccountKey"];

            logger.LogInformation($"Storage key is {storageAccountKey}");

            this._tableClient = new TableClient(
                new Uri(serviceUri),
                tableName,
                new TableSharedKeyCredential(accountName, storageAccountKey));
        } catch (Exception ex)
        {
            logger.LogCritical(ex, "Could not connect to storage.");
        }
    }

    public void OnGet()
    {
        Pageable<TableEntity> entities = _tableClient.Query<TableEntity>();

        foreach (TableEntity entity in entities)
        {
            var dto = new MenuItemDTO(entity.RowKey, entity.GetString("warmdish"), entity.GetString("colddish"));
            MenuItems.Add(dto);
        }        
    }

}
