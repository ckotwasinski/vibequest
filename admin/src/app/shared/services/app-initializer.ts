import { AccountService } from "src/app/login-page/service/account.service";
import { PermissionsService } from "src/app/permission-management/services/permissions.service";
import { StorageKeys } from "../config/constants";

export function appInitializer(permissionService: PermissionsService,
  accountService: AccountService) {
  return () =>
  new Promise((resolve) => {
    if (localStorage.getItem(StorageKeys.user) != null) {
      accountService.setUser();
    }

    permissionService.getUserPermissions().subscribe().add(resolve);
  });
}
