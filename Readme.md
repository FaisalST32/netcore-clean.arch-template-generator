## Generate ASP.Net Core 3.1 Template with Clean Architecture

### Template includes support for

1. AutoMapper
2. Entity Framework Core
3. Swagger

To use, simply clone the repository and in the root folder run the following command to install the tool globally

```
dotnet tool update --global --add-source ./nupkg netcore.cleanarch
```

You can then run the following command to create a Project in your preferred directory

```
cleanarch <SolutionName> <ProjectPrefix>
e.g., cleanarch FaisalsBlog Blog
```

