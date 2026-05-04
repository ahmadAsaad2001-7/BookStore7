using MediatR;

namespace StoreWebapi.Application.Features.Orders.ConfirmOrderPayment;

public record ConfirmOrderPaymentCommand(Guid OrderId) : IRequest<Result>;