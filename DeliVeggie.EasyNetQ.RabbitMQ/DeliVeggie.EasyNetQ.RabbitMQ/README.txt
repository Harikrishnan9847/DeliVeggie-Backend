This is a shared library that could be used across DeliVeggie platform. 
This library forms a wrapper around EasyNetQ library.
Currently this library only supports request/response model.

Please add a field under connection strings in the appsettings.json, as given below
"ConnectionStrings": {
    "rabbitMqConnectionString": "host=localhost;timeout=60",
  }

please add a dependency binding as given below in your project
services.AddSingleton<IEasyNetQBus, EasyNetQBus>();

Interfaces
----------
IEasyNetQBus

Classes
-------
EasyNetQBus
