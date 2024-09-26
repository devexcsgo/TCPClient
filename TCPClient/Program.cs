using System;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;

Console.WriteLine("Client");

bool keepSending = true;

TcpClient socket = new TcpClient("127.0.0.1", 7); 
NetworkStream ns = socket.GetStream();
StreamReader reader = new StreamReader(ns);
StreamWriter writer = new StreamWriter(ns);

while (keepSending)
{
    Console.WriteLine("Enter method");
    string method = Console.ReadLine().ToLower();

    if (method == "stop")
    {
        keepSending = false;
    }

    int[] parameters = new int[0];
    if (method == "random" || method == "add" || method == "subtract")
    {
        Console.WriteLine("Enter num 1");
        int num1 = Int32.Parse(Console.ReadLine());
        Console.WriteLine("Enter num 2");
        int num2 = Int32.Parse(Console.ReadLine());
        parameters = new int[] { num1, num2 };
    }

    var message = new { Command = method, Parameters = parameters };
    string jsonMsg = JsonSerializer.Serialize(message);
    writer.WriteLine(jsonMsg);
    writer.Flush();

    string response = reader.ReadLine();
    var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(response);
    if (jsonResponse.ContainsKey("result"))
    {
        Console.WriteLine(jsonResponse["result"]);
    }
    else
    {
        Console.WriteLine("Unexpected response format");
    }
}

socket.Close();