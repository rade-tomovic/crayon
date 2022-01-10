# Crayon interview task

### Prerequisites

- Visual Studio 2022
- .NET 6

### How to run

Open solution inside VS 2022, set API project as startup project and run. It will start API on https://localhost:7191, with generated Swagger UI.

### What could be done (if more time is invested)

- More unit/integration tests, only a few are added for the purposes of testing main functionalities
- Instead of sending all requests at once with `Task.WhenAll`, we could create a strategy which will run:
  - Single task if only one date is provided
  - `Task.WhenAll` with up to N (i.e. N=10)
  - Split into batch processing of N tasks and run the second case (i.e. 60 dates = 6 batch x 10 tasks)
- Response caching, either built-in or distributed Redis-like
