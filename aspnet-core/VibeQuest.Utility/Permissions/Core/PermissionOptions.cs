namespace VibeQuest.Utility.Permissions
{
    public class PermissionOptions
    {
        public ITypeList<IPermissionDefinitionProvider> DefinitionProviders { get; }

        public ITypeList<IPermissionValueProvider> ValueProviders { get; }

        public PermissionOptions()
        {
            DefinitionProviders = new TypeList<IPermissionDefinitionProvider>();
            ValueProviders = new TypeList<IPermissionValueProvider>();
        }
    }
}
