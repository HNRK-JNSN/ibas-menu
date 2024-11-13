using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure;
using Azure.Data.Tables;
using IBAS.MenuApp.Model;

namespace IBAS_menu.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public Pageable<MenuItemDTO> MenuItems {get; set;}

    private TableClient? _tableClient;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
    {
        _logger = logger;
        
        var connectionString = config.GetConnectionString("AzureStorage");

        if (string.IsNullOrEmpty(connectionString))
        {
            logger.LogCritical("Connection string is empty.");
            return;
        }

        try {
            this._tableClient = new TableClient(
                connectionString,
                "MenuTable"
            );
        } catch (Exception ex)
        {
            logger.LogCritical(ex, "Could not connect to storage.");
        }
    }

    public void OnGet()
    {
        if (this._tableClient == null)
        {
            return;
        }
        this.MenuItems = _tableClient.Query<MenuItemDTO>();
    }

}
