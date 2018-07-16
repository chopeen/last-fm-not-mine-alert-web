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
- [Azure Key Vault configuration provider in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-2.1&tabs=aspnetcore2x)
- [Safe storage of app secrets in development in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.1&tabs=linux)

## TODO

1. (!) Modify the API (and then `PostAsJsonAsync`) so that the key is sent as a query parameter (or even better - `x-functions-key` HTTP header), but artist name in the request body
1. (!) Show a confirmation that a new entry was added successfully
1. Find some CSS styling
1. Add robots.txt to prevent indexing
1. Validation for for entered values - letters, digits, commas, hyphens, what else?
1. Restore jQuery to use the out-of-the-box validation https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-aspnet-mvc4/adding-validation-to-the-model#validation-error-ui-in-aspnet-mvc
1. Some rudimentary authorization or CAPTCHA?
1. Helper method @Html.Button
 - https://books.google.pl/books?id=gEFPDwAAQBAJ&lpg=PA221&ots=So43eOzUpa&dq=IHtmlHelper%20button&pg=PA221#v=onepage&q&f=false
 - https://github.com/HtmlTags/htmltags/blob/master/src/HtmlTags.AspNetCore.TestSite/HtmlHelperExtensions.cs
