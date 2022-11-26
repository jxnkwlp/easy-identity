# EasyIdentity

OpenID Connect and OAuth 2.0 implement framework for asp.net core.

> The project is currently under development, if you have any ideas, please create issue

## Getting started

```
// register 
var identityBuilder = builder.Services.AddEasyIdentity(option => { });
// other options
identityBuilder
    .AddStandardScopes()
    .AddDevelopmentRSACredentialsStore()
    ... 
```

TODO

## Documentation

TODO

## Roadmap

Version 1.0

- [x] Core flow design
- [ ] All standard grant type implement
- [ ] Custom grant type interface
- [ ] OpenId connect implement
