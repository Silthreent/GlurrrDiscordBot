﻿using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2.Commands
{
    public class Randome
    {
        static Dictionary<DiscordUser, List<string>> randomeList = new Dictionary<DiscordUser, List<string>>();

        public static async Task runCommand(MessageCreateEventArgs args)
        {
            bool commandFound = false;
            string msg = args.Message.Content.ToLower();

        // Add to Randome list
            if(commandFound == false && (msg.Contains("add") || msg.Contains("追加")))
            {
                commandFound = true;

                // Check to make sure they put an object
                string[] splitString = args.Message.Content.Split('"');
                if(splitString.Length < 3)
                {
                    splitString = args.Message.Content.Split('”');
                    if(splitString.Length < 3)
                    {
                        if(!CommandHandler.japanMode)
                            await args.Message.Channel.SendMessageAsync("Enter something to add to the pool in quotes!");
                        else
                            await args.Message.Channel.SendMessageAsync("引用符で囲んでプールに追加するものを入力してください！");

                        return;
                    }
                }

                // Add it to their list, if they don't have a list yet create them one first
                if(!randomeList.ContainsKey(args.Author))
                {
                    Console.WriteLine("Author entry did not exist");
                    randomeList.Add(args.Author, new List<string>());
                }

                randomeList[args.Author].Add(splitString[1].ToLower());


                await displayRandome(args.Message.Channel);
            }

            if(commandFound == false && msg.Contains("save"))
            {
                commandFound = true;


            }

        // Removes a specified listing from either your own or the first one found
            if(commandFound == false && (msg.Contains("delete") || msg.Contains("削除")))
            {
                commandFound = true;

                // Check to make sure they put an object
                string[] splitString = args.Message.Content.Split('"');
                if(splitString.Length < 3)
                {
                    splitString = args.Message.Content.Split('”');
                    if(splitString.Length < 3)
                    {
                        if(!CommandHandler.japanMode)
                            await args.Message.Channel.SendMessageAsync("Enter something to add to the pool in quotes!");
                        else
                            await args.Message.Channel.SendMessageAsync("引用符で囲んでプールに追加するものを入力してください！");

                        return;
                    }
                }

                if(msg.Contains("from my") || msg.Contains("じぶんの"))
                {
                    // Look for the specified object from their own list
                    if(randomeList.ContainsKey(args.Author))
                    {
                        if(randomeList[args.Author].Contains(splitString[1]))
                        {
                            randomeList[args.Author].Remove(splitString[1]);
                            Console.WriteLine("Deleted " + splitString[1] + " from " + args.Author.Username);
                            if(!CommandHandler.japanMode)
                                await args.Channel.SendMessageAsync("Deleted " + splitString[1] + " from " + args.Author.Username + "'s list");
                            else
                                await args.Channel.SendMessageAsync("削除済み " + splitString[1] + " から " + args.Author.Username + "〜の りすと");

                            return;
                        }
                        else
                        {
                            Console.WriteLine("Couldn't find " + splitString[1] + " on " + args.Author.Username + "'s list");
                            if(!CommandHandler.japanMode)
                                await args.Channel.SendMessageAsync("Couldn't find " + splitString[1] + " on " + args.Author.Username + "'s list");
                            else
                                await args.Channel.SendMessageAsync("見つかりませんでした " + splitString[1] + " に " + args.Author.Username + "〜の りすと");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find " + args.Author.Username + "'s list");

                        if(!CommandHandler.japanMode)
                            await args.Channel.SendMessageAsync("Couldn't find " + args.Author.Username + "'s list");
                        else
                            await args.Channel.SendMessageAsync("見つかりませんでした " + args.Author.Username + "〜の りすと");
                    }
                }
                else
                {
                    // Look for the specified object in all lists
                    foreach(DiscordUser i in randomeList.Keys)
                    {
                        if(randomeList[i].Contains(splitString[1]))
                        {
                            randomeList[i].Remove(splitString[1]);
                            Console.WriteLine("Deleted " + splitString[1] + " from " + i);
                            if(!CommandHandler.japanMode)
                                await args.Channel.SendMessageAsync("Deleted " + splitString[1] + " from " + i + "'s list");
                            else
                                await args.Channel.SendMessageAsync("削除済み " + splitString[1] + " から " + i + "〜の りすと");

                            return;
                        }
                    }

                    Console.WriteLine("Couldn't find " + splitString[1] + " anywhere");
                    if(!CommandHandler.japanMode)
                        await args.Channel.SendMessageAsync("Couldn't find " + splitString[1] + " anywhere");
                    else
                        await args.Channel.SendMessageAsync("見つかりませんでした " + splitString[1] + " どこでも");
                }
            }

        // Roll for randome
            if(commandFound == false && (msg.Contains("roll") || msg.Contains("ろる")))
            {
                commandFound = true;

                List<string> rollOptions = new List<string>();

                foreach(DiscordUser i in randomeList.Keys)
                {
                    foreach(string s in randomeList[i])
                    {
                        rollOptions.Add(i.Username + "'s choice of " + s);
                    }
                }

                if(!CommandHandler.japanMode)
                    await args.Message.Channel.SendMessageAsync("Let's see...");
                else
                    await args.Message.Channel.SendMessageAsync("どれどれ...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                if(!CommandHandler.japanMode)
                    await args.Message.Channel.SendMessageAsync("And the winner is...");
                else
                    await args.Message.Channel.SendMessageAsync("そして勝者は...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);
                await args.Message.Channel.SendMessageAsync("...");
                await Task.Delay(1000);

                Random random = new Random();
                await args.Message.Channel.SendMessageAsync(rollOptions[random.Next(0, rollOptions.Count - 1)] + "!");
            }
            
        // Display the randome list
            if(commandFound == false && (msg.Contains("list") || msg.Contains("りすと")))
            {
                commandFound = true;
                await displayRandome(args.Message.Channel);
            }

        // Clear randome list
            if(commandFound == false && (msg.Contains("clear") || msg.Contains("くりあ")))
            {
                commandFound = true;
                randomeList.Clear();

                if(!CommandHandler.japanMode)
                    await args.Message.Channel.SendMessageAsync("Randome lists cleared");
                else
                    await args.Message.Channel.SendMessageAsync("ランドームリストがクリアされました");
            }
        }

        async static Task displayRandome(DiscordChannel channel)
        {
            if(randomeList.Keys.Count == 0)
            {
                Console.WriteLine("Empty Randome list");
                if(!CommandHandler.japanMode)
                    await channel.SendMessageAsync("Randome list is empty, nothing to display");
                else
                    await channel.SendMessageAsync("ランドームリストは空で、表示するものはありません");

                return;
            }

            Console.WriteLine("Displaying results");
            string builder = "";
            foreach(DiscordUser i in randomeList.Keys)
            {
                builder += i.Username + " - {";
                foreach(string s in randomeList[i])
                {
                    builder += s + ", ";
                }
                builder = builder.Remove(builder.Length - 2);
                builder += "}\n";
            }

            Console.WriteLine(builder);
            await channel.SendMessageAsync(builder);
        }
    }
}
