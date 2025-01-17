﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TelegramBotBase.Builder;

namespace AsyncFormUpdates
{
    class Program
    {
        static TelegramBotBase.BotBase bot = null;

        static void Main(string[] args)
        {
            String apiKey = "APIKey";

            bot = BotBaseBuilder.Create()
                                .QuickStart<forms.Start>(apiKey)
                                .Build();

            bot.Start();

            var timer = new Timer(5000);

            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            Console.ReadLine();

            timer.Stop();
            bot.Stop();
        }

        private static async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            foreach(var s in bot.Sessions.SessionList)
            {
                //Only for AsyncUpdateForm
                if (s.Value.ActiveForm.GetType() != typeof(forms.AsyncFormUpdate) && s.Value.ActiveForm.GetType() != typeof(forms.AsyncFormEdit))
                    continue;

                await bot.InvokeMessageLoop(s.Key);
            }


        }
    }
}
