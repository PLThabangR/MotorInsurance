using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//IDomainEvent - Records business facts. Example: PolicyIssued, ClaimSubmitted.

namespace Shared.Abstractions.Domain
{
    // <summary>
    // Represents event that happens within the domain. Domain events are used to capture and communicate important changes or occurrences within the domain model. They are typically used to trigger side effects, notify other parts of the system, or facilitate communication between different bounded contexts.
    // Example :QoutesCreated,PolicyIssued, ClaimSubmitted, PaymentReceived, CustomerRegistered, OrderShipped, ProductBackInStock, UserLoggedIn, PasswordChanged, AccountDeactivate, FeedbackReceived, TaskCompleted, EventScheduled, NotificationSent
    public interface IDomainEvent
    {
        //Get the date and time when the domain event occurred.
        DateTime OccurredOn { get; }
    }
}
