using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace syschat
{
	public class constance
	{
		public static readonly string version = "0.2.6";
		public static readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar +"syschat" + Path.DirectorySeparatorChar + "log.data";
		public static readonly string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar +"syschat";
	}
	public class Program
	{
		public static void Main(string[] args)
		{
			data d = new data();
			data f = new data();
			if(File.Exists(constance.path))
			{
				try
				{
					f = deserialize(constance.path);
				}
				catch
				{
					f = d; // data corrupted! just make it anew!
				}
				d = f; // load the data into a usable object
			}
			else
			{
				if(!(Directory.Exists(constance.folder)))
				{
					Directory.CreateDirectory(constance.folder);
				}
				File.Create(constance.path);
			}
			d = clearSystemMessages(d);
			while(true)
			{
				printRoom(d);
				ConsoleKeyInfo pressedKey = Console.ReadKey(true);
				if(pressedKey.KeyChar == '\\')
				{
					Console.Clear();
					Console.Write(d.myName + " > \\");
					string input = Console.ReadLine();
					input = "\\" + input;
					input = input.Trim();
					d = commandHandler(d, input);
				}
				else if(pressedKey.KeyChar == '/')
				{
					Console.Clear();
					Console.Write(d.myName + " > /");
					string input = Console.ReadLine();
					input = "/" + input;
					input = input.Trim();
					d = commandHandler(d, input);
				}
				else if((pressedKey.Key == ConsoleKey.Enter) || (pressedKey.Key == ConsoleKey.Escape))
				{
					Console.Clear();
					Console.Write(d.myName + " > ");
					string input = Console.ReadLine();
					input = input.Trim();
					if(input.StartsWith("\\") || input.StartsWith("/"))
					{
						d = commandHandler(d, input);
					}
					else
					{
						d = addMessage(d, d.myName, input);
					}
				}
			}
		}
		public static data deserialize(string path)
		{
			data d = new data();
			using(FileStream fs = new FileStream(constance.path, FileMode.Open))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				d = (data)formatter.Deserialize(fs);
			}
			return d;
		}
		public static data serialize(data d, string path)
		{
			using(FileStream fs = new FileStream(constance.path, FileMode.Create))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(fs, d);
			}
			return d;
		}
		public static data swapout(data d)
		{
			Console.Clear();
			Console.Write("name: ");
			string newName = Console.ReadLine();
			if(newName != "")
			{
				d.myName = newName.Trim();
				d = addMessage(d, "system", newName + " swapped in");
			}
			return d;
		}
		public static data helper(data d)
		{
			Console.Clear();
			Console.WriteLine("Syschat Help: ");
			Console.WriteLine("");
			Console.WriteLine("press enter to start writing a message");
			Console.WriteLine("press enter again to send it");
			Console.WriteLine("");
			Console.WriteLine("slash before a message turns it into a command");
			Console.WriteLine("");
			Console.WriteLine("available commands:");
			Console.WriteLine("front / switch / rp / name / swap: these change the name");
			Console.WriteLine("dump / log / dumplog : these save the chat log to a file");
			Console.WriteLine("ver / version : these print the syschat version number");
			Console.WriteLine("clear : this clears out the chat history without closing");
			Console.WriteLine("quit / end / close : these close syschat without saving");
			Console.WriteLine("nvm / no / n : these do nothing");
			Console.WriteLine("");
			Console.WriteLine("press any key to go back to chatting now");
			Console.WriteLine("");
			Console.ReadKey();
			return d;
		}
		public static data commandHandler(data d, string input)
		{
			input = input.Remove(0, 1);
			switch(input)
			{
				case "help":
				{
					d = helper(d);
					return d;
				}
				case "clear":
				{
					d = clearMessages(d);
					return d;
				}
				case "nvm":
				{
					return d;
				}
				case "no":
				{
					return d;
				}
				case "n":
				{
					return d;
				}
				case "close":
				{
					Environment.Exit(0);
					return d;
				}
				case "end":
				{
					Environment.Exit(0);
					return d;
				}
				case "quit":
				{
					Environment.Exit(0);
					return d;
				}
				case "version":
				{
					d = addMessage(d, "system", constance.version);
					return d;
				}
				case "ver":
				{
					d = addMessage(d, "system", constance.version);
					return d;
				}
				case "front":
				{
					d = swapout(d);
					return d;
				}
				case "switch":
				{
					d = swapout(d);
					return d;
				}
				case "swap":
				{
					d = swapout(d);
					return d;
				}
				case "name":
				{
					
					d = swapout(d);
					return d;
				}
				case "rp":
				{
					d = swapout(d);
					return d;
				}
				case "dump":
				{
					dumplog(d, "log.txt");
					return d;
				}
				case "log":
				{
					dumplog(d, "log.txt");
					return d;
				}
				case "dumplog":
				{
					dumplog(d, "log.txt");
					return d;
				}
				default:
				{
					d = addMessage(d, "system", "unknown command");
					return d;
				}
			}
		}
		public static data clearSystemMessages(data d)
		{
			List<message> mess = d.messages;
			mess.RemoveAll(message => message.fromName == "system");
			d.messages = mess;
			return d;
		}
		public static void dumplog(data d, string path)
		{
			string full = "";
			foreach(message m in d.messages)
			{
				full = full + m.fromName + " : " + m.content + "\n";
			}
			File.WriteAllText(path, full);
		}
		public static data addMessage(data d, string playerName, string messageContent)
		{
			message m = new message();
			m.fromName = playerName;
			m.content = messageContent;
			if(messageContent != "")
			{
				d.messages.Add(m);
			}
			serialize(d, constance.path);
			return d;
		}
		public static void printRoom(data d)
		{
			Console.Clear();
			foreach(message m in d.messages)
			{
				Console.WriteLine(m.fromName + " : " + m.content);
			}
		}
		public static data clearMessages(data d)
		{
			List<message> m = new List<message>();
			d.messages = m;
			serialize(d, constance.path);
			return d;
		}
	}
	[Serializable]
	public class data
	{
		public string myName = "me";
		public List<message> messages = new List<message>();
	}
	[Serializable]
	public class message
	{
		public string fromName;
		public string content;
	}
}

