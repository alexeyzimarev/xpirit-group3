using EventStore.Client;
using Eventuous;
using Eventuous.Subscriptions;
using Hotel.Bookings.Application.Bookings;
using Microsoft.Extensions.Logging;
using StreamSubscription = Eventuous.Subscriptions.EventStoreDB.StreamSubscription;

namespace Hotel.Bookings.Acls {
    public class PaymentsSubscription : StreamSubscription {
        public const string Id = "PaymentsAcl";

        public PaymentsSubscription(
            EventStoreClient       eventStoreClient,
            BookingsCommandService service,
            ICheckpointStore       checkpointStore,
            IEventSerializer       eventSerializer,
            ILoggerFactory         loggerFactory
        ) : base(
            eventStoreClient,
            "PaymentsIntegration",
            Id,
            checkpointStore,
            new[] {new PaymentReaction(service)},
            eventSerializer,
            loggerFactory
        ) { }

        class PaymentReaction : TypedEventHandler {
            public override string SubscriptionId => Id;

            public PaymentReaction(BookingsCommandService service) {
                On<PaymentIntegrationEvents.BookingPaymentRecorded>(
                    (evt, _, token) => service.Handle(
                        new BookingCommands.RecordPayment(
                            evt.BookingId,
                            evt.Amount,
                            evt.Currency,
                            evt.PaymentId,
                            ""
                        ),
                        token
                    )
                );
            }
        }
    }

    static class PaymentIntegrationEvents {
        public record BookingPaymentRecorded(string PaymentId, string BookingId, float Amount, string Currency);

        public static void MapEvents() {
            TypeMap.AddType<BookingPaymentRecorded>("BookingPaymentRecorded");
        }
    }
}