using System.Collections.Generic;
using System.Linq;
using GameBrains.Extensions.Lists;
using UnityEngine;

namespace GameBrains.Entities.Types
{
    [System.Serializable]
    public class EntityTypes
    {
        #region Constructors

        public EntityTypes()
        {
            types = new List<EntityType>();
        }

        public EntityTypes(params EntityType[] entityTypes) : this()
        {
            foreach (var entityType in entityTypes)
            {
                types.Add(entityType);
            }
        }

        #endregion Constructors

        [SerializeField] List<EntityType> types;

        public void Add(EntityType entityType)
        {
            if (entityType && !Contains(entityType))
            {
                types.Add(entityType);
            }
        }

        public void Remove(EntityType entityType)
        {
            if (entityType && Contains(entityType))
            {
                types.Remove(entityType);
            }
        }

        public void RemoveAt(int index)
        {
            types.RemoveAt(index);
        }

        public bool IsA(EntityType entityType)
        {
            return entityType && Contains(entityType);
        }

        public bool IsSubsetOf(EntityTypes supersetEntityTypes)
        {
            return types.Intersect(supersetEntityTypes.types).Count() == Count;
        }

        public void RemoveDuplicates()
        {
            var count = Count;
            // Removes nulls and duplicates
            types = types.Where(t => t).Distinct().ToList();
            if (Count < count)
            {
                // add one null back so we can replace it with an actual type in the inspector
                types.Add(default);
            }
        }

        public int Count => types.Count;

        public bool Contains(EntityType entityType)
        {
            return types.Contains(entityType);
        }

        public override string ToString()
        {
            return types.ToCommaSeparatedValuesString();
        }
    }
}