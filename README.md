# Expression Evaluation Service

.NET Core 3.1 web service used to evaluate expressions in input query

## How to build & run
```
dotnet build
dotnet run --project src/ExpressionEvaluation.Api
```

## How to query
```
curl http://localhost:8000/compute?expr=<your-expression>
```

## Example:
```
curl http://localhost:8000/compute?expr=((1%2B2)*43)%2F3.14%2B2%5E3
49.0828025477707
```
