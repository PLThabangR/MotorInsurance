
using Shared.Abstractions.Domain;

namespace Shared.Abstractions.Repository
{   
    //This is a generic repository interface that defines the basic operations for managing aggregate roots in a data store.
    //TAgregate: The type of the aggregate root that this repository manages. It must implement the IAggregateRoot interface.

    public interface IRepository<TAggregate,TId> where TAggregate : IAggregateRoot
    {   // /// <summary>
        /// Retrieves an aggregate by its identifier.
        /// Returns null if not found.
        /// </summary>
        Task<TAggregate?> GetAsync(TId id);

        /// <summary>
        /// Adds a new aggregate to the repository.
        /// </summary>
        Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing aggregate.
        /// </summary>
        Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an aggregate from the repository.
        /// </summary>
        Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an aggregate exists with the given identifier.
        /// </summary>
        Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);



    }
}
