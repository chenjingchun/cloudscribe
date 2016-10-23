﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Core.IdentityServer.EFCore.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly IConfigurationDbContext context;
        private string _siteId;

        public ClientStore(
            SiteContext site,
            IConfigurationDbContext context
            )
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _siteId = site.Id.ToString();
            this.context = context;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await context.Clients
                .AsNoTracking()
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .FirstOrDefaultAsync(x => x.SiteId == _siteId && x.ClientId == clientId);

            var model = client.ToModel();

            return model;
        }
    }
}