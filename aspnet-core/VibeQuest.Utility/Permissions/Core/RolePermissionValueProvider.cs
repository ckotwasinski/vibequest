﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Permissions.Core
{
    public class RolePermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "R";

        public override string Name => ProviderName;

        public RolePermissionValueProvider(IPermissionStore permissionStore)
            : base(permissionStore)
        {

        }

        public async override Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            //var roles = context.Principal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();

            //if (roles == null || !roles.Any())
            //{
            //    return PermissionGrantResult.Undefined;
            //}

            //foreach (var role in roles)
            //{
            //    if (await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, role))
            //    {
            //        return PermissionGrantResult.Granted;
            //    }
            //}

            return PermissionGrantResult.Granted;
        }

        public async override Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
        {
            var permissionNames = context.Permissions.Select(x => x.Name).ToList();
            var result = new MultiplePermissionGrantResult(permissionNames.ToArray());

            //var roles = context.Principal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();
            //if (roles == null || !roles.Any())
            //{
            //    return result;
            //}

            //foreach (var role in roles)
            //{
            //    var multipleResult = await PermissionStore.IsGrantedAsync(permissionNames.ToArray(), Name, role);
            //    foreach (var grantResult in multipleResult.Result.Where(grantResult =>
            //        result.Result.ContainsKey(grantResult.Key) &&
            //        result.Result[grantResult.Key] == PermissionGrantResult.Undefined &&
            //        grantResult.Value != PermissionGrantResult.Undefined))
            //    {
            //        result.Result[grantResult.Key] = grantResult.Value;
            //        permissionNames.RemoveAll(x => x == grantResult.Key);
            //    }

            //    if (result.AllGranted || result.AllProhibited)
            //    {
            //        break;
            //    }
            //}

            return result;
        }
    }
}
