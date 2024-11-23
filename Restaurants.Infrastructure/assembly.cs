using System.Runtime.CompilerServices;

//This attribute will allow us to actually use the internal classes or any other types from our test projects
[assembly: InternalsVisibleTo("Restaurants.Infrastructure.Tests")]
//Required for Moq
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
