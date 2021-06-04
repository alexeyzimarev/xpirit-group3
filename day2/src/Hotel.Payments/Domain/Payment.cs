using Eventuous;
using static Hotel.Payments.Domain.PaymentEvents;

namespace Hotel.Payments.Domain {
    public class Payment : Aggregate<PaymentState, PaymentId> {
        public void ProcessPayment(PaymentId paymentId, string bookingId, float amount, string method, string provider, string currency) {
            Apply(new PaymentRecorded(paymentId, bookingId, amount, method, provider, currency));
        }
    }

    public record PaymentState : AggregateState<PaymentState, PaymentId> {
        public string BookingId { get; init; }
        public float  Amount    { get; init; }
    }

    public record PaymentId(string Value) : AggregateId(Value);
}