using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure;
using Azure.Data.Tables;
using Azure.Identity;
using IBAS.MenuApp.Model;

namespace IBAS_menu.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public Pageable<MenuItemDTO>? MenuItems {get; set;}

    private TableClient? _tableClient;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
    {
        _logger = logger;
        
        var tableEndpoint = Environment.GetEnvironmentVariable("AZURE_STORAGETABLE_RESOURCEENDPOINT");

        if (string.IsNullOrEmpty(tableEndpoint))
        {
            logger.LogCritical("No Endpoint found.");
            return;
        }

        var credential = new DefaultAzureCredential();

        try {
            var client = new TableServiceClient(
                new Uri(tableEndpoint),
                credential
            );

            this._tableClient = client.GetTableClient("MenuTable");

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
