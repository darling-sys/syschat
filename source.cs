using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace syschat
{
	public class constance // helpful constance gives us info
	{
		public static readonly string version = "0.1.4";
	}
	public class Program
	{
		public static void Main(string[] args)
		{
			data d = new data();
			while(true)
			{
				printRoom(d);
				ConsoleKeyInfo pressedKey = Console.ReadKey(true);
				if(pressedKey.KeyChar == '\\')
				{
					Console.Clear();
					Console.Write("\\");
					string input = Console.ReadLine();
					input = "\\" + input;
					input = input.Trim();
					d = commandHandler(d, input);
				}
				if(pressedKey.KeyChar == '/')
				{
					Console.Clear();
					Console.Write("/");
					string input = Console.ReadLine();
					input = "\\" + input;
					input = input.Trim();
					d = commandHandler(d, input);
				}
				else if((pressedKey.Key == ConsoleKey.Enter) || (pressedKey.Key == ConsoleKey.Escape))
				{
					Console.Clear();
					Console.Write(d.myName + " > ");
					string input = Console.ReadLine();
					input = input.Trim();
					if(input.StartsWith("\\"))
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
		public static data commandHandler(data d, string input)
		{
			switch(input)
			{
				case "\\help":
				{
					Console.Clear();
					Console.WriteLine("Syschat Help: ");
					Console.WriteLine("");
					Console.WriteLine("press enter to start writing a message");
					Console.WriteLine("press the enter key to send it");
					Console.WriteLine("");
					Console.WriteLine("using backslash before a message turns it into a command");
					Console.WriteLine("");
					Console.WriteLine("available commands:");
					Console.WriteLine("front / switch / rp / name / swap: all 5 of these change the name");
					Console.WriteLine("dump / log / dumplog : all 3 of these save the chat log to a file");
					Console.WriteLine("ver / version : both of these print the syschat version number");
					Console.WriteLine("clear : this clears out the chat history without closing");
					Console.WriteLine("quit / end : both of these close syschat without saving");
					Console.WriteLine("nvm / no / n : all 3 of these do nothing");
					Console.WriteLine("");
					Console.WriteLine("press any key to go back to chat now");
					Console.WriteLine("");
					Console.ReadKey();
					return d;
				}
				case "\\clear":
				{
					d = clearMessages(d);
					return d;
				}
				case "\\nvm":
				{
					return d;
				}
				case "\\no":
				{
					return d;
				}
				case "\\n":
				{
					return d;
				}
				case "\\end":
				{
					Environment.Exit(0);
					return d;
				}
				case "\\quit":
				{
					Environment.Exit(0);
					return d;
				}
				case "\\version":
				{
					d = addMessage(d, "system", constance.version);
					return d;
				}
				case "\\ver":
				{
					d = addMessage(d, "system", constance.version);
					return d;
				}
				case "\\front":
				{
					Console.Clear();
					Console.Write("name: ");
					string newName = Console.ReadLine();
					d.myName = newName.Trim();
					d = addMessage(d, "system", newName + " swapped in");
					return d;
				}
				case "\\switch":
				{
					Console.Clear();
					Console.Write("name: ");
					string newName = Console.ReadLine();
					d.myName = newName.Trim();
					d = addMessage(d, "system", newName + " swapped in");
					return d;
				}
				case "\\swap":
				{
					Console.Clear();
					Console.Write("name: ");
					string newName = Console.ReadLine();
					d.myName = newName.Trim();
					d = addMessage(d, "system", newName + " swapped in");
					return d;
				}
				case "\\name":
				{
					Console.Clear();
					Console.Write("name: ");
					string newName = Console.ReadLine();
					d.myName = newName.Trim();
					d = addMessage(d, "system", newName + " swapped in");
					return d;
				}
				case "\\rp":
				{
					Console.Clear();
					Console.Write("name: ");
					string newName = Console.ReadLine();
					d.myName = newName.Trim();
					d = addMessage(d, "system", newName + " swapped in");
					return d;
				}
				case "\\dump":
				{
					dumplog(d, "log.txt");
					return d;
				}
				case "\\log":
				{
					dumplog(d, "log.txt");
					return d;
				}
				case "\\dumplog":
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
			d.messages.Add(m);
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
			return d;
		}
	}
	public class data
	{
		public string myName = "me";
		public List<message> messages = new List<message>();
	}
	public class message
	{
		public string fromName;
		public string content;
	}
}

