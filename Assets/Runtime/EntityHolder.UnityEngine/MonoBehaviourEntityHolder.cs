using Entity;
using UnityEngine;

namespace EntityHolder
{
    public sealed class MonoBehaviourEntityHolder : MonoBehaviour, IEntityHolder
    {
        public ScriptableObjectEntity _entity;
        public IEntity Entity => _entity;
    }
}
