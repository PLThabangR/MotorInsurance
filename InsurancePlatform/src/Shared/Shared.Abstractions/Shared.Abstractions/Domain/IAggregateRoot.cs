using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Abstractions.Domain
{/// <summary>
        /// Marker interface for Aggregate Roots in Domain-Driven Design.
        /// An aggregate root is the entry point to an aggregate.
        /// It ensures business invariants are maintained.
        /// 
        /// Examples of Aggregate Roots:
        /// - Quote (contains QuoteItems)
        /// - Policy (contains PolicyCoverages)
        /// - Claim (contains ClaimDocuments)
        /// </summary>
    public interface IAggregateRoot:IEntity<Guid>
    {   /// <summary>
    /// Gets the domain events that have occurred on this aggregate.
    /// These are collected during business operations and published after persistence.
    /// </summary>
        
        IReadOnlyList<IDomainEvent> DomainEvents { get; }

        

        //Clear all domain events after they have been published
        void ClearDomainEvents();
        


    }
}
