﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApp.Models;
using WebApp.Models.User;
using WebApp.Services.Error;

namespace WebApp.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        public IErrorService ErrorService { get; }

        public static string TokenClaim => "JWToken";
        public static string LastNameClaim => "LastName";
        public static string UserIdClaim => "UserId";

        /// <summary>
        /// uzivatel nacteny v http contextu
        /// </summary>
        public UserModel MyUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    return new UserModel
                    {
                        Email = User.FindFirstValue(ClaimTypes.Email),
                        Firstname = User.FindFirstValue(ClaimTypes.Name),
                        Lastname = User.FindFirstValue(LastNameClaim),
                        Role = User.FindFirstValue(ClaimTypes.Role),
                        UserId = int.Parse(User.FindFirstValue(UserIdClaim))
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// vrati JWT z cookie
        /// </summary>
        public string Token
        {
            get => User.Identity.IsAuthenticated ? User.FindFirstValue(TokenClaim) : null;
        }

        public AppController(IErrorService errorService)
        {
            ErrorService = errorService;
        }

        /// <summary>
        /// ulozi errory z modelstate collection do messagelistu modelu
        /// </summary>
        /// <param name="model">model, do ktereho se to ma vlozit</param>
        protected void AddModelStateErrors(ViewModel model)
        {
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    model.MessageList.Add(new Tuple<string, bool>(error.ErrorMessage, true));
                }
            }
        }

        protected void AddMessageToModel(ViewModel model, string message, bool error = true)
        {
            model.MessageList.Add(new Tuple<string, bool>(message, error));
        }
    }
}