# Third-Party Notices — DeferDataLoading

This project includes third-party packages. Each package remains licensed under its own license.  
When redistributing (source or binaries), preserve the original license texts and any required NOTICE files.

| Package | Version | License | Source |
|---|---:|---|---|
| Dapper | 2.1.66 | Apache-2.0 | https://www.nuget.org/packages/Dapper |
| Microsoft.Data.SqlClient | 6.1.1 | MIT | https://github.com/dotnet/SqlClient |
| Microsoft.Extensions.Hosting | 9.0.7 | MIT | https://www.nuget.org/packages/Microsoft.Extensions.Hosting |
| Microsoft.VisualStudio.Azure.Containers.Tools.Targets | 1.22.1 | Microsoft Software License (EULA; tooling) | https://www.nuget.org/packages/Microsoft.VisualStudio.Azure.Containers.Tools.Targets |
| MongoDB.Driver | 3.4.3 | Apache-2.0 | https://github.com/mongodb/mongo-csharp-driver |
| MySql.Data | 9.4.0 | GPL-2.0 with Universal FOSS Exception | https://dev.mysql.com/doc/index-other.html#foss-exception |
| Npgsql | 9.0.3 | PostgreSQL License (BSD-like) | https://www.npgsql.org/ |
| Oracle.ManagedDataAccess.Core | 23.9.1 | Oracle Free Use Terms and Conditions (FUTC) | https://www.nuget.org/packages/Oracle.ManagedDataAccess.Core |
| Quartz | 3.14.0 | Apache-2.0 | https://www.nuget.org/packages/Quartz |
| Quartz.Extensions.DependencyInjection | 3.14.0 | Apache-2.0 | https://www.nuget.org/packages/Quartz.Extensions.DependencyInjection |
| Quartz.Extensions.Hosting | 3.14.0 | Apache-2.0 | https://www.nuget.org/packages/Quartz.Extensions.Hosting |
| RabbitMQ.Client | 7.1.2 | Apache-2.0 OR MPL-2.0 (dual) | https://github.com/rabbitmq/rabbitmq-dotnet-client |
| Serilog.AspNetCore | 9.0.0 | Apache-2.0 | https://www.nuget.org/packages/Serilog.AspNetCore |
| Serilog.Settings.Configuration | 9.0.0 | Apache-2.0 | https://www.nuget.org/packages/Serilog.Settings.Configuration |
| Serilog.Sinks.Console | 6.0.0 | Apache-2.0 | https://www.nuget.org/packages/Serilog.Sinks.Console |
| Serilog.Sinks.Seq | 9.0.0 | Apache-2.0 | https://www.nuget.org/packages/Serilog.Sinks.Seq |

## Notes

- **MySql.Data** is GPL-2.0 **with the Universal FOSS Exception**. The exception allows dynamic linking with works under permissive licenses (e.g., MIT/Apache-2.0) without imposing GPL terms on your project, provided you don’t modify the library itself.
- **Oracle.ManagedDataAccess.Core (ODP.NET Core)** is provided under **Oracle Free Use Terms and Conditions (FUTC)**. It’s free to use but proprietary—ensure compliance for redistribution.
- **RabbitMQ.Client** is **dual-licensed** under **Apache-2.0 or MPL-2.0**. You may treat it as **Apache-2.0** for compatibility with this project’s MIT license.
- **Microsoft.VisualStudio.Azure.Containers.Tools.Targets** is a tooling/targets package governed by Microsoft’s EULA; it doesn’t change your project’s chosen license but should be kept in these notices.
