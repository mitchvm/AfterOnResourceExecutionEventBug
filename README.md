This code shows how to reproduce this issue where the ASP.NET Core MVC `AfterOnResourceExecution` diagnostic event is only emitted if the resource filter short-circuits the request, but the analogous `AfterOnActionExecution` event is emitted regardless if the action filter short-circuits the request.

This is implemented as a unit test project that uses the ASP.NET Core test host.

Steps to run:

1. Build the project.
2. Run the unit tests.

Expected results: all test cases pass

Actual results: the test cases in which the Async resource filter does not short-circuit the request fail
