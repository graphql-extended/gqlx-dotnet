# gqlx for .NET Applications

[![Build Status](https://travis-ci.org/graphql-extended/gqlx-dotnet.svg?branch=master)](https://travis-ci.org/graphql-extended/gqlx-dotnet)
[![NuGet Count](https://img.shields.io/nuget/v/gqlx.svg)](https://www.nuget.org/packages/gqlx/)
[![GitHub tag](https://img.shields.io/github/tag/graphql-extended/gqlx-dotnet.svg)](https://github.com/graphql-extended/gqlx-dotnet/releases)
[![GitHub issues](https://img.shields.io/github/issues/graphql-extended/gqlx-dotnet.svg)](https://github.com/graphql-extended/gqlx-dotnet/issues)

.NET library for using the gqlx language.

![gqlx Logo](https://github.com/graphql-extended/gqlx-spec/raw/master/logo.png)

## Documentation

This library offers support for a new way of writing GraphQL schemas called **gqlx** (short of GraphQL eXtended). gqlx gives developers a possibility to mix the definitions of resolvers directly into their GraphQL type definitions. Additionally, more advanced capabilities to be utilized directly can be found in the language.

For more information on GraphQL and learning material, see [GraphQL College](https://www.graphql.college/practice-graphql/). The specification of the gqlx language is available [on GitHub](https://github.com/graphql-extended/gqlx-spec). Herein we will only present a few examples.

This library offers the same set of functionality as the [JavaScript version](https://github.com/graphql-extended/gqlx-js).

## Contributing

We are totally open for contribution and appreciate any feedback, bug reports, or feature requests. More detailed information on contributing incl. a code of conduct are soon to be presented.

## FAQ

*How much can be customized?*

The core language is pretty much defined by GraphQL and JavaScript (officially ECMAScript version 6, abbr. ES6). Currently, all customizations need to take place within the ECMAScript layer, e.g., by defining new / changing existing API functions or inbuilt functions (i.e., macros).

## Changelog

This project adheres to [semantic versioning](https://semver.org).

You can find the changelog in the [CHANGELOG.md](CHANGELOG.md) file.

## License

gqlx-dotnet is released using the MIT license. For more information see the [LICENSE file](LICENSE).
