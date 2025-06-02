_2023-12-23 - Anthony Coudène - Creation_

# FTP PowerBI

A POC to express this kind of "architecture":
- A frontal client in Blazor WASM
- A frontal web in Asp.Net Core which upload a FTP from:
  - A serialization of a list of selected DTOs from user interface (export button)
  - A conversion in CSV file (property name are column headers and property values are column values)
  - An FTP upload to linux container
- A classical interaction with a backend api to store data in MongoDb (container)
- A Background service to simulate:
  - A periodical FTP upload towards CSV file (switch on launching)
  - A Cron approach to the same behavior (switch on launching).
 
This POC uses a .Net Aspire orchestration to illustrate all these resources.
