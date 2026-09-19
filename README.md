
# MultiTenant API

A sample ASP.NET Core Web API demonstrating a multi-tenant project for product management.

## Overview
- Project: MultiTenant
- Framework: ASP.NET Core
- Purpose: Manage products via REST endpoints (GET, POST)

## Running
1. Ensure dotnet SDK is installed (recommended version used by project).
2. Restore and run:

```bash
dotnet restore
dotnet run --project MultiTenant
```

3. Open Swagger UI (in Development): https://localhost:{port}/swagger

## Notable fix
The CreateProduct action originally used multiple simple parameters decorated with [FromBody], which causes an exception because ASP.NET Core allows only one parameter bound from the request body. The recommended fix is to use a single DTO or accept a single model from the body.

Example DTO approach:

```csharp
public class CreateProductDto
{
	public string Name { get; set; }
	public double Price { get; set; }
	public int Rated { get; set; }
}

[HttpPost]
public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
{
	var product = new Product
	{
		Name = dto.Name,
		Price = (decimal)dto.Price,
		Rated = dto.Rated
	};
	var createdProduct = await _productService.CreateProductAsync(product);
	return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
}
```

## Notes
- Alternatively bind some parameters from query/route, but prefer DTO for request body clarity and versioning.

