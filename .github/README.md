# Basic Web UI to manage configuration for `last-fm-not-mine-alert-func`

## Architecture

    +-----------------------+
    |                       |
    | Web UI (this repo)    |
    |                       |
    +-----------+-----------+
                |
                |
    +-----------v-----------+       +-------------------------+        +----------------------+
    |                       |       |                         |        |                      |
    | `not-my-artists` API  +-------> `send-alert`            +--------> email                |
    |                       |       |                         |        |                      |
    +-----------^-----------+       +------------^------------+        +----------------------+
                |                                |
                |                                |
    +-----------v-----------+                    |
    |                       |                    |
    | configuration tables  |                    |
    |                       |                    |
    +-----------------------+                    |
                                                 |
                                    +------------+-------------+
                                    |                          |
                                    | Last.fm API              |
                                    |                          |
                                    +--------------------------+

## Related projects

The core of the solution is the repository [last-fm-not-mine-alert-func](https://github.com/chopeen/last-fm-not-mine-alert-func).
It contains two Azure functions:

- `send-alert` (timer trigger) - fetching information from Last.fm API and sending email alerts
- `not-my-artists` (HTTP trigger) - API to access the configuration stored in Azure Storage tables

## Links

- [Introduction to Razor Pages in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-2.1&tabs=visual-studio-codex)
- [Safe storage of app secrets in development in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.1&tabs=linux)

## TODO

1. (!) Show a confirmation that a new entry was added successfully (by redirecting to a Success page)
1. Restore the full-blown authentication **and** implement authorization
    - *By default, App Service provides authentication but does not restrict authorized access to your site content and APIs. You must authorize users in your app code.*
    - https://docs.microsoft.com/gl-es/azure/app-service/app-service-authentication-overview#user-claims
    - https://github.com/aspnet/Docs/tree/master/aspnetcore/security/authorization/razor-pages-authorization/samples/2.x/AuthorizationSample 
1. Add a page to list existing artists
1. Add ILogger and ICont to the minimal template
1. Find some CSS styling
1. Validation for for entered values - letters, digits, commas, hyphens, what else?
1. Restore jQuery to use the out-of-the-box validation https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-aspnet-mvc4/adding-validation-to-the-model#validation-error-ui-in-aspnet-mvc
1. Helper method @Html.Button
    - https://books.google.pl/books?id=gEFPDwAAQBAJ&lpg=PA221&ots=So43eOzUpa&dq=IHtmlHelper%20button&pg=PA221#v=onepage&q&f=false
    - https://github.com/HtmlTags/htmltags/blob/master/src/HtmlTags.AspNetCore.TestSite/HtmlHelperExtensions.cs
1. Performance of `func`
    - example call
        - Total:   2500ms
        - Last.fm: 1500ms
        - What's eating up the remaining time?