# .net core技术文档
## 技术介绍
> **本技术文档将介绍开发过程中所用到的技术框架和应该注意的点（防止之后查看的时候没有头绪，或者再踩到同样的坑）。**

> **本技术框架主要是围绕微软所提供的开源框架，如有不懂，或者在此文档写的不够细致的情况下可以自行查看[微软官方文档](https://docs.microsoft.com/zh-cn/ "微软官方文档")**

----

##	主要使用的框架
> **.net core**
**ASP.NET Core 是一个跨平台的高性能开源框架，用于生成启用云且连接 Internet 的新式应用。 使用 ASP.NET Core，您可以：**
-	生成 Web 应用和服务、物联网 (IoT) 应用和移动后端。
-	在 Windows、macOS 和 Linux 上使用喜爱的开发工具。
-	部署到云或本地。
-	在 .NET Core 上运行。

> **EFcore** 

**EF Core 是一个 ORM(Object Relational Mapping)对象关系映射 框架，它也提供了对数据增删改查的基础封装，提供了Code First 的开发，它也有批量增删的功能扩展；**
-	支持多种数据库 MSSQL ,MySQL,SQLite，InMemory
-	支持数据库的逆向工程，可以先建立模型也可以先建立数据库
-	更改模型后可以使用迁移来更新数据库架构
-	轻量级的， 开源的， 可扩展的，支持跨平台的
-	使用简易，使用的人多，能够提高生产效率
-	可以使用Linq

> **数据库sqlserver** 

**SQL是英文Structured Query Language的缩写，意思为结构化查询语言。SQL语言的主要功能就是同各种数据库建立联系，进行沟通。按照ANSI(美国国家标准协会)的规定，SQL被作为关系型数据库管理系统的标准语言。SQL Server是由Microsoft开发和推广的关系数据库管理系统（RDBMS）。在平常的开发过程中最好不要对数据库进行功能上的操作（比如触发器），但是如果业务需要还是可以使用，但是会加大数据库压力，降低效率。存储过程维护起来不方便，但是涉及到金融或者是比较重要的数据还是可以使用。**

> **MongoDB**

**MongoDB 是由C++语言编写的，是一个基于分布式文件存储的开源数据库系统，**
*面向文档存储的数据库，操作起来比较简单和容易。*
-	你可以在MongoDB记录中设置任何属性的索引 (如：FirstName="Sameer",Address="8 Gandhi Road")来实现更快的排序。
-	Mongo支持丰富的查询表达式。查询指令使用JSON形式的标记，可轻易查询文档中内嵌的对象及数组。

> **Redis**

**Redis 是完全开源的，遵守 BSD 协议，是一个高性能的 key-value 数据库。**
**Redis 与其他 key - value 缓存产品有以下三个特点：**
-	Redis支持数据的持久化，可以将内存中的数据保存在磁盘中，重启的时候可以再次加载进行使用。
-	Redis不仅仅支持简单的key-value类型的数据，同时还提供list，set，zset，hash等数据结构的存储。
-	Redis支持数据的备份，即master-slave模式的数据备份。

> **Swagger**

 **我在这里只介绍后端用到的。Swagger UI:提供了一个可视化的UI页面展示描述文件。接口的调用方、测试、项目经理等都可以在该页面中对相关接口进行查阅和做一些简单的接口请求。该项目支持在线导入描述文件和本地部署UI项目。**

> **ioc依赖注入（DI）**

**DI在.NET Core里面被提到了一个非常重要的位置。当一个类需要另一个类协作来完成工作的时候就产生了依赖。比如我们在TestController这个控制器需要完成和用户相关的注册、登录 等事情。我主要使用了方法注入，构造函数注入。**

> **Hangfire**

-	Hangfire 是一个开源的.NET任务调度框架，目前1.6+版本已支持.NET Core。个人认为它最大特点在于内置提供集成化的控制台,方便后台查看及监控HangFire允许您以非常简单但可靠的方式启动请求处理管道外部的方法调用。这些方法调用在后台线程中执行，称为后台作业。另外Hangfire包含三大核心组件：客户端、持久化存储、服务端，
-	多语言支持
-	支持任务取消
-	支持按指定Job Queue处理任务
-	服务器端工作线程可控，即job执行并发数控制
-	分布式部署，支持高可用
-	良好的扩展性，如支持IOC、Hangfire Dashboard授权控制、Asp.net Core、持久化存储等

> **SignalR(目前没有添加，后续需要会加)**

 **SignalR是一个.NET Core/.NET Framework的开源实时框架. SignalR的可使用Web Socket, Server Sent Events 和 Long Polling作为底层传输方式.**
 **SignalR基于这三种技术构建, 抽象于它们之上, 它让你更好的关注业务问题而不是底层传输技术问题.**
 **SignalR这个框架分服务器端和客户端, 服务器端支持ASP.NET Core 和 ASP.NET; 而客户端除了支持浏览器里的javascript以外, 也支持其它类型的客户端, 例如桌面应用. SignalR利用底层传输来让服务器可以调用客户端的方法, 反之亦然, 这些方法可以带参数, 参数也可以是复杂对象, SignalR负责序列化和反序列化。**

> **consul(服务发现，集群目前还没有搞好)**

**consul 是 HashiCorp 公司推出的开源工具，用于实现分布式系统的服务发现与配置。与其他分布式服务注册与发现的方案，Consul的方案更“一站式”，内置了服务注册与发现框 架、具有以下性质：**
-	分布一致性协议实现、
-	健康检查、
-	Key/Value存储、
-	多数据中心方案，
-	不再需要依赖其他工具（比如ZooKeeper等）。
**使用起来也较 为简单。Consul使用Go语言编写，因此具有天然可移植性(支持Linux、windows和Mac OS X)；安装包仅包含一个可执行文件，方便部署，与Docker等轻量级容器可无缝配合 。** 
**基于 Mozilla Public License 2.0 的协议进行开源. Consul 支持健康检查,并允许 HTTP 和 DNS 协议调用 API 存储键值对.** 
**一致性协议采用 Raft 算法,用来保证服务的高可用. 使用 GOSSIP 协议管理成员和广播消息, 并且支持 ACL 访问控制.**

> **ocelot**

**Ocelot原本设计仅为与.NET Core一起使用的，它是一个.NET API网关，作为面向使用.NET运行微型服务/面向服务的体系结构需要统一的系统入口点，即当客户端（Web站点，手机APP）等访问Web API的时候，Ocelot作为统一的入口点会根据请求地址分发到对应的API站点去（寻址）。**

> **Serilog**

**与.NET的许多其他库一样，Serilog也提供对文件，控制台和其他地方的诊断日志记录 。它易于设置，具有简洁的API，并且可以在最新的.NET平台之间移植。与其他日志记录库不同，Serilog在构建时考虑了强大的结构化事件数据。**

> **Markdown**

**Markdown是一种可以使用普通文本编辑器编写的标记语言，通过简单的标记语法，它可以使普通文本内容具有一定的格式。接口文档如果不用这个来写，那么就忽视掉。**

----

##	项目技术框架结构详情

> **主要分为：DR.Configs，DR.EFcore，DR.EFCore.DbMigrations，DR.Extensions, DR.HangFire，DR.Models, DR.MongoDB, DR.Redis, DR.Services,DR.WebApi**

+ DR.Configs：DR.Configs{ ConfigModels，Config}
  + ConfigModels：consul配置文件
  + Config：获取配置文件内容--表：setting
+ DR.EFcore:{ EFCoreContextDR , EFCoreContextRead, EFCoreContextWrite }
   + EFCoreContextDR:拓展连接其他库
   + EFCoreContextRead：读库 （读写分离现在还没有弄好）
   + EFCoreContextWrite：写库（读写分离现在还没有弄好）
+ DR.EFCore.DbMigrations{ Migrations , EFCoreContext , appsettings .json}
 + Migrations:数据迁移记录文档。
 +  EFCoreContext：EF模型生成类
 + appsettings .json：配置
+ DR.Extensions公共方法写在这里
+ DR.HangFire hangfire类库
+ DR.Models 数据模型{包括：数据库模型，枚举，Dto模型}
+ DR.MongoDB{ DBRequestLogs ，MongoDBBaseService }
 + DBRequestLogs：MongoDB操作
+ MongoDBBaseService：MongoDB操作
 + DR.Redis :redis操作
 + DR.Services:{ Services 和IServices }
+ DR.WebApi 接口操作

