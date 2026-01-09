using Informatique.Alumni.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Informatique.Alumni.Permissions;

public class AlumniPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AlumniPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(AlumniPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(AlumniPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(AlumniPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(AlumniPermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(AlumniPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AlumniResource>(name);
    }
}
