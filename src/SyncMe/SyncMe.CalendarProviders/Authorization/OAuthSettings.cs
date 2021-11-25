using System;
using System.Collections.Generic;
using System.Text;

namespace SyncMe.CalendarProviders.Authorization
{
    public static class OAuthSettings
    {
        public const string ApplicationId = "904b52b5-a7ad-4ad5-b7e5-23160e0800e1";
        public const string Scopes = "Calendars.Read";
        public const string RedirectUri = @"msauth://com.companyname.SyncMe";
    }
}
