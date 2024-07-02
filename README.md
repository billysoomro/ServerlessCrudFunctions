# ServerlessCrudFunctions

A simple .NET Lambda project with functions that correspond to CRUD (Create, Read, Update, Delete) actions. Multiple CRUD functions have been bundled together in a single project instead of creating separate projects for ease of development and maintenance. Each function has its own aws-lambda-tools-defaults.json (re-named to match the function/action name) file to manage deployment settings independently, allowing for flexibility and ease of management.

### Features
- Create: Add new items to a DynamoDB table.
- Read: Retrieve items from the DynamoDB table.
- Update: Modify existing items in the DynamoDB table.
- Delete: Remove items from the DynamoDB table.

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [AWS CLI](https://aws.amazon.com/cli/)
- AWS credentials configured for DynamoDB access
- A "Guitars" DynamoDB table with a partition key of "id"


## License
This project is licensed under the MIT License. See the LICENSE file for details.
