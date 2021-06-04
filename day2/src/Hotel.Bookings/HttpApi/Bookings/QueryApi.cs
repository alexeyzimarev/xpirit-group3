using System.Threading;
using System.Threading.Tasks;
using Eventuous;
using Hotel.Bookings.Domain.Bookings;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Bookings.HttpApi.Bookings {
    [Route("/bookings")]
    public class QueryApi : ControllerBase {
        readonly IAggregateStore _store;
        
        public QueryApi(IAggregateStore store) => _store = store;

        [HttpGet]
        [Route("{id}")]
        public async Task<BookingState> GetBooking(string id, CancellationToken cancellationToken) {
            var booking = await _store.Load<Booking, BookingState, BookingId>(new BookingId(id), cancellationToken);
            return booking.State;
        }
    }
}