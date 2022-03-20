namespace GameBrains.Extensions.ScriptableObjects
{
    public abstract class EnumScriptableObject : ExtendedScriptableObject
    {
        public string description;
        public string shortDescription;

        public override void OnEnable() { base.OnEnable(); description ??= name; }
        
        public override string ToString() { return shortDescription; }
    }
}