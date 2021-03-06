﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UnrealEstate.Business.Authentication.ViewModel.Request;
using UnrealEstate.Business.Authentication.ViewModel.Response;
using UnrealEstate.Infrastructure.Models;
using IAuthenticationService = UnrealEstate.Business.Authentication.Service.IAuthenticationService;

namespace UnrealEstate.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            _ = returnUrl ?? Url.Action("Index", "Home");

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = await _authenticationService.GetExternalAuthenticationSchemesAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResult
                    = await _authenticationService.LoginAsync(model);

                if (loginResult.ResponseStatus == AuthenticationResponseStatus.Success)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return LocalRedirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, loginResult.Message);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Authentication", new { ReturnUrl = returnUrl });

            AuthenticationProperties properties = _authenticationService.GetExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            var loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = await _authenticationService.GetExternalAuthenticationSchemesAsync()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            var result = await _authenticationService.ExternalLoginAsync(loginViewModel);

            if (result.ResponseStatus == AuthenticationResponseStatus.Error)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View("Login", loginViewModel);
            }

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            if (ModelState.IsValid)
            {
                AuthenticationResponse authenticationResponseViewModel = await _authenticationService.SendResetPasswordEmail(model.Email);

                if (authenticationResponseViewModel.ResponseStatus == AuthenticationResponseStatus.Success)
                {
                    TempData["authenticationResponseMessage"] = authenticationResponseViewModel.Message;

                    return RedirectToAction(nameof(ResetPassword));
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid operation");

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            if (TempData["authenticationResponseMessage"] != null)
            {
                ViewData["authenticationResponseMessage"] = TempData["authenticationResponseMessage"].ToString();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            if (ModelState.IsValid)
            {
                var authenticationResponseViewModel = await _authenticationService.ResetPassword(model);

                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Invalid operation");

            return View();
        }
    }
}
