using UnityEngine;

namespace Entity
{
    [CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObjects/Entity", order = 1)]
    public sealed class ScriptableObjectEntity : ScriptableObject, IEntity
    {
        [SerializeField] private string _identifier;
        public string Identifier => _identifier;
    }
}
