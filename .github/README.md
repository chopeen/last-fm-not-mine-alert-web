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

## Non-ASCII characters in regular expressions

.NET Core supports the names of Unicode blocks in regular expressions, so there's no need to specify
every diacritic character explicitly.

- [Unicode block names for use with the `\p` token](https://www.w3.org/TR/xsd-unicode-blocknames/)
- [Unicode regular expressions](https://www.regular-expressions.info/unicode.html)
- ["Latin-1 Supplement" Unicode block](https://en.wikipedia.org/wiki/Latin-1_Supplement_(Unicode_block))

## Links

- [Introduction to Razor Pages in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-2.1&tabs=visual-studio-codex)
- [Safe storage of app secrets in development in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.1&tabs=linux)
- [Example Code - Opinionated ContosoUniversity on ASP.NET Core 2.0's Razor Pages](https://www.hanselman.com/blog/ExampleCodeOpinionatedContosoUniversityOnASPNETCore20sRazorPages.aspx)
  (lots if useful links in the article)

## TODO

1. (!!) Read about authentication and authorization in .NET Core - what will be easy to implement? Some standard implementation to replace the current custom one.
    - *By default, App Service provides authentication but does not restrict authorized access to your site content and APIs. You must authorize users in your app code.*
    - https://docs.microsoft.com/gl-es/azure/app-service/app-service-authentication-overview#user-claims
    - https://github.com/aspnet/Docs/tree/master/aspnetcore/security/authorization/razor-pages-authorization/samples/2.x/AuthorizationSample
1. (!) Show a confirmation that a new entry was added successfully (Success page or jQuery notification?)
1. (!) Add a page to list existing artists (new page or Index?)
1. Place the label and text box next to each other; make the latter smaller
1. Add ILogger and ICont to the minimal template
1. Performance of `func`
    - example call
        - Total:   2500ms
        - Last.fm: 1500ms
        - What's eating up the remaining time?
