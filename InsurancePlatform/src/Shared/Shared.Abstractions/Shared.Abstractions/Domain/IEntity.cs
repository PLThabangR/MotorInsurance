using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Abstractions.Domain
{
    //In DDD ,entities have entity identity and are mutable. They are defined by their identity rather than their attributes. Entities can change over time, but their identity remains the same. In contrast, value objects are immutable and defined by their attributes. They do not have a unique identity and are often used to represent concepts that do not change over time.
    public interface IEntity<TID>
    {

        // <summary>
        // Gets the unique identifier of the entity.
        //two entities are considered equal if they have the same identity, regardless of their other attributes. This is because the identity is what defines the entity, and it remains constant even if other attributes change.
        // </summary>
        TID Id { get; }
    }
}
