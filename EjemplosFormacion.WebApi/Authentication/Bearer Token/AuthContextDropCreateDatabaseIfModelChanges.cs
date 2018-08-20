﻿using EjemplosFormacion.WebApi.Authentication.BearerToken.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace EjemplosFormacion.WebApi.Authentication.BearerToken
{
    public class AuthContextDropCreateDatabaseIfModelChanges : DropCreateDatabaseIfModelChanges<TestAuthContext>
    {
        protected override void Seed(TestAuthContext context)
        {
            base.Seed(context);

            var webClientApp = new TestClientApp()
            {
                Id = "WebApp",
                Secret = @"8EIwyeSbUeltZsQvmKQfU/cOlv05zy179yRJWtYAPU0=", // Time2014*
                ApplicationType = TestApplicationTypes.JavaScript,
                Name = "Web non secure client app",
                AllowedOrigin = "*",
                Active = true,
                RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(1).TotalMinutes),
            };

            var nativeClientApp = new TestClientApp()
            {
                Id = "NativeApp",
                Secret = @"8EIwyeSbUeltZsQvmKQfU/cOlv05zy179yRJWtYAPU0=", // Time2014*
                ApplicationType = TestApplicationTypes.NativeConfidential,
                Name = "Native secure client app",
                AllowedOrigin = "*",
                Active = true,
                RefreshTokenLifeTime = Convert.ToInt32(TimeSpan.FromDays(1).TotalMinutes),
            };

            IdentityUser user = new IdentityUser
            {
                UserName = "Taiseer"
            };

            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
            IdentityResult result = userManager.CreateAsync(user, "SuperPass").Result;

            context.ClientApps.Add(webClientApp);
            context.ClientApps.Add(nativeClientApp);

            context.SaveChanges();
        }
    }
}