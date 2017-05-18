﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Wodsoft.ComBoost.Security
{
    public class ComBoostPrincipal : ClaimsPrincipal, IAuthentication
    {
        public ComBoostPrincipal(ISecurityProvider securityProvider)
        {
            if (securityProvider == null)
                throw new ArgumentNullException(nameof(securityProvider));
            SecurityProvider = securityProvider;
        }

        public ISecurityProvider SecurityProvider { get; private set; }

        public T GetUser<T>()
        {
            if (!Identity.IsAuthenticated)
                return default(T);
            string id = ((ClaimsIdentity)Identity).FindFirst(t => t.Type == ClaimTypes.NameIdentifier).Value;
            return (T)SecurityProvider.GetPermissionAsync(id).Result;
        }
        
        public bool IsInStaticRole(object role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (!Identity.IsAuthenticated)
                return false;
            var roles = FindAll(t => t.Type == ClaimTypes.Role);
            if (roles.Any(t => t.Value == SecurityProvider.ConvertRoleToString(role)))
                return true;
            return false;
        }

        public bool IsInDynamicRole(object role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (!Identity.IsAuthenticated)
                return false;
            string id = FindFirst(t => t.Type == ClaimTypes.NameIdentifier).Value;
            return SecurityProvider.GetPermissionAsync(id).Result.IsInRole(role);
        }

        public bool IsInRole(object role)
        {
            if (!Identity.IsAuthenticated)
                return false;
            if (IsInStaticRole(role))
                return true;
            return IsInDynamicRole(role);
        }
    }
}
