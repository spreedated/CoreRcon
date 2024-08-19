# Connecting

Create a new `Rcon` object with the server's IP, port, and password.

```csharp
Rcon connection = new(IPAddress.Parse("10.10.10.5"), 27015, "myVerySecurePassword");
await connection.ConnectAsync();

if(connection.Connected)
{
    // Do stuff with the connection
}
```