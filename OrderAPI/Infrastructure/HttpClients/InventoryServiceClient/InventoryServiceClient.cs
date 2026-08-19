using BuildingBlocks.Contracts.Inventory;
using BuildingBlocks.Errors;
using BuildingBlocks.Exceptions;
using OrderAPI.Infrastructure.HttpClients.InventoryServiceClient.Models;

namespace OrderAPI.Infrastructure.HttpClients.InventoryServiceClient;

public sealed class InventoryServiceClient(
    HttpClient client,
    ILogger<InventoryServiceClient> logger
)
    : IInventoryServiceClient
{
    public async Task<ReserveStocksResponse> ReserveStocksForVariants(
        Guid orderId,
        ICollection<ReservationItem> reserveStockRequets,
        CancellationToken cancellation = default
    )
    {
        var request = new ReserveStocksRequest(orderId, reserveStockRequets);
        var response = await client.PostAsJsonAsync("/inventories/reservations", request, cancellation);

        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ServiceProblemDetails>(cancellation);

            if (problem is null)
            {
                logger.LogWarning(
                    "Inventory service returned an error without a valid problem response. StatusCode:{StatusCode}",
                    response.StatusCode
                );

                throw new BadRequestException("Inventory service returned a empty or malformed response.");
            }

            logger.LogWarning(
                "Inventory service returned an error. StatusCode:{StatusCode},Code:{Code}, Detail:{Details}",
                response.StatusCode,
                problem.Code,
                problem.Detail
            );

            if (problem.Code == InventoryErrorCodes.InsufficientStock)
                throw new ConflictException(problem.Detail ?? "Not enough stock.", problem.Code);

            throw new BadRequestException(problem.Detail ?? "Inventory service returned an error.");
        }

        var result = await response.Content.ReadFromJsonAsync<ReserveStocksResponse>(cancellation);

        if (result is null)
        {
            logger.LogError("Inventory service returned a empty or malformed response. Request:{@Request}", request);
            throw new BadRequestException("Inventory service returned an empty malformed response.");
        }

        return result;
    }
}