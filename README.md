This repository implements a test suite for https://www.saucedemo.com/ as outlined in final task description.

## Setup

This project uses appsettings.json for configuration. To override settings locally, an appsettings.local.json may be used.

This project uses [Selenium Grid](https://www.selenium.dev/documentation/grid/getting_started/) to run tests in parallel. In order to run the project, a URL to an active Selenium server must be specified in appsettings. By default, it tries to connect to `https://localhost:4444`, which is the default URL for a local Selenium Grid instance.

Logs output path may also be specified in appsettings.json. By default, logs are written to the `Logs` directory in current directory, which is usually in `bin\Debug\net9.0\`.

Example config file:
```json

{
  "BaseUrl": "https://www.saucedemo.com",
  "Selenium": {
    "GridUrl": "http://localhost:4444"
  },

  "Logs": {
    "FilePath": "E:/FinalTask/Logs/log-{Date}-{Time}-{ThreadIdx}.log",
    "Level": "Verbose"
  }
}
```

## Task description

Launch URL: https://www.saucedemo.com/

### UC-1 Test Login form with empty credentials:

- Type any credentials into "Username" and "Password" fields.
- Clear the inputs.
- Hit the "Login" button.
- Check the error messages: "Username is required".

### UC-2 Test Login form with credentials by passing Username:

- Type any credentials in username.
- Enter password.
- Clear the "Password" input.
- Hit the "Login" button.
- Check the error messages: "Password is required".

### UC-3 Test Login form with credentials by passing Username & Password:

- Type credentials in username which are under Accepted username are sections.
- Enter password as secret sauce.
- Click on Login and validate the title “Swag Labs” in the dashboard.

Provide parallel execution, add logging for tests and use Data Provider to parametrize tests. Make sure that all tasks are supported by these 3 conditions: UC-1; UC-2; UC-3.
Please, add task description as README.md into your solution!

To perform the task use the various of additional options:

- Test Automation tool: Selenium WebDriver;
- Browsers:
	1) Firefox;
	2) Edge;
- Locators: CSS;
- Test Runner: MSTest;
- Assertions: FluentAssertions;
- [Optional] Patterns:
	1) Singleton;
	2) Adapter;
	3) Strategy;
- [Optional] Test automation approach: BDD;
- [Optional] Loggers: Serilog.